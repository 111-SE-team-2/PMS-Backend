namespace project_manage_system_backend.Dtos.Schedule
{
    public class ScheduleDto
    {
        public int projectId { set; get; }
        public string title { set; get; }
        public string location { set; get; }
        public string description { set; get; }
        public bool isVideoConferencing { set; get; }
    }
}
