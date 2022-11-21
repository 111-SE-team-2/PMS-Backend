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
    }
}
