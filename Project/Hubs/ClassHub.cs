using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class ClassHub : Hub
    {
        public async Task SendClassUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Class Update", message);
        }
    }
}
