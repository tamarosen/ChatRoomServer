using ChatRoomServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatRoomServer.Infra
{
    public interface IUsersRepository
    {
        bool IsUserExist(string userName);
        bool IsPasswordCorrect(string userName, string providedPassword);
        void AddNewUser(UserModel user);
        IList<AbstractXmlSerializable> GetContactsList(string user, ISet<string> onlineSet);
    }
}
