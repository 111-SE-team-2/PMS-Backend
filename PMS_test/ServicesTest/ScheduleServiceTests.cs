using Microsoft.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Models;
using project_manage_system_backend.Services;
using project_manage_system_backend.Shares;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Xunit;
using Project = project_manage_system_backend.Models.Project;

namespace PMS_test.ServicesTest
{
    [TestCaseOrderer("XUnit.Project.Orderers.AlphabeticalOrderer", "XUnit.Project")]
    public class ScheduleServiceTests
    {
        private readonly PMSContext _dbContext;
        private readonly ScheduleService _scheduleService;

        public ScheduleServiceTests()
        {
            _dbContext = new PMSContext(new DbContextOptionsBuilder<PMSContext>()
                                            .UseSqlite(CreateInMemoryDatabase())
                                            .Options);
            _dbContext.Database.EnsureCreated();
            _scheduleService = new ScheduleService(_dbContext);
            InitialDatabase();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();

            return connection;
        }

        private void InitialDatabase()
        {
            User testUser = new User
            {
                Account = "github_testUser",
                Name = "testUser",
                Projects = new List<UserProject>(),
            };

            Project testProject = new Project
            {
                Name = "testProject",
                Owner = testUser,
            };

            List<UserProject> testUserProject = new List<UserProject>()
            {
                new UserProject
                {
                    User = testUser,
                    Project = testProject,
                },
            };

            testUser.Projects = testUserProject;

            Schedule testSchedule = new Schedule
            {
                ProjectId = 1,
                Project = testProject,
                Title = "title",
                Location = "location",
                Description = "description",
                IsVideoConferencing = false
            };

            _dbContext.Projects.Add(testProject);
            _dbContext.Users.Add(testUser);
            _dbContext.Schedules.Add(testSchedule);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void TestGetScheduleByProjectId()
        {
            List<Schedule> scheduleList = _scheduleService.GetScheduleByProjectId(1);
            Assert.Single(scheduleList);
            Assert.Equal("title", scheduleList[0].Title);
            Assert.Equal("location", scheduleList[0].Location);
            Assert.Equal("description", scheduleList[0].Description);
            Assert.False(scheduleList[0].IsVideoConferencing);
        }

        [Fact]
        public void TestGetScheduleByScheduleId()
        {
            Schedule schedule = _scheduleService.GetScheduleByScheduleId(1);
            Assert.Equal("title", schedule.Title);
            Assert.Equal("location", schedule.Location);
            Assert.Equal("description", schedule.Description);
            Assert.False(schedule.IsVideoConferencing);
        }

        [Fact]
        public void TestCreateSchedule()
        {
            ScheduleDto scheduleDto = new ScheduleDto
            {
                projectId = 1,
                title = "test title",
                location = "test location",
                description = "test description",
                isVideoConferencing = true
            };
            _scheduleService.CreateSchedule(scheduleDto);

            Schedule schedule = _scheduleService.GetScheduleByScheduleId(2);
            Assert.Equal("test title", schedule.Title);
            Assert.Equal("test location", schedule.Location);
            Assert.Equal("test description", schedule.Description);
            Assert.True(schedule.IsVideoConferencing);

            List<Schedule> scheduleList = _scheduleService.GetScheduleByProjectId(1);
            Assert.Equal(2, scheduleList.Count);
            Assert.Equal("test title", scheduleList[1].Title);
            Assert.Equal("test location", scheduleList[1].Location);
            Assert.Equal("test description", scheduleList[1].Description);
            Assert.True(scheduleList[1].IsVideoConferencing);
        }

        [Fact]
        public void TestEditScheduleInformation()
        {
            ScheduleDto editedScheduleDto = new ScheduleDto
            {
                scheduleId = 1,
                title = "edited title",
                location = "edited location",
                description = "edited description",
                isVideoConferencing = false
            };
            _scheduleService.EditScheduleInformation(editedScheduleDto);

            Schedule schedule = _scheduleService.GetScheduleByScheduleId(1);
            Assert.Equal("edited title", schedule.Title);
            Assert.Equal("edited location", schedule.Location);
            Assert.Equal("edited description", schedule.Description);
            Assert.False(schedule.IsVideoConferencing);

            List<Schedule> scheduleList = _scheduleService.GetScheduleByProjectId(1);
            Assert.Single(scheduleList);
            Assert.Equal("edited title", scheduleList[0].Title);
            Assert.Equal("edited location", scheduleList[0].Location);
            Assert.Equal("edited description", scheduleList[0].Description);
            Assert.False(scheduleList[0].IsVideoConferencing);
        }

        [Fact]
        public void TestDeleteSchedule()
        {
            _scheduleService.DeleteSchedule(1);

            List<Schedule> scheduleList = _scheduleService.GetScheduleByProjectId(1);
            Assert.Empty(scheduleList);
        }
    }
}
