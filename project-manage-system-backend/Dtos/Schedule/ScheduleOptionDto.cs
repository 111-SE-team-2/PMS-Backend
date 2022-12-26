namespace project_manage_system_backend.Dtos.Schedule
{
    public class ScheduleOptionDto
    {
        public int scheduleOptionId { set; get; }
        public int scheduleId { set; get; }
        public string duration { set; get; }
        public string startTime { set; get; }
        public int numberOfYes { set; get; }
        public int numberOfIfNeedBe { set; get; }
        public int numberOfCannotAttend { set; get; }
        public int numberOfPending { set; get; }
    }
}
