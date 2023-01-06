using System.Collections.Generic;

namespace project_manage_system_backend.Models
{
    public class MeetingMinute
    {
        public int Id { get; set; }
        public Project Project { get; set; }
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
