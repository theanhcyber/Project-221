using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class SubjectHub : Hub
    {
        public async Task SendSubjectUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Subject Update", message);
        }
    }
}
