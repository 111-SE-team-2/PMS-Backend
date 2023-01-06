using project_manage_system_backend.Dtos;
using project_manage_system_backend.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace PMS_test.ControllersTest
{
    [TestCaseOrderer("XUnit.Project.Orderers.AlphabeticalOrderer", "XUnit.Project")]
    public class MeetingMinuteControllerTests : BaseControllerTests
    {
        public MeetingMinuteControllerTests() : base()
        {
            InitialDatabase();
        }

        internal void InitialDatabase()
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
        public async Task TestGetMeetingMinuteByProjectId()
        {
            var requestTask = await _client.GetAsync("/meetingMinute/project/1");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<List<MeetingMinute>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Single(actual);
            Assert.Equal("title", actual[0].Title);
            Assert.Equal("content", actual[0].Content);
        }

        [Fact]
        public async Task TestGetMeetinhMinuteByMeetingMinuteId()
        {
            var requestTask = await _client.GetAsync("/meetingMinute/1");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<MeetingMinute>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("title", actual.Title);
            Assert.Equal("content", actual.Content);
        }

        [Fact]
        public async Task TestCreateMeetingMinute()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "test title",
                content = "test content"
            };

            var content = new StringContent(JsonSerializer.Serialize(meetingMinuteDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/meetingMinute/create", content);

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = _dbContext.MeetingMinutes.Find(2);

            Assert.Equal("test title", actual.Title);
            Assert.Equal("test content", actual.Content);
        }

        [Fact]
        public async Task TestEditMeetingMinuteInformation()
        {
            MeetingMinuteDto meetingMinuteDto = new MeetingMinuteDto
            {
                projectId = 1,
                title = "test title",
                content = "test content"
            };

            var content = new StringContent(JsonSerializer.Serialize(meetingMinuteDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/meetingMinute/create", content);

            MeetingMinuteDto meetingMinuteDto2 = new MeetingMinuteDto
            {
                meetingMinuteId = 2,
                title = "edited title",
                content = "edited content"
            };

            var content2 = new StringContent(JsonSerializer.Serialize(meetingMinuteDto2), Encoding.UTF8, "application/json");
            var requestTask2 = await _client.PutAsync("/meetingMinute/edit", content2);

            Assert.True(requestTask2.IsSuccessStatusCode);

            var actual = _dbContext.MeetingMinutes.Find(2);

            Assert.Equal("edited title", actual.Title);
            Assert.Equal("edited content", actual.Content);
        }

        [Fact]
        public async Task TestDeleteMeetingMinute()
        {
            var requestTask = await _client.DeleteAsync("/meetingMinute/1");

            Assert.True(requestTask.IsSuccessStatusCode);

            var requestTask2 = await _client.GetAsync("/meetingMinute/project/1");
            string resultContent = await requestTask2.Content.ReadAsStringAsync();
            var actual = JsonSerializer.Deserialize<List<MeetingMinute>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Empty(actual);
        }
    }
}
