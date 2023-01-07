using Microsoft.CodeAnalysis;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using project_manage_system_backend.Dtos;
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
    public class ScheduleOptionServiceTests
    {
        private readonly PMSContext _dbContext;
        private readonly ScheduleOptionService _scheduleOptionService;

        public ScheduleOptionServiceTests()
        {
            _dbContext = new PMSContext(new DbContextOptionsBuilder<PMSContext>()
                                            .UseSqlite(CreateInMemoryDatabase())
                                            .Options);
            _dbContext.Database.EnsureCreated();
            _scheduleOptionService = new ScheduleOptionService(_dbContext);
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
                IsVideoConferencing = false,
                ScheduleOptions = new List<ScheduleOption>(),
            };

            ScheduleOption testScheduleOption = new ScheduleOption
            {
                Schedule = testSchedule,
                Duration = "1 hour",
                StartTime = "2023/01/01 15:00",
                NumberOfYes = 0,
                NumberOfIfNeedBe = 0,
                NumberOfCannotAttend = 0,
                NumberOfPending = 0,
            };

            testSchedule.ScheduleOptions.Add(testScheduleOption);

            _dbContext.Projects.Add(testProject);
            _dbContext.Users.Add(testUser);
            _dbContext.Schedules.Add(testSchedule);
            _dbContext.ScheduleOptions.Add(testScheduleOption);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void TestGetScheduleOptionByScheduleId()
        {
            List<ScheduleOption> scheduleOptionList = _scheduleOptionService.GetScheduleOptionByScheduleId(1);
            Assert.Single(scheduleOptionList);
            Assert.Equal("1 hour", scheduleOptionList[0].Duration);
            Assert.Equal("2023/01/01 15:00", scheduleOptionList[0].StartTime);
            Assert.Equal(0, scheduleOptionList[0].NumberOfYes);
            Assert.Equal(0, scheduleOptionList[0].NumberOfIfNeedBe);
            Assert.Equal(0, scheduleOptionList[0].NumberOfCannotAttend);
            Assert.Equal(0, scheduleOptionList[0].NumberOfPending);
        }

        [Fact]
        public void TestAddScheduleOption()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };
            _scheduleOptionService.AddScheduleOption(scheduleOptionDto);

            List<ScheduleOption> scheduleOptionList = _scheduleOptionService.GetScheduleOptionByScheduleId(1);
            Assert.Equal(2, scheduleOptionList.Count);
            Assert.Equal("2 hour", scheduleOptionList[1].Duration);
            Assert.Equal("2022/12/31 19:00", scheduleOptionList[1].StartTime);
            Assert.Equal(0, scheduleOptionList[1].NumberOfYes);
            Assert.Equal(0, scheduleOptionList[1].NumberOfIfNeedBe);
            Assert.Equal(0, scheduleOptionList[1].NumberOfCannotAttend);
            Assert.Equal(0, scheduleOptionList[1].NumberOfPending);
        }

        [Fact]
        public void TestAddScheduleOptionFailWithUnvalidScheduleId()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = -1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };
            
            Assert.Throws<Exception>(() => _scheduleOptionService.AddScheduleOption(scheduleOptionDto));
        }

        [Fact]
        public void TestAddScheduleOptionFailWithUnvalidDuration()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "",
                startTime = "2022/12/31 19:00"
            };

            Assert.Throws<Exception>(() => _scheduleOptionService.AddScheduleOption(scheduleOptionDto));
        }

        [Fact]
        public void TestAddScheduleOptionFailWithUnvalidStartTime()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "2 hour",
                startTime = ""
            };

            Assert.Throws<Exception>(() => _scheduleOptionService.AddScheduleOption(scheduleOptionDto));
        }

        [Fact]
        public void TestAddScheduleOptionFailDuplicate()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };
            _scheduleOptionService.AddScheduleOption(scheduleOptionDto);

            ScheduleOptionDto scheduleOptionDto2 = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };

            Assert.Throws<Exception>(() => _scheduleOptionService.AddScheduleOption(scheduleOptionDto2));
        }

        [Fact]
        public void TestDeleteScheduleOption()
        {
            _scheduleOptionService.DeleteScheduleOption(1);

            List<ScheduleOption> scheduleOptionList = _scheduleOptionService.GetScheduleOptionByScheduleId(1);
            Assert.Empty(scheduleOptionList);
        }

        [Fact]
        public void TestDeleteScheduleOptionFail()
        {
            Assert.False(_scheduleOptionService.DeleteScheduleOption(-1));
        }
    }
}
