using ChatRoomServer.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatRoomServer.Models;
using DAL;
using DAL.Models;

namespace ChatRoomServer.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        TalkBackDBContext _context;

        public UsersRepository(TalkBackDBContext context)
        {
            _context = context;
        }

        public void AddNewUser(UserModel user)
        {
            User newUser = new User{ Name = user.Name, Password = user.Password };
            _context.User.Add(newUser);
            _context.SaveChanges();
        }

        public IList<AbstractXmlSerializable> GetContactsList(string user, ISet<string> onlineSet)
        {
            IList<AbstractXmlSerializable> contacts = new List<AbstractXmlSerializable>();
            foreach (User u in _context.User.ToList<User>())
            {
                if (!u.Name.Equals(user))
                {
                    contacts.Add(new Contact { Name = u.Name, IsOnline = onlineSet.Contains(u.Name) });
                }
            }
            return contacts;
        }

        public bool IsPasswordCorrect(string username, string providedPpassword)
        {
            User u = _context.User.Find(username);
            if (u == null)
            {
                return false; // should never happen as we always ensure that user exists
            }
            return u.Password.Equals(providedPpassword);
        }

        public bool IsUserExist(string userName)
        {
            User u = _context.User.Find(userName);
            return (u != null);
        }
    }
}
