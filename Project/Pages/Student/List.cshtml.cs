using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Project.Hubs;
using Project.Models;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Project.Pages.Student
{
    public class ListModel : PageModel
    {
        private ProjectPRN221Context _context;
        private readonly IWebHostEnvironment _environment;
        private IHubContext<StudentHub> _hubContext;

        private string result;
        [BindProperty]
        public int StudentId { get; set; }
        [BindProperty]
        public string StudentName { get; set; }
        [BindProperty]
        public int Gender { get; set; }
        [BindProperty]
        public DateTime DoB { get; set; }
        [BindProperty]
        public string Phone { get; set; }
        [BindProperty]
        public int ClassId { get; set; }
        [BindProperty]
        public int IsActive { get; set; }
        public List<Models.Student> Students { get; set; }
        public List<Models.Class> Classes { get; set; }

        [BindProperty]
        public string SearchName { get; set; }
        [BindProperty]
        public string SearchPhone { get; set; }
        [BindProperty]
        public int? SearchClassId { get; set; }
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

        public ListModel(ProjectPRN221Context context, IHubContext<StudentHub> hubContext, IWebHostEnvironment environment)
        {
            _context = context;
            _hubContext = hubContext;
            _environment = environment;
            LoadData();
        }

        private void LoadData()
        {
            Students = _context.Students.ToList();
            Classes = _context.Classes.ToList();
            SearchGenders = new List<String> { "All", "Male", "Female" };
            SearchStatus = new List<String> { "All", "Active", "Deactive" };
            SortOrders = new List<String> { "All", "Asc", "Desc" };
            SortTypes = new List<String> { "All", "ID", "StudentName", "DoB", "Phone", "Gender", "Status", "Class" };
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

        public void OnPostSearchStudent()
        {
            IQueryable<Models.Student> query = _context.Students;

            if (!string.IsNullOrEmpty(SearchName))
            {
                query = query.Where(s => s.Name.Contains(SearchName));
            }
            if (!string.IsNullOrEmpty(SearchPhone))
            {
                query = query.Where(s => s.Phone == SearchPhone);
            }
            if (SearchClassId.HasValue)
            {
                query = query.Where(s => s.ClassId == SearchClassId);
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

            Students = query.ToList();


            if (SortType != "All" && SortOrder != "All")
            {
                SelectedSortOrder = SortOrder;
                SelectedSortType = SortType;
                switch (SortType)
                {
                    case "ID":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Id).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Id).ToList();
                        }
                        break;
                    case "StudentName":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Name).ToList();
                        }
                        break;
                    case "DoB":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.DoB).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.DoB).ToList();
                        }
                        break;
                    case "Class":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Class.Name).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Class.Name).ToList();
                        }
                        break;
                    case "Phone":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Phone).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Phone).ToList();
                        }
                        break;
                    case "Gender":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Gender).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Gender).ToList();
                        }
                        break;
                    case "Status":
                        if (SelectedSortOrder == "Asc")
                        {
                            Students = Students.OrderBy(t => t.Active).ToList();
                        }
                        else if (SelectedSortOrder == "Desc")
                        {
                            Students = Students.OrderByDescending(t => t.Active).ToList();
                        }
                        break;
                    case "All":
                        LoadData();
                        break;
                }
            }
        }

        public async Task OnPostAddStudentAsync()
        {
            if (!string.IsNullOrEmpty(StudentName))
            {
                var newStudent = new Models.Student
                {
                    Name = StudentName,
                    DoB = DoB,
                    Gender = Gender,
                    Phone = Phone,
                    ClassId = ClassId,
                    Active = IsActive,
                };
                _context.Students.Add(newStudent);
                _context.SaveChanges();

                var affectedClass = _context.Classes
            .Include(c => c.Students)
            .FirstOrDefault(c => c.Id == ClassId);

                if (affectedClass != null)
                {
                    affectedClass.TotalStudent += 1;
                    _context.SaveChanges();
                }
                await _hubContext.Clients.All.SendAsync("Receive Student Update", "A new student has been added successfully.");
            }
            LoadData();
        }

        public async Task OnPostEditStudentAsync(int StudentID, string Name, DateTime DoB, string Phone, int ClassID, int Gender, int Active)
        {
            var selectStudent = _context.Students.FirstOrDefault(t => t.Id == StudentID);

            if (selectStudent != null)
            {
                int oldClassId = selectStudent.ClassId;

                selectStudent.Name = Name;
                selectStudent.DoB = DoB;
                selectStudent.ClassId = ClassID;
                selectStudent.Phone = Phone;
                selectStudent.Gender = Gender;
                selectStudent.Active = Active;
                _context.SaveChanges();

                var oldClass = _context.Classes.FirstOrDefault(c => c.Id == oldClassId);
                if (oldClass != null)
                {
                    oldClass.TotalStudent = _context.Students.Count(s => s.ClassId == oldClass.Id && s.Active == 1);
                }

                var newClass = _context.Classes.FirstOrDefault(c => c.Id == ClassID);
                if (newClass != null)
                {
                    newClass.TotalStudent = _context.Students.Count(s => s.ClassId == newClass.Id && s.Active == 1);
                }

                _context.SaveChanges();

                await _hubContext.Clients.All.SendAsync("Receive Student Update", "A student has been updated successfully.");
            }
            LoadData();
        }

        public async Task OnPostDeactiveStudentAsync(int StudentID)
        {
            var selectStudent = _context.Students.FirstOrDefault(t => t.Id == StudentID);
            if (selectStudent != null)
            {
                selectStudent.Active = 0;
                int classId = selectStudent.ClassId;

                _context.SaveChanges();

                var affectedClass = _context.Classes
                    .Include(c => c.Students)
                    .FirstOrDefault(c => c.Id == classId);

                if (affectedClass != null)
                {
                    affectedClass.TotalStudent = _context.Students.Count(s => s.ClassId == classId && s.Active == 1);
                    _context.SaveChanges();
                }

                await _hubContext.Clients.All.SendAsync("Receive Student Update", "A student has been deactived successfully.");
            }
            LoadData();
        }

        public async Task OnPostReactiveStudentAsync(int StudentID)
        {
            var selectStudent = _context.Students.FirstOrDefault(t => t.Id == StudentID);
            if (selectStudent != null)
            {
                selectStudent.Active = 1;
                int classId = selectStudent.ClassId;

                _context.SaveChanges();

                var affectedClass = _context.Classes
                    .Include(c => c.Students)
                    .FirstOrDefault(c => c.Id == classId);

                if (affectedClass != null)
                {
                    affectedClass.TotalStudent = _context.Students.Count(s => s.ClassId == classId && s.Active == 1);
                    _context.SaveChanges();
                }
                await _hubContext.Clients.All.SendAsync("Receive Student Update", "A student has been reactived successfully.");
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

                XmlNodeList studentNodes = doc.SelectNodes("//StudentData/Students/Student");
                if (studentNodes != null)
                {
                    foreach (XmlNode studentNode in studentNodes)
                    {
                        int Id = int.Parse(studentNode.SelectSingleNode("Id").InnerText);
                        string Name = studentNode.SelectSingleNode("Name").InnerText;
                        DateTime DoB = DateTime.Parse(studentNode.SelectSingleNode("DoB").InnerText);
                        int Gender = int.Parse(studentNode.SelectSingleNode("Gender").InnerText);
                        string Phone = studentNode.SelectSingleNode("Phone").InnerText;
                        int Active = int.Parse(studentNode.SelectSingleNode("Active").InnerText);
                        int ClassID = int.Parse(studentNode.SelectSingleNode("ClassId").InnerText);

                        var existingStudent = _context.Students.FirstOrDefault(t => t.Id == Id);

                        if (existingStudent != null)
                        {
                            if (existingStudent.Name != Name || existingStudent.DoB != DoB ||
                                existingStudent.Gender != Gender || existingStudent.Phone != Phone ||
                                existingStudent.Active != Active || existingStudent.ClassId != ClassID)
                            {
                                existingStudent.Name = Name;
                                existingStudent.DoB = DoB;
                                existingStudent.Gender = Gender;
                                existingStudent.Phone = Phone;
                                existingStudent.Active = Active;
                                existingStudent.ClassId = ClassID;
                            }
                        }
                        else
                        {
                            var newStudent = new Models.Student
                            {
                                Id = Id,
                                Name = Name,
                                DoB = DoB,
                                Gender = Gender,
                                Phone = Phone,
                                ClassId = ClassID,
                                Active = Active,
                            };
                            _context.Students.Add(newStudent);
                        }
                    }

                    await _context.SaveChangesAsync();
                    await _hubContext.Clients.All.SendAsync("Receive Student Update", "Students have been imported successfully.");
                }
                else
                {
                    await _hubContext.Clients.All.SendAsync("Receive Student Update", "No students found in the XML file.");
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

            var xsdPath = Path.Combine(_environment.ContentRootPath, "Upload", "Students.xsd");
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

        public async Task<IActionResult> OnPostExportStudentsAsync()
        {
            try
            {
                var students = _context.Students.Include(s => s.Class).ToList();

                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Id,Name,DoB,Gender,Phone,Class,Active");

                foreach (var student in students)
                {
                    string studentName = student.Name ?? "N/A";
                    string doB = student.DoB.HasValue ? student.DoB.Value.ToString("dd/MM/yyyy") : "N/A";
                    string gender = student.Gender == 1 ? "Male" : "Female";
                    string phone = student.Phone ?? "N/A";
                    string active = student.Active == 1 ? "active" : "deactive";

                    csvContent.AppendLine($"{student.Id},{studentName},{doB},{gender},{phone},{student.Class.Name},{active}");
                }

                byte[] csvBytes = Encoding.UTF8.GetBytes(csvContent.ToString().TrimEnd());

                await _hubContext.Clients.All.SendAsync("Receive Student Update", "Students have been exported successfully.");

                return File(csvBytes, "text/csv", "students.csv");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
