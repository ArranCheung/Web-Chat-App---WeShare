using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Web_Chat_App___WeShare.Databases;

namespace Web_Chat_App___WeShare.Hubs
{
    public class ChatHub : Hub
    {
        private readonly CDDB _context;

        public ChatHub(CDDB context)
        {
            _context = context;
        }

        public async Task ReceiveID(string username)
        {

            if (!string.IsNullOrEmpty(username))
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

                if (user != null)
                {
                    user.conID = Context.ConnectionId;
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task SendMessage(string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageTo(string user, string message)
        {
            AccountAuth? userAcc = await _context.Users.SingleOrDefaultAsync(u => u.Username == user);
            AccountAuth? sender = await _context.Users.SingleOrDefaultAsync(u => u.conID == Context.ConnectionId);

            if (userAcc != null && sender != null)
            {
                string username = userAcc.Username;

                await Clients.User(username).SendAsync("ReceiveMessageFrom", message, sender.Username);
            }
        }
    }
}