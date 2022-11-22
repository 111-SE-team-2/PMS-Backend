using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Models;
using project_manage_system_backend.Shares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_manage_system_backend.Services
{
    public class ScheduleOptionService : BaseService
    {
        public ScheduleOptionService(PMSContext dbContext) : base(dbContext) { }

        public void AddScheduleOption(ScheduleOptionDto scheduleOptionDto)
        {
            if (scheduleOptionDto.duration == "")
            {
                throw new Exception("please enter schedule option duration");
            }
            if (scheduleOptionDto.startTime == "")
            {
                throw new Exception("please enter schedule option startTime");
            }

            var schedule = _dbContext.Schedules.Where(schedule => schedule.Id == scheduleOptionDto.scheduleId).First();
            var scheduleOptionsInSchedule = schedule.ScheduleOptions.Where(scheduleOption => scheduleOption.Duration == scheduleOptionDto.duration && scheduleOption.StartTime == scheduleOptionDto.startTime).ToList();
            if (scheduleOptionsInSchedule.Count != 0)
            {
                throw new Exception("duplicate schedule option");
            }

            var scheduleOption = new Models.ScheduleOption
            {
                Schedule = schedule,
                Duration = scheduleOptionDto.duration,
                StartTime = scheduleOptionDto.startTime,
            };

            _dbContext.Add(scheduleOption);
            if (_dbContext.SaveChanges() == 0)
                throw new Exception("add schedule option fail, DB can't save!");
        }
    }
}
