using project_manage_system_backend.Enum;

namespace project_manage_system_backend.Models
{
    public class UserScheduleOption
    {
        public string Account { get; set; }
        public User User { get; set; }
        public int ScheduleOptionId { get; set; }
        public ScheduleOption ScheduleOption { get; set; }
        public ScheduleOptionAvailability Availability { get; set; }
    }
}
