﻿namespace project_manage_system_backend.Dtos.Schedule
{
    public class ScheduleOptionDto
    {
        public int scheduleOptionId { set; get; }
        public int scheduleId { set; get; }
        public string duration { set; get; }
        public string startTime { set; get; }
    }
}