using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Models;
using project_manage_system_backend.Shares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_manage_system_backend.Services
{
    public class ScheduleService : BaseService
    {
        public ScheduleService(PMSContext dbContext) : base(dbContext) { }

        public void CreateSchedule(ScheduleDto scheduleDto)
        {
            if (scheduleDto.title == "")
            {
                throw new Exception("please enter schedule title");
            }
            if (scheduleDto.location == "")
            {
                throw new Exception("please enter schedule location");
            }

            var project = _dbContext.Projects.Where(project => project.Id.Equals(scheduleDto.projectId)).Include(project => project.Schedules).First();
            if (project.Schedules.Where(schedule => schedule.Title == scheduleDto.title).ToList().Count != 0)
            {
                throw new Exception("duplicate schedule title");
            }

            var schedule = new Models.Schedule
            {
                Project = project,
                Title = scheduleDto.title,
                Location = scheduleDto.location,
                Description = scheduleDto.description,
                IsVideoConferencing = scheduleDto.isVideoConferencing,
            };

            _dbContext.Add(schedule);
            if (_dbContext.SaveChanges() == 0)
                throw new Exception("create schedule fail");
        }

        public void EditScheduleInformation(ScheduleDto scheduleDto)
        {
            if (scheduleDto.title == "")
            {
                throw new Exception("please enter schedule title");
            }
            if (scheduleDto.location == "")
            {
                throw new Exception("please enter schedule location");
            }

            var schedule = _dbContext.Schedules.Find(scheduleDto.scheduleId);
            var project = _dbContext.Projects.Where(project => project.Id.Equals(schedule.ProjectId)).Include(project => project.Schedules).First();
            var schedulesInProjectWithSameTitle = project.Schedules.Where(schedule => schedule.Title == scheduleDto.title).ToList();
            if (schedulesInProjectWithSameTitle.Count != 0 && schedule.Title != scheduleDto.title)
            {
                throw new Exception("duplicate schedule title");
            }

            schedule.Title = scheduleDto.title;
            schedule.Location = scheduleDto.location;
            schedule.Description = scheduleDto.description;
            schedule.IsVideoConferencing = scheduleDto.isVideoConferencing;

            _dbContext.Update(schedule);
            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("edit schedule information fail");
            }
        }

        public bool DeleteSchedule(int scheduleId)
        {
            try
            {
                var scheduleOptions = _dbContext.ScheduleOptions.Where(scheduleOption => scheduleOption.Schedule.Id.Equals(scheduleId));
                _dbContext.ScheduleOptions.RemoveRange(scheduleOptions);
                var schedule = _dbContext.Schedules.Find(scheduleId);
                _dbContext.Schedules.Remove(schedule);
                return !(_dbContext.SaveChanges() == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Schedule> GetScheduleByProjectId(int projectId)
        {
            var project = _dbContext.Projects.Where(project => project.Id.Equals(projectId)).Include(project => project.Schedules).First();
            return project.Schedules;
        }
    }
}
