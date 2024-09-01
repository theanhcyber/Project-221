using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using System.Xml.Schema;

namespace Project.Pages
{
    public class ImportModel : PageModel
    {
        private readonly IWebHostEnvironment environment;
        private string result;
        [BindProperty]
        public IFormFile Upload { get; set; }
        public ImportModel(IWebHostEnvironment _environment)
        {
            environment = _environment;
        }
        public void OnGet()
        {
        }

        public void OnPost()
        {
            string filename = Upload.FileName;
            var file = Path.Combine(environment.ContentRootPath, "Upload", filename);

            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                Upload.CopyTo(fileStream);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            ValidateXML(file);
        }

        private void ValidateXML(string file)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;

            var xsdPath = Path.Combine(environment.ContentRootPath, "Upload", "Schedule.xsd");
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
            }catch (Exception ex)
            {
                ViewData["result"] = ex.Message;
            }
        }

        public void ValidationEventHandler(object sender, ValidationEventArgs args)
        {
            result = "Validate failed because " + args.Message;
            throw new Exception("Validation Failed because " + args.Message);
        }
    }
}
