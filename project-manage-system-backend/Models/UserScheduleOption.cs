namespace project_manage_system_backend.Models
{
    public class UserScheduleOption
    {
        public string Account { get; set; }
        public User User { get; set; }
        public int ScheduleOptionId { get; set; }
        public ScheduleOption ScheduleOption { get; set; }
        /// value：
        ///     Yes
        ///     If Need Be
        ///     Cannot Attend
        ///     Pending
        public string Availability { get; set; }
    }
}
