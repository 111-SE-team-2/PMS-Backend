using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Services;
using project_manage_system_backend.Shares;
using System;

namespace project_manage_system_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserScheduleOptionController : ControllerBase
    {
        private readonly UserScheduleOptionService _userScheduleOptionService;
        private readonly PMSContext _dbContext;

        public UserScheduleOptionController(PMSContext dbContext)
        {
            _dbContext = dbContext;
            _userScheduleOptionService = new UserScheduleOptionService(_dbContext);
        }

        [Authorize]
        [HttpPost("vote")]
        public IActionResult VoteScheduleOption(UserScheduleOptionDto userScheduleOptionDto)
        {
            try
            {
                _userScheduleOptionService.VoteScheduleOption(userScheduleOptionDto, User.Identity.Name);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "voted Success"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDto
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [Authorize]
        [HttpGet("{scheduleOptionId}/list")]
        public IActionResult GetUserListInScheduleOption(int scheduleOptionId)
        {
            var result = _userScheduleOptionService.GetUserListInScheduleOption(scheduleOptionId);
            return Ok(result);
        }
    }
}
