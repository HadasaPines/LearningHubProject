using BL.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAvailabilityController : ControllerBase
    {
        private readonly ITeacherAvailabilityServiceBL _teacherAvailabilityServiceBL;

        public TeacherAvailabilityController(ITeacherAvailabilityServiceBL teacherAvailabilityServiceBL)
        {
            _teacherAvailabilityServiceBL = teacherAvailabilityServiceBL;
        }


        [HttpGet("getAllTeacherAvailability")]

        public async Task<IActionResult> GetAllTeacherAvailabilities()
        {
            var availabilities = await _teacherAvailabilityServiceBL.GetAllTeacherAvailabilities();
            if (availabilities == null || !availabilities.Any())
            {
                return NotFound("No teacher availabilities found.");
            }
            return Ok(availabilities);
        }

        [HttpGet("GetTeacherAvailabilitiesByWeekDay/{weekDay}")]
        public async Task<IActionResult> GetTeacherAvailabilitiesByWeekDay(int weekDay)
        {
            var availabilities = await _teacherAvailabilityServiceBL.GetTeacherAvailabilitiesByWeekDay(weekDay);
            if (availabilities == null || !availabilities.Any())
            {
                return NotFound($"No teacher availabilities found for week day {weekDay}.");
            }
            return Ok(availabilities);
        }

        [HttpGet("GetTeacherAvailabilitiesByTimeRange")]
        public async Task<IActionResult> GetTeacherAvailabilitiesByTimeRange([FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime)
        {
            var availabilities = await _teacherAvailabilityServiceBL.GetTeacherAvailabilitiesByTimeRange(startTime, endTime);
            if (availabilities == null || !availabilities.Any())
            {
                return NotFound("No teacher availabilities found for the specified time range.");
            }
            return Ok(availabilities);
        }

        [HttpPost("AddTeacherAvailability")]
        public async Task<IActionResult> AddTeacherAvailability([FromBody] TeacherAvailabilityBL teacherAvailabilityBL)
        {
            await _teacherAvailabilityServiceBL.AddTeacherAvailability(teacherAvailabilityBL);
            return Ok($"Teacher availability with id {teacherAvailabilityBL.AvailabilityId} added successfully.");
        }


        [HttpPatch("updateTeacherAvailability/{id}")]

        public async Task<IActionResult> UpdateTeacherAvailability(int id, [FromBody] JsonPatchDocument<TeacherAvailabilityBL> patchDoc)
        {
            var updated = await _teacherAvailabilityServiceBL.UpdateTeacherAvailability(id, patchDoc);

            return Ok(updated);
        }

        [HttpDelete("deleteTeacherAvailability{id}")]

        public async Task<IActionResult> DeleteTeacherAvailability(int id)
        {
            await _teacherAvailabilityServiceBL.DeleteTeacherAvailability(id);
         return Ok($"eacher availability with id {id} deleted successfully.");
        }
    }
}
