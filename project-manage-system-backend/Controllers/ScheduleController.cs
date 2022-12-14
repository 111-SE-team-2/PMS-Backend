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
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;
        public ScheduleController(PMSContext dbContext)
        {
            _scheduleService = new ScheduleService(dbContext);
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateSchedule(ScheduleDto scheduleDto)
        {
            try
            {
                _scheduleService.CreateSchedule(scheduleDto);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Created Success"
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
        [HttpPut("edit")]
        public IActionResult EditScheduleInformation(ScheduleDto scheduleDto)
        {
            try
            {
                _scheduleService.EditScheduleInformation(scheduleDto);
                return Ok(new ResponseDto
                {
                    success = true,
                    message = "Edited Success",
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseDto
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        [Authorize]
        [HttpDelete("{scheduleId}")]
        public IActionResult DeleteSchedule(int scheduleId)
        {
            bool isSuccess = _scheduleService.DeleteSchedule(scheduleId);
            return Ok(new ResponseDto()
            {
                success = isSuccess,
                message = isSuccess ? "Success" : "Error"
            });
        }

        [Authorize]
        [HttpGet("project/{projectId}")]
        public IActionResult GetScheduleByProjectId(int projectId)
        {
            var result = _scheduleService.GetScheduleByProjectId(projectId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{scheduleId}")]
        public IActionResult GetScheduleByScheduleId(int scheduleId)
        {
            var result = _scheduleService.GetScheduleByScheduleId(scheduleId);
            return Ok(result);
        }
    }
}
