using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class TeacherHub: Hub
    {
        public async Task SendTeacherUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Teacher Update", message);
        }
    }
}
