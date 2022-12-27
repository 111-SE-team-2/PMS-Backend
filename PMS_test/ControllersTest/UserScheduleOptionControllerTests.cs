using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
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
    public class UserScheduleOptionControllerTests : BaseControllerTests
    {
        public UserScheduleOptionControllerTests() : base()
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
                Users = new List<UserScheduleOption>(),
                NumberOfYes = 0,
                NumberOfIfNeedBe = 0,
                NumberOfCannotAttend = 0,
                NumberOfPending = 0,
            };

            UserScheduleOption userScheduleOption = new UserScheduleOption
            {
                Account = "github_testuser",
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
        public async Task TestGetUserListInScheduleOption()
        {
            var requestTask = await _client.GetAsync("/userScheduleOption/1/list");
            string resultContent = await requestTask.Content.ReadAsStringAsync();

            Assert.True(requestTask.IsSuccessStatusCode);

            var actual = JsonSerializer.Deserialize<List<UserScheduleOption>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Single(actual);
            Assert.Equal("github_testUser", actual[0].Account);
            Assert.Equal(1, actual[0].ScheduleOptionId);
            Assert.Equal("Yes", actual[0].Availability);
        }

        [Fact]
        public async Task TestVoteScheduleOption()
        {
            UserScheduleOptionDto userScheduleOptionDto = new UserScheduleOptionDto
            {
                ScheduleOptionId = 1,
                Availability = "If Need Be"
            };

            var content = new StringContent(JsonSerializer.Serialize(userScheduleOptionDto), Encoding.UTF8, "application/json");
            var requestTask = await _client.PostAsync("/userScheduleOption/vote", content);

            Assert.True(requestTask.IsSuccessStatusCode);

            var requestTask2 = await _client.GetAsync("/userScheduleOption/1/list");
            string resultContent = await requestTask2.Content.ReadAsStringAsync();
            var actual = JsonSerializer.Deserialize<List<UserScheduleOption>>(resultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(2, actual.Count);
            Assert.Equal("github_testuser", actual[1].Account);
            Assert.Equal(1, actual[1].ScheduleOptionId);
            Assert.Equal("If Need Be", actual[1].Availability);
        }
    }
}
