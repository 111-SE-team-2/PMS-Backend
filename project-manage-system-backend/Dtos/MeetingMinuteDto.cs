namespace project_manage_system_backend.Dtos
{
    public class MeetingMinuteDto
    {
        public int meetingMinuteId { set; get; }
        public int projectId { set; get; }
        public string title { set; get; }
        public string content { set; get; }
    }
}
