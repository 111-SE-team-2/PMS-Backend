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
    public class ScheduleOptionController : ControllerBase
    {
        private readonly ScheduleOptionService _scheduleOptionService;
        public ScheduleOptionController(PMSContext dbContext)
        {
            _scheduleOptionService = new ScheduleOptionService(dbContext);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddScheduleOption(ScheduleOptionDto scheduleOptionDto)
        {
            try
            {
                _scheduleOptionService.AddScheduleOption(scheduleOptionDto);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Added Success"
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
        [HttpDelete("{scheduleOptionId}")]
        public IActionResult DeleteScheduleOption(int scheduleOptionId)
        {
            bool isSuccess = _scheduleOptionService.DeleteScheduleOption(scheduleOptionId);
            return Ok(new ResponseDto()
            {
                success = isSuccess,
                message = isSuccess ? "Success" : "Error"
            });
        }

        [Authorize]
        [HttpGet("{scheduleId}")]
        public IActionResult GetScheduleOptionByScheduleId(int scheduleId)
        {
            var result = _scheduleOptionService.GetScheduleOptionByScheduleId(scheduleId);
            return Ok(result);
        }
    }
}
