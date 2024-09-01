using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;
using Project.Models;

namespace Project.Pages.Subject
{
    public class ListModel : PageModel
    {
        private ProjectPRN221Context _context;
        private IHubContext<SubjectHub> _hubContext;
        [BindProperty]
        public string SubjectName { get; set; }
        public List<Models.Subject> Subjects { get; set; }
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

        public ListModel(ProjectPRN221Context context, IHubContext<SubjectHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            LoadData();
        }

        private void LoadData()
        {
            Subjects = _context.Subjects.ToList();
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "SubjectId", "SubjectName" };
        }

        public void OnGet()
        {
            LoadData();
        }

        public void OnPostSearchSubject()
        {
            if (!string.IsNullOrEmpty(SearchName))
            {
                Subjects = Subjects.Where(r => r.Name.Contains(SearchName)).ToList();
            }
            else
            {
                Subjects = _context.Subjects.ToList();
            }

            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SelectedSortType)
                {
                    case "SubjectId":
                        if (SelectedSortOrder == "Asc")
                        {
                            Subjects = Subjects.OrderBy(r => r.Id).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Subjects = Subjects.OrderByDescending(r => r.Id).ToList();
                        }
                        break;
                    case "SubjectName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Subjects = Subjects.OrderBy(r => r.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Subjects = Subjects.OrderByDescending(r => r.Name).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }
        public async Task OnPostAddSubjectAsync()
        {
            if (!string.IsNullOrEmpty(SubjectName))
            {
                var existSubject = _context.Subjects.FirstOrDefault(r => r.Name == SubjectName);
                if (existSubject != null)
                {
                    await _hubContext.Clients.All.SendAsync("Receive Subject Update", "Subject existed!");
                }
                else
                {
                    var newSubject = new Models.Subject { Name = SubjectName };
                    _context.Subjects.Add(newSubject);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Subject Update", "A new subject has been added successfully!");
                }
            }

            OnGet();
        }

        public async Task OnPostEditSubjectAsync(int SubjectID, string Name)
        {
            var existingSubject = _context.Subjects.FirstOrDefault(r => r.Name == Name && r.Id != SubjectID);
            if (existingSubject != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Subject Update", "Subject name existed!");
            }
            else
            {
                var selectSubject = _context.Subjects.FirstOrDefault(r => r.Id == SubjectID);

                if (selectSubject != null)
                {
                    selectSubject.Name = Name;
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Subject Update", "A subject has been updated successfully!");
                }
            }
            OnGet();
        }

        public async Task OnPostDeleteSubjectAsync(int SubjectID)
        {
            var selectSubject = _context.Subjects.FirstOrDefault(r => r.Id == SubjectID);
            var existSchedule = _context.Schedules.FirstOrDefault(r => r.Subject.Id == SubjectID);
            if (existSchedule != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Subject Update", "Subject is currently on teaching, can't delete!");
            }
            else
            {
                if (selectSubject != null)
                {
                    _context.Subjects.Remove(selectSubject);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Subject Update", "Subject has been deleted successfully!");
                }
            }
            OnGet();
        }
    }
}
