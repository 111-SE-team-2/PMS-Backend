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
    public class ScheduleControllerTests : BaseControllerTests
    {
        public ScheduleControllerTests() : base()
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
        public async Task TestGetScheduleByProjectId()
        {
            var requestTask = await _client.GetAsync("/schedule/project/1");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<List<Schedule>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Single(actual);
            Assert.Equal("title", actual[0].Title);
            Assert.Equal("location", actual[0].Location);
            Assert.Equal("description", actual[0].Description);
            Assert.False(actual[0].IsVideoConferencing);
        }

        [Fact]
        public async Task TestGetScheduleByScheduleId()
        {
            var requestTask = await _client.GetAsync("/schedule/1");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<Schedule>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal("title", actual.Title);
            Assert.Equal("location", actual.Location);
            Assert.Equal("description", actual.Description);
            Assert.False(actual.IsVideoConferencing);
        }

        [Fact]
        public async Task TestCreateSchedule()
        {
            ScheduleDto scheduleDto = new ScheduleDto
            {
                projectId = 1,
                title = "test title",
                location = "test location",
                description = "test description",
                isVideoConferencing = true
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/schedule/create", content);

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = _dbContext.Schedules.Find(2);

            Assert.Equal("test title", actual.Title);
            Assert.Equal("test location", actual.Location);
            Assert.Equal("test description", actual.Description);
            Assert.True(actual.IsVideoConferencing);
        }

        [Fact]
        public async Task TestCreateScheduleFail()
        {
            ScheduleDto scheduleDto = new ScheduleDto
            {
                projectId = -1,
                title = "test title",
                location = "test location",
                description = "test description",
                isVideoConferencing = true
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/schedule/create", content);

            var result = requestTask.Content.ReadAsStringAsync().Result;
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(result);
            Assert.False(responseDto.success);
        }

        [Fact]
        public async Task TestEditScheduleInformation()
        {
            ScheduleDto scheduleDto = new ScheduleDto
            {
                projectId = 1,
                title = "test title",
                location = "test location",
                description = "test description",
                isVideoConferencing = true
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleDto), Encoding.UTF8, "application/json");
            await _client.PostAsync("/schedule/create", content);

            ScheduleDto scheduleDto2 = new ScheduleDto
            {
                scheduleId = 2,
                title = "edited title",
                location = "edited location",
                description = "edited description",
                isVideoConferencing = false
            };

            var content2 = new StringContent(JsonSerializer.Serialize(scheduleDto2), Encoding.UTF8, "application/json");
            var requestTask2 = await _client.PutAsync("/schedule/edit", content2);

            Assert.True(requestTask2.IsSuccessStatusCode);

            var actual = _dbContext.Schedules.Find(2);

            Assert.Equal("edited title", actual.Title);
            Assert.Equal("edited location", actual.Location);
            Assert.Equal("edited description", actual.Description);
            Assert.False(actual.IsVideoConferencing);
        }

        [Fact]
        public async Task TestEditScheduleInformationFail()
        {
            ScheduleDto scheduleDto = new ScheduleDto
            {
                scheduleId = -1,
                title = "edited title",
                location = "edited location",
                description = "edited description",
                isVideoConferencing = false
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PutAsync("/schedule/edit", content);

            var result = requestTask.Content.ReadAsStringAsync().Result;
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(result);
            Assert.False(responseDto.success);
        }

        [Fact]
        public async Task TestDeleteSchedule()
        {
            var requestTask = await _client.DeleteAsync("/schedule/1");

            Assert.True(requestTask.IsSuccessStatusCode);

            var requestTask2 = await _client.GetAsync("/schedule/project/1");
            string resultContent = await requestTask2.Content.ReadAsStringAsync();
            var actual = JsonSerializer.Deserialize<List<Schedule>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Empty(actual);
        }

        [Fact]
        public async Task TestDeleteScheduleFail()
        {
            var requestTask = await _client.DeleteAsync("/schedule/-1");

            Assert.True(requestTask.IsSuccessStatusCode);

            var result = requestTask.Content.ReadAsStringAsync().Result;
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(result);
            Assert.False(responseDto.success);
        }
    }
}
