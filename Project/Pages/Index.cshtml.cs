using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using System.Linq;
using System.Xml.Schema;
using System.Xml;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;

namespace Project.Pages
{
    public class IndexModel : PageModel
    {
        private ProjectPRN221Context _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<ScheduleHub> _hubContext;

        private string result;

        public List<Models.Schedule> Schedules { get; set; }
        public List<Models.Class> Classes { get; set; }
        public List<Models.Teacher> Teachers { get; set; }
        public List<Models.Subject> Subjects { get; set; }
        public List<Models.Room> Rooms { get; set; }
        [BindProperty]
        public int ScheduleId { get; set; }
        [BindProperty]
        public int TeacherId { get; set; }
        [BindProperty]
        public int RoomId { get; set; }
        [BindProperty]
        public int ClassId { get; set; }
        [BindProperty]
        public int SubjectId { get; set; }
        [BindProperty]
        public int DayOfWeek { get; set; }
        [BindProperty]
        public int Slot { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public string SearchName { get; set; }
        [BindProperty]
        public string SearchClass { get; set; }
        [BindProperty]
        public string SearchRoom { get; set; }
        [BindProperty]
        public string SearchSubject { get; set; }
        [BindProperty]
        public string SearchDay { get; set; }
        [BindProperty]
        public string SearchSlot { get; set; }
        [BindProperty]
        public string SortOrder { get; set; }
        [BindProperty]
        public string SortType { get; set; }
        public List<String> SortOrders { get; set; }
        public List<String> SortTypes { get; set; }
        public string SelectedSortType { get; set; }
        public string SelectedSortOrder { get; set; }
        public List<String> SearchSlots {  get; set; }
        public List<String> SearchDays { get; set; }

        public IndexModel(ProjectPRN221Context context, IWebHostEnvironment environment, IHubContext<ScheduleHub> hubContext)
        {
            _context = context;
            _environment = environment;
            _hubContext = hubContext;
            LoadData();
        }

        private void LoadData()
        {
            Schedules = new List<Models.Schedule>();
            var activeTeachers = _context.Teachers.Where(t => t.Active == 1).Select(t => t.Id).ToList();
            Schedules = _context.Schedules
        .Include(c => c.Class)
        .Include(sub => sub.Subject)
        .Include(t => t.Teacher)
        .Include(r => r.Room)
        .Where(s => activeTeachers.Contains((int)s.TeacherId))
        .OrderBy(s => s.DayOfWeek)
        .ThenBy(s => s.Slot)
        .ToList();

            Teachers = _context.Teachers.Where(t => t.Active == 1).ToList();
            Subjects = _context.Subjects.ToList();
            Classes = _context.Classes.ToList();
            Rooms = _context.Rooms.ToList();
            SearchSlots = new List<String> { "All", "Slot 1 (7:30 - 9:50)", "Slot 2 (10:00 - 12:20)", "Slot 3 (12:50 - 15:00)", "Slot 4 (15:10 - 17:20)" };
            SearchDays = new List<String> {"All", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "TeacherName", "Class", "Subject", "Room", "TimeSlot", "DayOfWeek" };
        }

        public void OnGet()
        {
            LoadData();
        }

        private int ConvertDay(string sday)
        {
            switch (sday)
            {
                case "Monday":
                    return 2;
                case "Tuesday":
                    return 3;
                case "Wednesday":
                    return 4;
                case "Thursday":
                    return 5;
                case "Friday":
                    return 6;
                case "Saturday":
                    return 7;
                case "Sunday":
                    return 8;
                default: 
                    return 0;
            }
        }

        private int ConvertSlot(string sslot)
        {
            switch (sslot)
            {
                case "Slot 1 (7:30 - 9:50)":
                    return 1;
                case "Slot 2 (10:00 - 12:20)":
                    return 2;
                case "Slot 3 (12:50 - 15:00)":
                    return 3;
                case "Slot 4 (15:10 - 17:20)":
                    return 4;
                default:
                    return 0;
            }
        }

        public void OnPostSearchSchedule()
        {
            IQueryable<Models.Schedule> query = _context.Schedules.Include(s => s.Teacher)
                                                    .Include(s => s.Class)
                                                    .Include(s => s.Room)
                                                    .Include(s => s.Subject);

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(s => s.Teacher.Name.Contains(SearchName));
            }

            if (!string.IsNullOrEmpty(SearchClass))
            {
                query = query.Where(s => s.Class.Name == SearchClass);
            }

            if (!string.IsNullOrEmpty(SearchRoom))
            {
                query = query.Where(s => s.Room.Name == SearchRoom);
            }

            if (!string.IsNullOrEmpty(SearchSubject))
            {
                query = query.Where(s => s.Subject.Name == SearchSubject);
            }

            if (!string.IsNullOrEmpty(SearchSlot) && SearchSlot != "All")
            {
                query = query.Where(s => s.Slot == ConvertSlot(SearchSlot));
            }

            if (!string.IsNullOrEmpty(SearchDay) && SearchDay != "All")
            {
                query = query.Where(s => s.DayOfWeek == ConvertDay(SearchDay));
            }

            Schedules = query.ToList();


            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SortType)
                {
                    case "TeacherName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(s => s.Teacher.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(s => s.Teacher.Name).ToList();
                        }
                        break;
                    case "Subject":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(s => s.Subject.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(s => s.Subject.Name).ToList();
                        }
                        break;
                    case "Room":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(s => s.Room.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(s => s.Room.Name).ToList();
                        }
                        break;
                    case "TimeSlot":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(r => r.Slot).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(r => r.Slot).ToList();
                        }
                        break;
                    case "Class":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(s => s.Class.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(s => s.Class.Name).ToList();
                        }
                        break;
                    case "DayOfWeek":
                        if (SelectedSortOrder == "Asc")
                        {
                            Schedules = Schedules.OrderBy(s => s.DayOfWeek).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Schedules = Schedules.OrderByDescending(s => s.DayOfWeek).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }

