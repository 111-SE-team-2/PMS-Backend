using System.Collections.Generic;

namespace project_manage_system_backend.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsVideoConferencing { get; set; }
        public List<ScheduleOption> ScheduleOptions { get; set; } = new List<ScheduleOption>();
    }
}
