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
    public class MeetingMinuteServiceTests
    {
        private readonly PMSContext _dbContext;
        private readonly MeetingMinuteService _meetingMinuteService;

        public MeetingMinuteServiceTests()
        {
            _dbContext = new PMSContext(new DbContextOptionsBuilder<PMSContext>()
                                            .UseSqlite(CreateInMemoryDatabase())
                                            .Options);
            _dbContext.Database.EnsureCreated();
            _meetingMinuteService = new MeetingMinuteService(_dbContext);
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

            MeetingMinute testMeetingMinute = new MeetingMinute
            {
                ProjectId = 1,
                Project = testProject,
                Title = "title",
                Content = "content"
            };

            _dbContext.Projects.Add(testProject);
            _dbContext.Users.Add(testUser);
            _dbContext.MeetingMinutes.Add(testMeetingMinute);
            _dbContext.SaveChanges();
        }

        [Fact]
        public void TestGetMeetingMinuteByProjectId()
        {
            List<MeetingMinute> meetingMinuteList = _meetingMinuteService.GetMeetingMinuteByProjectId(1);
            Assert.Single(meetingMinuteList);
            Assert.Equal("title", meetingMinuteList[0].Title);
            Assert.Equal("content", meetingMinuteList[0].Content);
        }

        [Fact]
        public void TestGetMeetinhMinuteByMeetingMinuteId()
        {
            MeetingMinute meetingMinute = _meetingMinuteService.GetMeetinhMinuteByMeetingMinuteId(1);
            Assert.Equal("title", meetingMinute.Title);
            Assert.Equal("content", meetingMinute.Content);
        }

        [Fact]
        public void TestCreateMeetingMinute()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "test title",
                content = "test content",
            };
            _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto);

            MeetingMinute meetingMinute = _meetingMinuteService.GetMeetinhMinuteByMeetingMinuteId(2);
            Assert.Equal("test title", meetingMinute.Title);
            Assert.Equal("test content", meetingMinute.Content);

            List<MeetingMinute> meetingMinuteList = _meetingMinuteService.GetMeetingMinuteByProjectId(1);
            Assert.Equal(2, meetingMinuteList.Count);
            Assert.Equal("test title", meetingMinuteList[1].Title);
            Assert.Equal("test content", meetingMinuteList[1].Content);
        }

        [Fact]
        public void TestCreateMeetingMinuteFailWithUnvalidProjectId()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = -1,
                title = "test title",
                content = "test content",
            };
            Assert.Throws<Exception>(() => _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto));
        }

        [Fact]
        public void TestCreateMeetingMinuteFailWithUnvalidTitleEmpty()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "",
                content = "test content",
            };
            Assert.Throws<Exception>(() => _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto));
        }

        [Fact]
        public void TestCreateMeetingMinuteFailWithUnvalidTitleDuplicate()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "title",
                content = "test content",
            };
            Assert.Throws<Exception>(() => _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto));
        }

        [Fact]
        public void TestEditMeetingMinuteInformation()
        {
            MeetingMinuteDto editedMeetingMinuteDto = new MeetingMinuteDto
            {
                meetingMinuteId = 1,
                title = "edited title",
                content = "edited content",
            };
            _meetingMinuteService.EditMeetingMinuteInformation(editedMeetingMinuteDto);

            MeetingMinute meetingMinute = _meetingMinuteService.GetMeetinhMinuteByMeetingMinuteId(1);
            Assert.Equal("edited title", meetingMinute.Title);
            Assert.Equal("edited content", meetingMinute.Content);

            List<MeetingMinute> meetingMinuteList = _meetingMinuteService.GetMeetingMinuteByProjectId(1);
            Assert.Single(meetingMinuteList);
            Assert.Equal("edited title", meetingMinuteList[0].Title);
            Assert.Equal("edited content", meetingMinuteList[0].Content);
        }

        [Fact]
        public void TestEditMeetingMinuteInformationFailWithUnvalidMeetingMinuteId()
        {
            MeetingMinuteDto editedMeetingMinuteDto = new MeetingMinuteDto
            {
                meetingMinuteId = -1,
                title = "edited title",
                content = "edited content",
            };

            Assert.Throws<Exception>(() => _meetingMinuteService.EditMeetingMinuteInformation(editedMeetingMinuteDto));
        }

        [Fact]
        public void TestEditMeetingMinuteInformationFailWithUnvalidTitleEmpty()
        {
            MeetingMinuteDto editedMeetingMinuteDto = new MeetingMinuteDto
            {
                meetingMinuteId = 1,
                title = "",
                content = "edited content",
            };

            Assert.Throws<Exception>(() => _meetingMinuteService.EditMeetingMinuteInformation(editedMeetingMinuteDto));
        }

        [Fact]
        public void TestEditMeetingMinuteInformationFailWithUnvalidTitleDuplicate()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "test title",
                content = "test content",
            };
            _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto);

            MeetingMinuteDto editedMeetingMinuteDto = new MeetingMinuteDto
            {
                meetingMinuteId = 1,
                title = "test title",
                content = "edited content",
            };

            Assert.Throws<Exception>(() => _meetingMinuteService.EditMeetingMinuteInformation(editedMeetingMinuteDto));
        }

        [Fact]
        public void TestDeleteMeetingMinute()
        {
            _meetingMinuteService.DeleteMeetingMinute(1);

            List<MeetingMinute> meetingMinuteList = _meetingMinuteService.GetMeetingMinuteByProjectId(1);
            Assert.Empty(meetingMinuteList);
        }

        [Fact]
        public void TestDeleteMeetingMinuteFail()
        {
            Assert.False(_meetingMinuteService.DeleteMeetingMinute(2));
        }
    }
}