        public async Task OnPostAddScheduleAsync(Models.Schedule schedule)
        {
            bool isConflict = CheckScheduleConflict(schedule);

            if (!isConflict)
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "A new schedule has been added successfully.");
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "There is a schedule conflict. Please change!");
            }
            LoadData();
        }

        public async Task OnPostDeleteScheduleAsync(int ScheduleID)
        {
            var selectSchedule = _context.Schedules.FirstOrDefault(s => s.Id == ScheduleID);
            if (selectSchedule != null)
            {
                _context.Schedules.Remove(selectSchedule);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "Schedule has been deleted successfully!");
            }
            LoadData();
        }

        public async Task OnPostEditScheduleAsync(Models.Schedule schedule)
        {
            var selectedSchedule = _context.Schedules.FirstOrDefault(s => s.Id == schedule.Id);

            if (selectedSchedule != null)
            {
                bool isConflict = CheckScheduleConflict(schedule);

                if (!isConflict)
                {
                    selectedSchedule.ClassId = schedule.ClassId;
                    selectedSchedule.SubjectId = schedule.SubjectId;
                    selectedSchedule.TeacherId = schedule.TeacherId;
                    selectedSchedule.RoomId = schedule.RoomId;
                    selectedSchedule.DayOfWeek = schedule.DayOfWeek;
                    selectedSchedule.Slot = schedule.Slot;

                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "Schedule has been updated successfully!");
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "There is a schedule conflict. Schedule was not updated!");
                }
            }
            
            LoadData();
        }

        private bool CheckScheduleConflict(Models.Schedule editedSchedule)
        {
            bool isConflict = false;

            if (editedSchedule == null)
            {
                isConflict = _context.Schedules.Any(s =>
                    s.TeacherId == TeacherId &&
                    s.DayOfWeek == DayOfWeek &&
                    s.Slot == Slot);

                isConflict |= _context.Schedules.Any(s =>
                    s.RoomId == RoomId &&
                    s.DayOfWeek == DayOfWeek &&
                    s.Slot == Slot);

                isConflict |= _context.Schedules.Any(s =>
                    s.ClassId == ClassId &&
                    s.DayOfWeek == DayOfWeek &&
                    s.Slot == Slot);
            }
            else
            {
                isConflict = _context.Schedules.Any(s =>
                    s.Id != editedSchedule.Id &&
                    ((s.TeacherId == editedSchedule.TeacherId && s.DayOfWeek == editedSchedule.DayOfWeek && s.Slot == editedSchedule.Slot) ||
                     (s.RoomId == editedSchedule.RoomId && s.DayOfWeek == editedSchedule.DayOfWeek && s.Slot == editedSchedule.Slot) || 
                     (s.ClassId == editedSchedule.ClassId && s.DayOfWeek == editedSchedule.DayOfWeek && s.Slot == editedSchedule.Slot)));
            }

            return isConflict;
        }

        public async Task OnPostImportFileAsync()
        {
            try
            {

                string filename = Upload.FileName;
                var file = Path.Combine(_environment.ContentRootPath, "Upload", filename);

                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    Upload.CopyTo(fileStream);
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                ValidateXML(file);

                XmlNodeList scheduleNodes = doc.SelectNodes("//ScheduleData/Schedules/Schedule");
                if (scheduleNodes != null)
                {
                    foreach (XmlNode scheduleNode in scheduleNodes)
                    {
                        int classId = int.Parse(scheduleNode.SelectSingleNode("ClassId").InnerText);
                        int subjectId = int.Parse(scheduleNode.SelectSingleNode("SubjectId").InnerText);
                        int teacherId = int.Parse(scheduleNode.SelectSingleNode("TeacherId").InnerText);
                        int roomId = int.Parse(scheduleNode.SelectSingleNode("RoomId").InnerText);
                        int dayOfWeek = int.Parse(scheduleNode.SelectSingleNode("DayOfWeek").InnerText);
                        int slot = int.Parse(scheduleNode.SelectSingleNode("Slot").InnerText);

                        if (slot < 1 || slot > 4 || dayOfWeek < 2 || dayOfWeek > 8)
                        {
                            continue;
                        }

                        var newSchedule = new Models.Schedule
                        {
                            ClassId = classId,
                            SubjectId = subjectId,
                            TeacherId = teacherId,
                            RoomId = roomId,
                            DayOfWeek = dayOfWeek,
                            Slot = slot,
                        };

                        bool isConflict = CheckScheduleConflict(newSchedule);

                        if (!isConflict)
                        {
                            _context.Schedules.Add(newSchedule);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "Schedules have been imported.");
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "No schedules found in the XML file.");
                }

                LoadData();
            }
            catch (Exception ex)
            {
                ViewData["result"] = ex.Message;
            }
        }

        private void ValidateXML(string file)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            var xsdPath = Path.Combine(_environment.ContentRootPath, "Upload", "Schedule.xsd");
            settings.Schemas.Add(null, xsdPath);

            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ProcessSchemaLocation;
            settings.ValidationFlags |= System.Xml.Schema.XmlSchemaValidationFlags.ReportValidationWarnings;

            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(this.ValidationEventHandler);
            try
            {
                XmlReader xmlReader = XmlReader.Create(file, settings);
                while (xmlReader.Read())
                {

                }
                ViewData["result"] = "Validation Passed";
            }
            catch (Exception ex)
            {
                ViewData["result"] = ex.Message;
            }
        }

        public void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            result = "Validate failed because " + args.Message;
            throw new Exception("Validation Failed because " + args.Message);
        }

        public async Task<IActionResult> OnPostExportSchedulesAsync()
        {
            var schedules = _context.Schedules
                .Include(s => s.Class)
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Include(s => s.Room)
                .ToList();

            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Class Name,Subject Name,Teacher Name,Room Name,Day of Week,Slot");

            foreach (var schedule in schedules)
            {
                string className = schedule.Class != null ? schedule.Class.Name : "N/A";
                string subjectName = schedule.Subject != null ? schedule.Subject.Name : "N/A";
                string teacherName = schedule.Teacher != null ? schedule.Teacher.Name : "N/A";
                string roomName = schedule.Room != null ? schedule.Room.Name : "N/A";

                csvContent.AppendLine($"{className},{subjectName},{teacherName},{roomName},{schedule.DayOfWeek},{schedule.Slot}");
            }

            byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString().TrimEnd());
            await _hubContext.Clients.All.SendAsync("Receive Schedule Update", "Schedules have been exported.");

            return File(csvBytes, "text/csv", "schedules.csv");
        }
    }
}
