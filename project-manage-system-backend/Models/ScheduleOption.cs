using System;
using System.Collections.Generic;

namespace project_manage_system_backend.Models
{
    public class ScheduleOption
    {
        public int Id { get; set; }
        public Schedule Schedule { get; set; }
        public string Duration { get; set; }
        public DateTime StartTime { get; set; }
        public List<UserScheduleOption> Users { get; set; } = new List<UserScheduleOption>();
    }
}
