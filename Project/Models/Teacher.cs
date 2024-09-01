using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? DoB { get; set; }
        public int? Gender { get; set; }
        public string? Phone { get; set; }
        public int Active { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
