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
    public class UserScheduleOptionServiceTests
    {
        private readonly PMSContext _dbContext;
        private readonly UserScheduleOptionService _userScheduleOptionService;

        public UserScheduleOptionServiceTests()
        {
            _dbContext = new PMSContext(new DbContextOptionsBuilder<PMSContext>()
                                            .UseSqlite(CreateInMemoryDatabase())
                                            .Options);
            _dbContext.Database.EnsureCreated();
            _userScheduleOptionService = new UserScheduleOptionService(_dbContext);
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
                Users = new List<UserScheduleOption>(),
                NumberOfYes = 0,
                NumberOfIfNeedBe = 0,
                NumberOfCannotAttend = 0,
                NumberOfPending = 0,
            };

            UserScheduleOption userScheduleOption = new UserScheduleOption
            {
                Account = "github_testUser",
                User = testUser,
                ScheduleOptionId = 1,
                ScheduleOption = testScheduleOption,
                Availability = "Yes"
            };

            testSchedule.ScheduleOptions.Add(testScheduleOption);
            testScheduleOption.Users.Add(userScheduleOption);

            _dbContext.Projects.Add(testProject);
            _dbContext.Users.Add(testUser);
            _dbContext.Schedules.Add(testSchedule);
            _dbContext.ScheduleOptions.Add(testScheduleOption);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void TestGetUserListInScheduleOption()
        {
            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(1);
            Assert.Single(userScheduleOptionList);
            Assert.Equal("github_testUser", userScheduleOptionList[0].Account);
            Assert.Equal(1, userScheduleOptionList[0].ScheduleOptionId);
            Assert.Equal("Yes", userScheduleOptionList[0].Availability);
        }

        [Fact]
        public void TestVoteScheduleOption()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 1,
                Availability = "If Need Be"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(1);
            Assert.Single(userScheduleOptionList);
            Assert.Equal("github_testUser", userScheduleOptionList[0].Account);
            Assert.Equal(1, userScheduleOptionList[0].ScheduleOptionId);
            Assert.Equal("If Need Be", userScheduleOptionList[0].Availability);
        }
    }
}
