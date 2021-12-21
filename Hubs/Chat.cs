using Microsoft.AspNetCore.SignalR;

namespace Stack.Hubs
{
    public class Chat:Hub
    {
        public async Task SendMessage(string uname, string msg)
        {
            await Clients.All.SendAsync("RecieveMessage",uname,msg);
        }
    }
}
