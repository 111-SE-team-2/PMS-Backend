using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Shares;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace project_manage_system_backend.Services
{
    public class ScheduleService : BaseService
    {
        public ScheduleService(PMSContext dbContext) : base(dbContext) { }

        public void CreateSchedule(ScheduleDto scheduleDto)
        {
            string regexPattern = "^[A-Za-z0-9]+";
            Regex regex = new Regex(regexPattern);
            if (scheduleDto.title == "" || !regex.IsMatch(scheduleDto.title))
            {
                throw new Exception("please enter schedule title");
            }
            if (scheduleDto.location == "" || !regex.IsMatch(scheduleDto.location))
            {
                throw new Exception("please enter schedule location");
            }

            var project = _dbContext.Projects.Include(project => project.Repositories).Where(project => project.Id == scheduleDto.projectId).First();
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
                throw new Exception("create schedule fail, DB can't save!");
        }

        public void EditScheduleInformation(ScheduleDto scheduleDto)
        {
            string regexPattern = "^[A-Za-z0-9]+";
            Regex regex = new Regex(regexPattern);
            if (scheduleDto.title == "" || !regex.IsMatch(scheduleDto.title))
            {
                throw new Exception("please enter schedule title");
            }
            if (scheduleDto.location == "" || !regex.IsMatch(scheduleDto.location))
            {
                throw new Exception("please enter schedule location");
            }

            var project = _dbContext.Projects.Include(project => project.Repositories).Where(project => project.Id == scheduleDto.projectId).First();
            var schedulesInProjectWithSameTitle = project.Schedules.Where(schedule => schedule.Title == scheduleDto.title).ToList();
            var schedule = _dbContext.Schedules.Find(scheduleDto.scheduleId);
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

        public bool DeleteSchedule(int projectId, int scheduleId)
        {
            try
            {
                var schedule = _dbContext.Schedules.Include(schedule => schedule.Project).First(schedule => schedule.Id == scheduleId && schedule.Project.Id == projectId);
                _dbContext.Schedules.Remove(schedule);
                return !(_dbContext.SaveChanges() == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
