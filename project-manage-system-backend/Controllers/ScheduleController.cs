using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
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
                    message = "Created Error" + ex.Message
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
        [HttpDelete("{projectId}/{scheduleId}")]
        public IActionResult DeleteRepo(int projectId, int scheduleId)
        {
            try
            {
                bool isSuccess = _scheduleService.DeleteSchedule(projectId, scheduleId);
                return Ok(new ResponseDto()
                {
                    success = isSuccess,
                    message = isSuccess ? "Success" : "Error"
                });
            }
            catch (Exception e)
            {
                return Ok(new ResponseDto()
                {
                    success = false,
                    message = e.Message
                });
            }
        }
    }
}
