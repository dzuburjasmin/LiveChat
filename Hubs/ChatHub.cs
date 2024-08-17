using LiveChat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LiveChat.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private readonly ApplicationDbContext _context;
        private static readonly Dictionary<string, int> messageCounts = new Dictionary<string, int>();
        private static readonly TimeSpan resetInterval = TimeSpan.FromMinutes(1);
        private static readonly int limit = 10;
        private static Timer resetTimer;

        public ChatHub(ChatService chatService, ApplicationDbContext context)
        {
            _chatService = chatService;
            _context = context;
            if (resetTimer == null)
            {
                resetTimer = new Timer(ResetMessageCounts, null, resetInterval, resetInterval);
            }
        }
        private void ResetMessageCounts(object state)
        {
            messageCounts.Clear();
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
            var messageHistory = _context.Messages.Where(m=>m.ChatName=="LiveChat").OrderByDescending(m => m.DateTime).Take(10).Reverse().ToList();
            _chatService.AddUserConnection(userName, Context.ConnectionId);
            var onlineUsers = _chatService.getOnlineUsers();
            await Clients.Groups("LiveChat").SendAsync("OnlineUsers", onlineUsers);
            await Clients.Caller.SendAsync("MessageHistory", messageHistory);
        }
        public async Task ReceiveMessage(MessageDTO  mess)
        {
            //limiter
            if (messageCounts.ContainsKey(Context.ConnectionId))
            {
                if (messageCounts[Context.ConnectionId] >= limit)
                {
                    await Clients.Caller.SendAsync("MessageLimitReached");
                    return;
                }

                messageCounts[Context.ConnectionId]++;
            }
            else
            {
                messageCounts[Context.ConnectionId] = 1;
            }
            //limiter
            try
            {

            Message message = new Message();
            message.Id = Guid.NewGuid();
            message.ChatName = "LiveChat";
            message.User = mess.User;
            message.Receiver = mess.Receiver;
            message.Text = mess.Text;
            message.DateTime = mess.DateTime;

            await SaveMessageToDb(message);

            await Clients.Group("LiveChat").SendAsync("NewMessage", mess);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public async Task CreatePrivateChat(MessageDTO mess)
        {
            string chatName = GetPrivateChatName(mess.User, mess.Receiver);
            Message message = new Message();
            message.ChatName = chatName;
            message.User = mess.User;
            message.Receiver = mess.Receiver;
            message.Text = mess.Text;
            message.DateTime = mess.DateTime;

            await SaveMessageToDb(message);

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
            Message message = new Message();
            message.ChatName = chatName;
            message.User = mess.User;
            message.Receiver = mess.Receiver;
            message.Text = mess.Text;
            message.DateTime = mess.DateTime;

            await SaveMessageToDb(message);

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

        public async Task SaveMessageToDb(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }

        public async Task LogoutUser()
        {
            await OnDisconnectedAsync(null);
        }
    }
}
