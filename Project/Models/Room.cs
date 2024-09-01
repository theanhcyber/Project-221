using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Room
    {
        public Room()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
