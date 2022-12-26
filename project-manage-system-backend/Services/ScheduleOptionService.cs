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
            if (!(scheduleOptionDto.scheduleId > 0))
            {
                throw new Exception("please enter schedule Id");
            }
            if (scheduleOptionDto.duration == "")
            {
                throw new Exception("please enter schedule option duration");
            }
            if (scheduleOptionDto.startTime == "")
            {
                throw new Exception("please enter schedule option startTime");
            }

            var schedule = _dbContext.Schedules.Where(schedule => schedule.Id.Equals(scheduleOptionDto.scheduleId)).Include(schedule => schedule.ScheduleOptions).First();
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
                NumberOfYes = scheduleOptionDto.numberOfYes,
                NumberOfIfNeedBe = scheduleOptionDto.numberOfIfNeedBe,
                NumberOfCannotAttend = scheduleOptionDto.numberOfCannotAttend,
                NumberOfPending = scheduleOptionDto.numberOfPending
            };

            _dbContext.Add(scheduleOption);
            if (_dbContext.SaveChanges() == 0)
                throw new Exception("add schedule option fail, DB can't save!");
        }

        public bool DeleteScheduleOption(int scheduleOptionId)
        {
            try
            {
                var scheduleOption = _dbContext.ScheduleOptions.Where(scheduleOption => scheduleOption.Id.Equals(scheduleOptionId)).First(); ;
                _dbContext.ScheduleOptions.Remove(scheduleOption);
                return !(_dbContext.SaveChanges() == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ScheduleOption> GetScheduleOptionByScheduleId(int scheduleId)
        {
            var schedule = _dbContext.Schedules.Where(schedule => schedule.Id.Equals(scheduleId)).Include(schedule => schedule.ScheduleOptions).ThenInclude(scheduleOption => scheduleOption.Users).First();
            return schedule.ScheduleOptions;
        }
    }
}
