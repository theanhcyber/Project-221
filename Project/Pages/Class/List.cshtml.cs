using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;
using Project.Models;
using System.Security.Claims;

namespace Project.Pages.Class
{
    public class ListModel : PageModel
    {
        private ProjectPRN221Context _context;
        private IHubContext<ClassHub> _hubContext;
        [BindProperty]
        public string ClassName { get; set; }
        [BindProperty]
        public int TotalStudent { get; set; }
        public List<Models.Class> Classes { get; set; }
        [BindProperty]
        public string SearchName { get; set; }
        [BindProperty]
        public int? SearchTotal { get; set; }
        [BindProperty]
        public string SortOrder { get; set; }
        [BindProperty]
        public string SortType { get; set; }
        public List<String> SortOrders { get; set; }
        public List<String> SortTypes { get; set; }
        public string SelectedSortType { get; set; }
        public string SelectedSortOrder { get; set; }

        public ListModel (ProjectPRN221Context context, IHubContext<ClassHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
            LoadData();
        }

        private void LoadData()
        {
            Classes = _context.Classes.ToList();
            foreach (var @class in Classes)
            {
                @class.TotalStudent = _context.Students.Count(s => s.ClassId == @class.Id && s.Active == 1);
            }
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "ClassId", "ClassName", "Total Student in class" };
        }

        public void OnGet()
        {
            LoadData();
        }

        public void OnPostSearchClass()
        {
            IQueryable<Models.Class> query = _context.Classes;

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(c => c.Name.Contains(SearchName));
            }
            
            if (SearchTotal.HasValue)
            {
                query = query.Where(c => c.TotalStudent == SearchTotal.Value);
            }

            Classes = query.ToList();

            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SelectedSortType)
                {
                    case "ClassId":
                        if (SelectedSortOrder == "Asc")
                        {
                            Classes = Classes.OrderBy(c => c.Id).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Classes = Classes.OrderByDescending(c => c.Id).ToList();
                        }
                        break;
                    case "ClassName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Classes = Classes.OrderBy(c => c.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Classes = Classes.OrderByDescending(c => c.Name).ToList();
                        }
                        break;
                    case "Total Student in class":
                        if (SelectedSortOrder == "Asc")
                        {
                            Classes = Classes.OrderBy(c => c.TotalStudent).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Classes = Classes.OrderByDescending(c => c.TotalStudent).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }

        public async Task OnPostAddClassAsync()
        {
            if (!string.IsNullOrEmpty(ClassName))
            {
                var existClass = _context.Classes.FirstOrDefault(c => c.Name == ClassName);
                if (existClass != null)
                {
                    await _hubContext.Clients.All.SendAsync("Receive Class Update", "Class existed.");
                }
                else
                {
                    var newClass = new Models.Class { Name = ClassName };
                    _context.Classes.Add(newClass);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Class Update", "A new class has been added.");
                }
            }

            LoadData();
        }

        public async Task OnPostEditClassAsync(int ClassID, string Name)
        {
            var existingClass = _context.Classes.FirstOrDefault(r => r.Name == Name && r.Id != ClassID);
            if (existingClass != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Class Update", "Class existed.");
            }
            else
            {
                var selectClass = _context.Classes.FirstOrDefault(c => c.Id == ClassID);

                if (selectClass != null)
                {
                    selectClass.Name = Name;
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Class Update", "A class has been updated successfully.");
                }
            }
            LoadData();
        }

        public async Task OnPostDeleteClassAsync(int ClassID)
        {
            var selectClass = _context.Classes.FirstOrDefault(c => c.Id == ClassID);
            var existSchedule = _context.Schedules.FirstOrDefault(r => r.ClassId == ClassID);
            if (existSchedule != null)
            {
                await _hubContext.Clients.All.SendAsync("Receive Class Update", "Class is currently having a schedule!");
            }
            else
            {

                if (selectClass != null)
                {
                    _context.Classes.Remove(selectClass);
                    _context.SaveChanges();
                    await _hubContext.Clients.All.SendAsync("Receive Class Update", "A class has been deleted successfully.");
                }
            }
            LoadData();
        }
    }
}
