using LiveChat.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class ChatService
{
    static readonly Dictionary<string, string> Users = new Dictionary<string, string>();

    public bool AddUserToOnlineList(string newUser)
    {
        lock (Users)
        {
            foreach (var user in Users)
            {
                if (user.Key.ToLower() == newUser.ToLower())
                {
                    return false;
                }
            }
            Users.Add(newUser, null);
            return true;
        }
    }
    public void AddUserConnection(string user, string connectionId)
    {
        lock (Users)
        {
            if (Users.ContainsKey(user))
            {
                Users[user] = connectionId;
            }
        }
    }
    public string getUserByConnId(string connectionId)
    {
        lock (Users)
        {
            return Users.Where(x => x.Value == connectionId).Select(x => x.Key).FirstOrDefault();
        }
    }
    public string getConnIdByUser(string user)
    {
        lock (Users)
        {
            return Users.Where(x => x.Value == user).Select(x => x.Value).FirstOrDefault();
        }
    }
    public void removeUser(string user)
    {
        lock (Users)
        {
            if (Users.ContainsKey(user))
            {
                Users.Remove(user);
            }
        }
    }
    public string[] getOnlineUsers()
    {
        lock (Users)
        {
            return Users.OrderBy(x => x.Key).Select(x => x.Key).ToArray();
        }
    } 
}