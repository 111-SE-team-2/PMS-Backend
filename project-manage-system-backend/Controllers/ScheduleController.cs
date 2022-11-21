using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_manage_system_backend.Dtos;
using project_manage_system_backend.Dtos.Schedule;
using project_manage_system_backend.Services;
using project_manage_system_backend.Shares;
using System;
using System.Threading.Tasks;

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
    }
}
