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
    public class ScheduleOptionControllerTests : BaseControllerTests
    {
        public ScheduleOptionControllerTests() : base()
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
        public async Task TestGetScheduleOptionByScheduleId()
        {
            var requestTask = await _client.GetAsync("/scheduleOption/1");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<List<ScheduleOption>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Single(actual);
            Assert.Equal("1 hour", actual[0].Duration);
            Assert.Equal("2023/01/01 15:00", actual[0].StartTime);
            Assert.Equal(0, actual[0].NumberOfYes);
            Assert.Equal(0, actual[0].NumberOfIfNeedBe);
            Assert.Equal(0, actual[0].NumberOfCannotAttend);
            Assert.Equal(0, actual[0].NumberOfPending);
        }

        [Fact]
        public async Task TestAddScheduleOption()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = 1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleOptionDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/scheduleOption/add", content);

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = _dbContext.ScheduleOptions.Find(2);

            Assert.Equal("2 hour", actual.Duration);
            Assert.Equal("2022/12/31 19:00", actual.StartTime);
            Assert.Equal(0, actual.NumberOfYes);
            Assert.Equal(0, actual.NumberOfIfNeedBe);
            Assert.Equal(0, actual.NumberOfCannotAttend);
            Assert.Equal(0, actual.NumberOfPending);
        }

        [Fact]
        public async Task TestAddScheduleOptionFail()
        {
            ScheduleOptionDto scheduleOptionDto = new ScheduleOptionDto
            {
                scheduleId = -1,
                duration = "2 hour",
                startTime = "2022/12/31 19:00"
            };

            var content = new StringContent(JsonSerializer.Serialize(scheduleOptionDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/scheduleOption/add", content);

            var result = requestTask.Content.ReadAsStringAsync().Result;
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(result);
            Assert.False(responseDto.success);
        }

        [Fact]
        public async Task TestDeleteScheduleOption()
        {
            var requestTask = await _client.DeleteAsync("/scheduleOption/1");

            Assert.True(requestTask.IsSuccessStatusCode);

            var requestTask2 = await _client.GetAsync("/scheduleOption/1");
            string resultContent = await requestTask2.Content.ReadAsStringAsync();
            var actual = JsonSerializer.Deserialize<List<ScheduleOption>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Empty(actual);
        }

        [Fact]
        public async Task TestDeleteScheduleOptionFail()
        {
            var requestTask = await _client.DeleteAsync("/scheduleOption/-1");

            Assert.True(requestTask.IsSuccessStatusCode);

            var result = requestTask.Content.ReadAsStringAsync().Result;
            var responseDto = JsonSerializer.Deserialize<ResponseDto>(result);
            Assert.False(responseDto.success);
        }
    }
}
