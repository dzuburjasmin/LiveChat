using System.Runtime.InteropServices;
using LiveChat.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace LiveChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "LiveChat");
            await Clients.Caller.SendAsync("Connected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "LiveChat");
            await base.OnDisconnectedAsync(exception);
            var user = _chatService.getUserByConnId(Context.ConnectionId);
            _chatService.removeUser(user);
            var onlineUsers = _chatService.getOnlineUsers();
            await Clients.Groups("LiveChat").SendAsync("OnlineUsers", onlineUsers);
        }

        public async Task AddUserConnectionId(string userName)
        {
            _chatService.AddUserConnection(userName, Context.ConnectionId);
            var onlineUsers = _chatService.getOnlineUsers();
            await Clients.Groups("LiveChat").SendAsync("OnlineUsers", onlineUsers);
        }
        public async Task ReceiveMessage(MessageDTO  mess)
        {
            await Clients.Group("LiveChat").SendAsync("NewMessage", mess);
        }
        public async Task CreatePrivateChat(MessageDTO mess)
        {
            string chatName = GetPrivateChatName(mess.User, mess.Receiver);
            await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
            var receiverId = _chatService.getConnIdByUser(mess.Receiver);
            await Groups.AddToGroupAsync(receiverId, chatName);
            await Clients.Client(receiverId).SendAsync("OpenPrivateChat", mess);
        }

        private string GetPrivateChatName (string sender, string receiver)
        {
            var compareAlphabetic = string.CompareOrdinal(sender, receiver) < 0;
            return compareAlphabetic ? $"{sender}-{receiver}" : $"{receiver}-{sender}";
        }
        public async Task ReceivePrivate(MessageDTO mess)
        {
            string chatName = GetPrivateChatName(mess.User, mess.Receiver);
            await Clients.Group(chatName).SendAsync("NewPrivateMessage", mess);
        }
        public async Task RemovePrivateChat(string sender, string receiver)
        {
            string chatName = GetPrivateChatName(sender, receiver);
            await Clients.Group(chatName).SendAsync("ClosePrivateChat");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
            var receiverId = _chatService.getConnIdByUser(receiver);
            await Groups.RemoveFromGroupAsync(receiverId, chatName);

        }
    }
}
