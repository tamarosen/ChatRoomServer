using ChatRoomServer.Models;
using ChatRoomServer.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ChatRoomServer.Models.ModelXmlMapper;

namespace ChatRoomServer
{
    public class MessageHandler
    {
        private WebSocket _webSocket;

        public string UserName { get; set; }

        public MessageHandler(WebSocket ws)
        {
            UserName = null;
            _webSocket = ws;
        }

        public void HandleMessage(string incommingMessage, TalkBackDBContext dbContext)
        {
            MappedType t = MappedType.UNDEFINED;
            XDocument serialized = XDocument.Parse(incommingMessage);
            XElement typeElement = serialized.Root.Element("Type");
            if (typeElement == null)
            {
                // ignore
                return;
            }
            string type = typeElement.Value;
            if (ModelXmlMapper.map.TryGetValue(type, out t))
            {
                switch (t)
                {
                    case MappedType.MESSAGE:
                        MessageModel msg = new MessageModel();
                        if (msg.FromXml(serialized.Root))
                        {
                            HandleChatMessageMsg(msg, dbContext);
                        }
                        break;
                    case MappedType.USER:
                        UserModel user = new UserModel();
                        user.FromXml(serialized.Root);
                        break;
                    case MappedType.CONTACT:
                        Contact contact = new Contact();
                        contact.FromXml(serialized.Root);
                        break;
                    case MappedType.CHAT_REQUEST:
                        ChatRequest req = new ChatRequest();
                        if (req.FromXml(serialized.Root))
                        {
                            HandleChatRequestMsg(req, dbContext);
                        }
                        break;
                    case MappedType.CHAT_REQUEST_RESPONSE:
                        ChatRequestResponse resp = new ChatRequestResponse();
                        if (resp.FromXml(serialized.Root))
                        {
                            HandleChatResponseMsg(resp, dbContext);
                        }
                        break;
                    case MappedType.LOGIN:
                        Login login = new Login();
                        if (login.FromXml(serialized.Root))
                        {
                            HandleLoginMsg(login, dbContext);
                        }
                        break;
                    case MappedType.LOGIN_RESPONSE:
                        LoginResponse loginResp = new LoginResponse();
                        loginResp.FromXml(serialized.Root);
                        break;
                    case MappedType.UNDEFINED:
                        throw new Exception("Don't know how to parse this type");
                }
            }
        }

        private void HandleChatMessageMsg(MessageModel msg, TalkBackDBContext dbContext)
        {
            // first check that the addressee is in the dictionary:
            WebSocket peer;
            if (!WebSocketMiddleware._sockets.TryGetValue(msg.To, out peer))
            {
                // not found -> send a ChatRequestResponse with failure:
                ChatRequestResponse resp = new ChatRequestResponse { Success = false, ErrorMessage = "Chat peer is not online" };
                WebSocketMiddleware.SendStringAsync(_webSocket, resp.ToXml().ToString());
                return;
            }

            // save message in the log:
            Message dbMessage = new Message
            {
                Content = msg.Content,
                ReceiverName = msg.To,
                SenderName = msg.From,
                Time = DateTime.Now
            };

            try
            {
                dbContext.Message.Add(dbMessage);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //do nothing
            }

            // send the message to both sender and receiver:
            string strMsg = msg.ToXml().ToString();
            WebSocketMiddleware.SendStringAsync(_webSocket, strMsg);
            WebSocketMiddleware.SendStringAsync(peer, strMsg);

            return;
            }

        private void HandleChatRequestMsg(ChatRequest req, TalkBackDBContext dbContext)
        {
            // first check that the addressee is in the dictionary:
            WebSocket peer;
            if (!WebSocketMiddleware._sockets.TryGetValue(req.To, out peer))
            {
                // not found -> send a ChatRequestResponse with failure:
                ChatRequestResponse resp = new ChatRequestResponse { Success = false, ErrorMessage = "Chat peer is not online" };
                WebSocketMiddleware.SendStringAsync(_webSocket, resp.ToXml().ToString());
                return;
            }

            string strMsg = req.ToXml().ToString();
            WebSocketMiddleware.SendStringAsync(peer, strMsg);

            return;
        }

        private void HandleChatResponseMsg(ChatRequestResponse resp, TalkBackDBContext dbContext)
        {
            // first check that the addressee is in the dictionary:
            WebSocket peer;
            if (!WebSocketMiddleware._sockets.TryGetValue(resp.To, out peer))
            {
                // not found -- ignore
                return;
            }

            string strMsg = resp.ToXml().ToString();
            WebSocketMiddleware.SendStringAsync(peer, strMsg);

            return;
        }


        internal void HandleUserDisconnect()
        {
            if (UserName != null)
            {
                WebSocket dummy;
                // remove from online dictionary:
                WebSocketMiddleware._sockets.TryRemove(UserName, out dummy);
                // send all online clients contact update message:
                Broadcast(new Contact { Name = UserName, IsOnline = false }.ToXml().ToString());
            }
        }

        private void HandleLoginMsg(Login login, TalkBackDBContext dbContext)
        {
            UsersRepository repo = new UsersRepository(dbContext);

            // check if User Exists:
            if (repo.IsUserExist(login.Name))
            {
                // check password:
                if (!repo.IsPasswordCorrect(login.Name, login.Password))
                {
                    LoginResponse response = new LoginResponse { Success = false, ErrorMessage = "Wrong password, please try again" };
                    WebSocketMiddleware.SendStringAsync(_webSocket, response.ToXml().ToString());
                    return;
                }
            }
            else
            {
                // new user: add it to repo
                repo.AddNewUser(new UserModel { Name = login.Name, Password = login.Password });
            }

            // login successfully:
            // 1. record UserName for this handler
            UserName = login.Name;
            // 2. add to the websocket dictionary
            WebSocketMiddleware._sockets.TryAdd(UserName, _webSocket);
            // 3. inform all online users about the new/updated contact,
            //    create a Set<string> of online users:
            ISet<string> online = new HashSet<string>();
            Contact me = new Contact { Name = UserName, IsOnline = true };
            string contactUpdate = me.ToXml().ToString();
            online = Broadcast(contactUpdate);
            // 4. send the client the list of contacts:
            IList<AbstractXmlSerializable> contacts = repo.GetContactsList(UserName, online);
            string contactsList = ModelXmlMapper.GetAsXmlString(contacts);
            WebSocketMiddleware.SendStringAsync(_webSocket, contactsList);
        }

        private ISet<string> Broadcast(string msg)
        {
            ISet<string> online = new HashSet<string>();

            foreach (var entry in WebSocketMiddleware._sockets)
            {
                if (!entry.Key.Equals(UserName))
                {
                    online.Add(entry.Key);
                    WebSocketMiddleware.SendStringAsync(entry.Value, msg);
                }
            }

            return online;
        }
    }
}
