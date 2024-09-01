using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;
using Project.Models;

namespace Project.Pages.Room
{
    public class ListModel : PageModel
    {
        private ProjectPRN221Context _context;
        private readonly IHubContext<RoomHub> _hubContext;
        [BindProperty]
        public string RoomName { get; set; }
        public List<Models.Room> Rooms { get; set; }

        [BindProperty]
        public string SearchName { get; set; }
        [BindProperty]
        public string SortOrder { get; set; }
        [BindProperty]
        public string SortType { get; set; }
        public List<String> SortOrders { get; set; }
        public List<String> SortTypes { get; set; }
        public string SelectedSortType { get; set; }
        public string SelectedSortOrder { get; set; }

        public ListModel(ProjectPRN221Context context, IHubContext<RoomHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            LoadData();
        }

        private void LoadData()
        {
            Rooms = _context.Rooms.ToList();
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "RoomId", "RoomName" };
        }

        public void OnGet()
        {
            LoadData();
        }

        public void OnPostSearchRoom()
        {
            if (!string.IsNullOrEmpty(SearchName))
            {
                Rooms = _context.Rooms.Where(r => r.Name.Contains(SearchName)).ToList();
            }
            else
            {
                Rooms = _context.Rooms.ToList();
            }

            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SelectedSortType)
                {
                    case "RoomId":
                        if (SelectedSortOrder == "Asc")
                        {
                            Rooms = Rooms.OrderBy(r => r.Id).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Rooms = Rooms.OrderByDescending(r => r.Id).ToList();
                        }
                        break;
                    case "RoomName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Rooms = Rooms.OrderBy(r => r.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Rooms = Rooms.OrderByDescending(r => r.Name).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }

        public async Task OnPostAddRoomAsync()
        {
            if (!string.IsNullOrEmpty(RoomName))
            {
                var existRoom = _context.Rooms.FirstOrDefault(r => r.Name == RoomName);
                if (existRoom != null)
                {
                    await _hubContext.Clients.All.SendAsync("Receive Room Update", "Room existed.");
                }
                else
                {
                    var newRoom = new Models.Room { Name = RoomName };
                    _context.Rooms.Add(newRoom);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Room Update", "A new room has been added successfully.");
                }
            }

            LoadData();
        }

        public async Task OnPostEditRoomAsync(int RoomID, string Name)
        {
            var existingRoom = _context.Rooms.FirstOrDefault(r => r.Name == Name && r.Id != RoomID);
            if (existingRoom != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Room Update", "Room name existed.");
            }
            else
            {
                var selectRoom = _context.Rooms.FirstOrDefault(r => r.Id == RoomID);

                if (selectRoom != null)
                {
                    selectRoom.Name = Name;
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Room Update", "A room has been updated.");
                }
            }
            LoadData();
        }

        public async Task OnPostDeleteRoomAsync(int RoomID)
        {
            var selectRoom = _context.Rooms.FirstOrDefault(r => r.Id == RoomID);
            var existSchedule = _context.Schedules.FirstOrDefault(r => r.RoomId == RoomID);
            if (existSchedule != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Room Update", "Room's currently in use!");
            }
            else
            {
                if (selectRoom != null)
                {
                    _context.Rooms.Remove(selectRoom);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Room Update", "A room has been deleted successfully.");
                }
            }
            LoadData();
        }
    }
}
