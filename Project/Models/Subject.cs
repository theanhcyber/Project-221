using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Subject
    {
        public Subject()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
