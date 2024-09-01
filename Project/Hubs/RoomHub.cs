using Microsoft.AspNetCore.SignalR;

namespace Project.Hubs
{
    public class RoomHub :Hub
    {
        public async Task SendRoomUpdate(string message)
        {
            await Clients.All.SendAsync("Receive Room Update", message);
        }
    }
}
