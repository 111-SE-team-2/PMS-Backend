using System;
using System.Collections.Generic;

namespace project_manage_system_backend.Models
{
    public class ScheduleOption
    {
        public int Id { get; set; }
        public Schedule Schedule { get; set; }
        public string Duration { get; set; }
        public string StartTime { get; set; }
        public List<UserScheduleOption> Users { get; set; } = new List<UserScheduleOption>();
        public int NumberOfYes { set; get; }
        public int NumberOfIfNeedBe { set; get; }
        public int NumberOfCannotAttend { set; get; }
        public int NumberOfPending { set; get; }
    }
}
