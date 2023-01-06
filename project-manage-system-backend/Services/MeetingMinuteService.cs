using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Models;
using project_manage_system_backend.Shares;
using System;
using System.Collections.Generic;
using System.Linq;

namespace project_manage_system_backend.Services
{
    public class MeetingMinuteService : BaseService
    {
        public MeetingMinuteService(PMSContext dbContext) : base(dbContext) { }

        public void CreateMeetingMinute(MeetingMinuteDto meetingMinuteDto)
        {
            if (!(meetingMinuteDto.projectId > 0))
            {
                throw new Exception("please enter project Id");
            }
            if (meetingMinuteDto.title == "")
            {
                throw new Exception("please enter meeting minute title");
            }

            var project = _dbContext.Projects.Where(project => project.Id.Equals(meetingMinuteDto.projectId)).Include(project => project.MeetingMinutes).First();
            if (project.MeetingMinutes.Where(meetingMinute => meetingMinute.Title == meetingMinuteDto.title).ToList().Count != 0)
            {
                throw new Exception("duplicate meeting minute title");
            }

            var meetingMinute = new Models.MeetingMinute
            {
                Project = project,
                Title = meetingMinuteDto.title,
                Content = meetingMinuteDto.content
            };

            _dbContext.Add(meetingMinute);
            if (_dbContext.SaveChanges() == 0)
                throw new Exception("create meeting minute fail");
        }

        public void EditMeetingMinuteInformation(MeetingMinuteDto meetingMinuteDto)
        {
            if (!(meetingMinuteDto.meetingMinuteId > 0))
            {
                throw new Exception("please enter meeting minutes Id");
            }
            if (meetingMinuteDto.title == "")
            {
                throw new Exception("please enter meeting minute title");
            }

            var meetingMinute = _dbContext.MeetingMinutes.Where(meetingMinute => meetingMinute.Id.Equals(meetingMinuteDto.meetingMinuteId)).First();
            var project = _dbContext.Projects.Where(project => project.Id.Equals(meetingMinute.ProjectId)).Include(project => project.MeetingMinutes).First();
            var meetingMinutesInProjectWithSameTitle = project.MeetingMinutes.Where(meetingMinute => meetingMinute.Title == meetingMinuteDto.title).ToList();
            if (meetingMinutesInProjectWithSameTitle.Count != 0 && meetingMinute.Title != meetingMinuteDto.title)
            {
                throw new Exception("duplicate meeting minute title");
            }

            meetingMinute.Title = meetingMinuteDto.title;
            meetingMinute.Content = meetingMinuteDto.content;

            _dbContext.Update(meetingMinute);
            if (_dbContext.SaveChanges() == 0)
            {
                throw new Exception("edit meeting minute information fail");
            }
        }

        public bool DeleteMeetingMinute(int meetingMinuteId)
        {
            try
            {
                var meetingMinute = _dbContext.MeetingMinutes.Where(meetingMinute => meetingMinute.Id.Equals(meetingMinuteId)).First();
                _dbContext.MeetingMinutes.Remove(meetingMinute);
                return !(_dbContext.SaveChanges() == 0);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<MeetingMinute> GetMeetingMinuteByProjectId(int projectId)
        {
            var project = _dbContext.Projects.Where(project => project.Id.Equals(projectId)).Include(project => project.MeetingMinutes).First();
            return project.MeetingMinutes;
        }

        public MeetingMinute GetMeetinhMinuteByMeetingMinuteId(int meetingMinuteId)
        {
            var meetingMinute = _dbContext.MeetingMinutes.Where(meetingMinute => meetingMinute.Id.Equals(meetingMinuteId)).First();
            return meetingMinute;
        }
    }
}
