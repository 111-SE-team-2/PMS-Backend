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
using System.Drawing;
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

            User testUser2 = new User
            {
                Account = "github_testUser2",
                Name = "testUser2",
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

            List<UserProject> testUserProject2 = new List<UserProject>()
            {
                new UserProject
                {
                    User = testUser2,
                    Project = testProject,
                },
            };

            testUser.Projects = testUserProject;
            testUser2.Projects = testUserProject2;

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

            ScheduleOption testScheduleOption2 = new ScheduleOption
            {
                Schedule = testSchedule,
                Duration = "2 hour",
                StartTime = "2023/01/02 15:00",
                Users = new List<UserScheduleOption>(),
                NumberOfYes = 0,
                NumberOfIfNeedBe = 0,
                NumberOfCannotAttend = 0,
                NumberOfPending = 0,
            };

            ScheduleOption testScheduleOption3 = new ScheduleOption
            {
                Schedule = testSchedule,
                Duration = "3 hour",
                StartTime = "2023/01/03 15:00",
                Users = new List<UserScheduleOption>(),
                NumberOfYes = 0,
                NumberOfIfNeedBe = 0,
                NumberOfCannotAttend = 0,
                NumberOfPending = 0,
            };

            ScheduleOption testScheduleOption4 = new ScheduleOption
            {
                Schedule = testSchedule,
                Duration = "4 hour",
                StartTime = "2023/01/04 15:00",
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

            UserScheduleOption userScheduleOption2 = new UserScheduleOption
            {
                Account = "github_testUser",
                User = testUser,
                ScheduleOptionId = 2,
                ScheduleOption = testScheduleOption2,
                Availability = "If Need Be"
            };

            UserScheduleOption userScheduleOption3 = new UserScheduleOption
            {
                Account = "github_testUser",
                User = testUser,
                ScheduleOptionId = 3,
                ScheduleOption = testScheduleOption3,
                Availability = "Cannot Attend"
            };

            UserScheduleOption userScheduleOption4 = new UserScheduleOption
            {
                Account = "github_testUser",
                User = testUser,
                ScheduleOptionId = 4,
                ScheduleOption = testScheduleOption4,
                Availability = "Pending"
            };

            testSchedule.ScheduleOptions.Add(testScheduleOption);
            testSchedule.ScheduleOptions.Add(testScheduleOption2);
            testSchedule.ScheduleOptions.Add(testScheduleOption3);
            testSchedule.ScheduleOptions.Add(testScheduleOption4);
            testScheduleOption.Users.Add(userScheduleOption);
            testScheduleOption2.Users.Add(userScheduleOption2);
            testScheduleOption3.Users.Add(userScheduleOption3);
            testScheduleOption4.Users.Add(userScheduleOption4);

            _dbContext.Projects.Add(testProject);
            _dbContext.Users.Add(testUser);
            _dbContext.Users.Add(testUser2);
            _dbContext.Schedules.Add(testSchedule);
            _dbContext.ScheduleOptions.Add(testScheduleOption);
            _dbContext.ScheduleOptions.Add(testScheduleOption2);
            _dbContext.ScheduleOptions.Add(testScheduleOption3);
            _dbContext.ScheduleOptions.Add(testScheduleOption4);
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

        [Fact]
        public void TestVoteScheduleOption2()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 2,
                Availability = "Cannot Attend"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(2);
            Assert.Single(userScheduleOptionList);
            Assert.Equal("github_testUser", userScheduleOptionList[0].Account);
            Assert.Equal(2, userScheduleOptionList[0].ScheduleOptionId);
            Assert.Equal("Cannot Attend", userScheduleOptionList[0].Availability);
        }

        [Fact]
        public void TestVoteScheduleOption3()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 3,
                Availability = "Pending"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(3);
            Assert.Single(userScheduleOptionList);
            Assert.Equal("github_testUser", userScheduleOptionList[0].Account);
            Assert.Equal(3, userScheduleOptionList[0].ScheduleOptionId);
            Assert.Equal("Pending", userScheduleOptionList[0].Availability);
        }

        [Fact]
        public void TestVoteScheduleOption4()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 4,
                Availability = "Yes"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(4);
            Assert.Single(userScheduleOptionList);
            Assert.Equal("github_testUser", userScheduleOptionList[0].Account);
            Assert.Equal(4, userScheduleOptionList[0].ScheduleOptionId);
            Assert.Equal("Yes", userScheduleOptionList[0].Availability);
        }

        [Fact]
        public void TestNewVoteScheduleOption()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 1,
                Availability = "If Need Be"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser2");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(1);
            Assert.Equal(2, userScheduleOptionList.Count);
            Assert.Equal("github_testUser2", userScheduleOptionList[1].Account);
            Assert.Equal(1, userScheduleOptionList[1].ScheduleOptionId);
            Assert.Equal("If Need Be", userScheduleOptionList[1].Availability);
        }

        [Fact]
        public void TestNewVoteScheduleOption2()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 2,
                Availability = "Cannot Attend"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser2");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(2);
            Assert.Equal(2, userScheduleOptionList.Count);
            Assert.Equal("github_testUser2", userScheduleOptionList[1].Account);
            Assert.Equal(2, userScheduleOptionList[1].ScheduleOptionId);
            Assert.Equal("Cannot Attend", userScheduleOptionList[1].Availability);
        }

        [Fact]
        public void TestNewVoteScheduleOption3()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 3,
                Availability = "Pending"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser2");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(3);
            Assert.Equal(2, userScheduleOptionList.Count);
            Assert.Equal("github_testUser2", userScheduleOptionList[1].Account);
            Assert.Equal(3, userScheduleOptionList[1].ScheduleOptionId);
            Assert.Equal("Pending", userScheduleOptionList[1].Availability);
        }

        [Fact]
        public void TestNewVoteScheduleOption4()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 4,
                Availability = "Yes"
            };
            _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser2");

            List<UserScheduleOption> userScheduleOptionList = _userScheduleOptionService.GetUserListInScheduleOption(4);
            Assert.Equal(2, userScheduleOptionList.Count);
            Assert.Equal("github_testUser2", userScheduleOptionList[1].Account);
            Assert.Equal(4, userScheduleOptionList[1].ScheduleOptionId);
            Assert.Equal("Yes", userScheduleOptionList[1].Availability);
        }

        [Fact]
        public void TestVoteScheduleOptionFailWithUnvalidScheduleOptionId()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = -1,
                Availability = "If Need Be"
            };
            
            Assert.Throws<Exception>(() => _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser"));
        }

        [Fact]
        public void TestVoteScheduleOptionFailWithUnvalidAvailability()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 1,
                Availability = ""
            };

            Assert.Throws<Exception>(() => _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser"));
        }

        [Fact]
        public void TestVoteScheduleOptionFailWithUnvalidUserNotExist()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 1,
                Availability = "If Need Be"
            };

            Assert.Throws<Exception>(() => _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, "github_testUser0"));
        }
    }
}
