using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class StudentHub : Hub
    {
        public async Task SendStudentUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Student Update", message);
        }

        public async Task SendStudentUpdateToClass(string message)
        {
            await Clients.All.SendAsync("Receive Student To Class Update", message);
        }
    }
}
