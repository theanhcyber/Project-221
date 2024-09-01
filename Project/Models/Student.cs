using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? DoB { get; set; }
        public int? Gender { get; set; }
        public string? Phone { get; set; }
        public int Active { get; set; }
        public int ClassId { get; set; }

        public virtual Class Class { get; set; } = null!;
    }
}
