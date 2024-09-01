using System;
using System.Collections.Generic;

namespace Project.Models
{
    public partial class Schedule
    {
        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? RoomId { get; set; }
        public int Slot { get; set; }
        public int DayOfWeek { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}
