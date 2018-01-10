using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChatRoomServer
{
    public class WebSocketMiddleware
    {
        public static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            CancellationToken ct = context.RequestAborted;
            IServiceProvider sp = context.RequestServices;
            TalkBackDBContext dbContext = sp.GetService<TalkBackDBContext>();

            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
            //var socketId = Guid.NewGuid().ToString();

            //_sockets.TryAdd(socketId, currentSocket);

            MessageHandler handler = new MessageHandler(currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                var response = await ReceiveStringAsync(currentSocket, ct);
                if (string.IsNullOrEmpty(response))
                {
                    if (currentSocket.State != WebSocketState.Open)
                    {
                        break;
                    }

                    continue;
                }

                //handling the message from the client
                handler.HandleMessage(response, dbContext);

                //foreach (var socket in _sockets)
                //{
                //    if (socket.Value.State != WebSocketState.Open)
                //    {
                //        continue;
                //    }

                //    await SendStringAsync(socket.Value, response, ct);
                //}
            }

            handler.HandleUserDisconnect();

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
            dbContext.Dispose();
        }

        public static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }

        private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    try
                    {
                        result = await socket.ReceiveAsync(buffer, ct);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
