using System.Collections.Generic;

namespace project_manage_system_backend.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User Owner { get; set; }
        public List<Repo> Repositories { get; set; } = new List<Repo>();
        public List<Jira> Jiras { get; set; } = new List<Jira>();
        public List<UserProject> Users { get; set; } = new List<UserProject>();
        public List<Schedule> Schedules { get; set; } = new List<Schedule>();
        public List<MeetingMinute> MeetingMinutes { get; set; } = new List<MeetingMinute>();
    }
}
