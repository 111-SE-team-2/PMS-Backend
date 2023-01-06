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
    public class MeetingMinuteController : ControllerBase
    {
        private readonly MeetingMinuteService _meetingMinuteService;
        public MeetingMinuteController(PMSContext dbContext)
        {
            _meetingMinuteService = new MeetingMinuteService(dbContext);
        }

        [Authorize]
        [HttpPost("create")]
        public IActionResult CreateMeetingMinute(MeetingMinuteDto meetingMinuteDto)
        {
            try
            {
                _meetingMinuteService.CreateMeetingMinute(meetingMinuteDto);
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
        public IActionResult EditMeetingMinuteInformation(MeetingMinuteDto meetingMinuteDto)
        {
            try
            {
                _meetingMinuteService.EditMeetingMinuteInformation(meetingMinuteDto);
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
        [HttpDelete("{meetingMinuteId}")]
        public IActionResult DeleteMeetingMinute(int meetingMinuteId)
        {
            try
            {
                bool isSuccess = _meetingMinuteService.DeleteMeetingMinute(meetingMinuteId);
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

        [Authorize]
        [HttpGet("project/{projectId}")]
        public IActionResult GetScheduleByProjectId(int projectId)
        {
            var result = _meetingMinuteService.GetMeetingMinuteByProjectId(projectId);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{meetingMinuteId}")]
        public IActionResult GetMeetinhMinuteByMeetingMinuteId(int meetingMinuteId)
        {
            var result = _meetingMinuteService.GetMeetinhMinuteByMeetingMinuteId(meetingMinuteId);
            return Ok(result);
        }
    }
}
