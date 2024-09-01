using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Project.Hubs;
using Project.Models;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.Xml.Linq;
using System;

namespace Project.Pages.Teacher
{
    public class ListModel : PageModel
    {
        private ProjectPRN221Context _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IHubContext<TeacherHub> _hubContext;

        private string result;

        [BindProperty]
        public int TeacherId { get; set; }
        [BindProperty]
        public string TeacherName { get; set; }
        [BindProperty]
        public int Gender { get; set; }
        [BindProperty]
        public DateTime DoB { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public int IsActive { get; set; }
        public List<Models.Teacher> Teachers { get; set; }

        [BindProperty]
        public string SearchName { get; set; }
        [BindProperty]
        public string SearchPhone { get; set; }
        [BindProperty]
        public string SearchGender { get; set; }
        [BindProperty]
        public string SearchIsActive { get; set; }
        [BindProperty]
        public DateTime? SearchDoB { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public string SortOrder { get; set; }
        [BindProperty]
        public string SortType { get; set; }
        public List<String> SortOrders { get; set; }
        public List<String> SortTypes { get; set; }
        public string SelectedSortType { get; set; }
        public string SelectedSortOrder { get; set; }
        public List<String> SearchGenders { get; set; }
        public List<String> SearchStatus { get; set; }

        public ListModel(ProjectPRN221Context context, IHubContext<TeacherHub> hubContext, IWebHostEnvironment environment)
        {
            _context = context;
            _hubContext = hubContext;
            _environment = environment;
            LoadData();
        }

        private void LoadData()
        {
            Teachers = _context.Teachers.ToList();
            SearchGenders = new List<String> {"All", "Male", "Female"};
            SearchStatus = new List<String> { "All", "Active", "Deactive" };
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "ID", "TeacherName", "DoB", "Phone", "Gender", "Status" };
        }

        public void OnGet()
        {
            LoadData();
        }

        private int ConvertGender(string sgender)
        {
            switch (sgender)
            {
                case "Male":
                    return 1;
                case "Female":
                    return 0;
                default:
                    return 2;
            }
        }

        private int ConvertStatus(String sstatus)
        {
            switch (sstatus)
            {
                case "Active":
                    return 1;
                case "Deactive":
                    return 0;
                default:
                    return 2;
            }
        }

        public void OnPostSearchTeacher()
        {
            IQueryable<Models.Teacher> query = _context.Teachers;

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(s => s.Name.Contains(SearchName));
            }
            if (!string.IsNullOrEmpty(SearchPhone))
            {
                query = query.Where(s => s.Phone == SearchPhone);
            }
            if (!string.IsNullOrEmpty(SearchGender) && SearchGender != "All")
            {
                query = query.Where(s => s.Gender == ConvertGender(SearchGender));
            }
            if (!string.IsNullOrEmpty(SearchIsActive) && SearchIsActive != "All")
            {
                query = query.Where(s => s.Active == ConvertStatus(SearchIsActive));
            }
            if (SearchDoB.HasValue)
            {
                query = query.Where(s => s.DoB == SearchDoB.Value);
            }

            Teachers = query.ToList();


            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SortType)
                {
                    case "TeacherName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.Name).ToList();
                        }
                        break;
                    case "ID":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.Id).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.Id).ToList();
                        }
                        break;
                    case "DoB":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.DoB).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.DoB).ToList();
                        }
                        break;
                    case "Phone":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.Phone).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.Phone).ToList();
                        }
                        break;
                    case "Gender":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.Gender).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.Gender).ToList();
                        }
                        break;
                    case "Status":
                        if (SelectedSortOrder == "Asc")
                        {
                            Teachers = Teachers.OrderBy(t => t.Active).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Teachers = Teachers.OrderByDescending(t => t.Active).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }

        public async Task OnPostAddTeacherAsync()
        {
            if (!string.IsNullOrEmpty(TeacherName))
            {
                var newTeacher = new Models.Teacher
                {
                    Name = TeacherName,
                    DoB = DoB,
                    Phone = Phone,
                    Gender = Gender,
                    Active = IsActive,
                };
                _context.Teachers.Add(newTeacher);
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "A new teacher has been added successfully.");
            }

            LoadData();
        }

        public async Task OnPostEditTeacherAsync(int TeacherID, string Name, DateTime DoB, string Phone, int gender, int Active)
        {
            var selectTeacher = _context.Teachers.FirstOrDefault(t => t.Id == TeacherID);

            if (selectTeacher != null)
            {
                selectTeacher.Name = Name;
                selectTeacher.DoB = DoB;
                selectTeacher.Phone = Phone;
                selectTeacher.Gender = gender;
                selectTeacher.Active = Active;
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "Teacher updated successfully!");
            }
            LoadData();
        }

        public async Task OnPostDeactiveTeacherAsync(int TeacherID)
        {
            var selectTeacher = _context.Teachers.FirstOrDefault(t => t.Id == TeacherID);
            if (selectTeacher != null)
            {
                selectTeacher.Active = 0;
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "Teacher deactived successfully!");
            }
            LoadData();
        }

        public async Task OnPostReactiveTeacherAsync(int TeacherID)
        {
            var selectTeacher = _context.Teachers.FirstOrDefault(t => t.Id == TeacherID);
            if (selectTeacher != null)
            {
                selectTeacher.Active = 1;
                _context.SaveChanges();
                await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "Teacher actived successfully!");
            }
            LoadData();
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

                XmlNodeList teacherNodes = doc.SelectNodes("//TeacherData/Teachers/Teacher");
                if (teacherNodes != null)
                {
                    foreach (XmlNode teacherNode in teacherNodes)
                    {
                        int Id = int.Parse(teacherNode.SelectSingleNode("Id").InnerText);
                        string Name = teacherNode.SelectSingleNode("Name").InnerText;
                        DateTime DoB = DateTime.Parse(teacherNode.SelectSingleNode("DoB").InnerText);
                        int Gender = int.Parse(teacherNode.SelectSingleNode("Gender").InnerText);
                        string Phone = teacherNode.SelectSingleNode("Phone").InnerText;
                        int Active = int.Parse(teacherNode.SelectSingleNode("Active").InnerText);

                        var existingTeacher = _context.Teachers.FirstOrDefault(t => t.Id == Id);

                        if (existingTeacher != null)
                        {
                            if (existingTeacher.Name != Name || existingTeacher.DoB != DoB ||
                                existingTeacher.Gender != Gender || existingTeacher.Phone != Phone ||
                                existingTeacher.Active != Active)
                            {
                                existingTeacher.Name = Name;
                                existingTeacher.DoB = DoB;
                                existingTeacher.Gender = Gender;
                                existingTeacher.Phone = Phone;
                                existingTeacher.Active = Active;
                            }
                        }
                        else
                        {
                            var newTeacher = new Models.Teacher
                            {
                                Id = Id,
                                Name = Name,
                                DoB = DoB,
                                Gender = Gender,
                                Phone = Phone,
                                Active = Active,
                            };
                            _context.Teachers.Add(newTeacher);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "Teachers have been imported.");
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "No teachers found in the XML file.");
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

            var xsdPath = Path.Combine(_environment.ContentRootPath, "Upload", "Teacher.xsd");
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

        public async Task<IActionResult> OnPostExportTeachersAsync()
        {
            try
            {
                var teachers = _context.Teachers.ToList();

                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Id,Name,DoB,Gender,Phone,Active");

                foreach (var teacher in teachers)
                {
                    string teacherName = teacher.Name ?? "N/A";
                    string doB = teacher.DoB.HasValue ? teacher.DoB.Value.ToString("dd/MM/yyyy") : "N/A";
                    string gender = teacher.Gender == 1 ? "Male" : "Female";
                    string phone = teacher.Phone ?? "N/A";
                    string active = teacher.Active == 1 ? "active" : "deactive";

                    csvContent.AppendLine($"{teacher.Id},{teacherName},{doB},{gender},{phone},{active}");
                }

                byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString().TrimEnd());

                await _hubContext.Clients.All.SendAsync("Receive Teacher Update", "Teachers have been exported.");

                return File(csvBytes, "text/csv", "teachers.csv");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
