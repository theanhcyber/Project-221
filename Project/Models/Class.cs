using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Class
    {
        public Class()
        {
            Schedules = new HashSet<Schedule>();
            Students = new HashSet<Student>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TotalStudent { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
