using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class ScheduleHub : Hub
    {
        public async Task SendScheduleUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Schedule Update", message);
        }
    }
}
