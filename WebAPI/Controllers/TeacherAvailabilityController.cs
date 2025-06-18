using BL.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.JsonPatch;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAvailabilityController : ControllerBase
    {
        private readonly ITeacherAvailabilityServiceBL _teacherAvailabilityServiceBL;
        public TeacherAvailabilityController(ITeacherAvailabilityServiceBL _teacherAvailabilityServiceBL)
        {
            _teacherAvailabilityServiceBL = _teacherAvailabilityServiceBL;
        }
        [HttpGet]
        public async Task<List<TeacherAvailabilityBL>> GetAllTeacherAvailabilities()
        {
            return await _teacherAvailabilityServiceBL.GetAllTeacherAvailabilities();

        }

        [HttpGet("GetTeacherAvailabilitiesByWeekDay/{weekDay}")]
        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByWeekDay(int weekDay)
        {
            return await _teacherAvailabilityServiceBL.GetTeacherAvailabilitiesByWeekDay(weekDay);
        }
        [HttpGet("GetTeacherAvailabilitiesByTimeRange")]
        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByTimeRange([FromQuery] TimeOnly startTime, [FromQuery] TimeOnly endTime)
        {
            return await _teacherAvailabilityServiceBL.GetTeacherAvailabilitiesByTimeRange(startTime, endTime);
        }
        [HttpPost("AddTeacherAvailability")]
        public async Task AddTeacherAvailability([FromBody] TeacherAvailabilityBL teacherAvailabilityBL)
        {

            await _teacherAvailabilityServiceBL.AddTeacherAvailability(teacherAvailabilityBL);

        }

        [HttpPatch("{id}")]
        public async Task<TeacherAvailabilityBL> UpdateTeacherAvailability(int id, [FromBody] JsonPatchDocument<TeacherAvailabilityBL> patchDoc)
        {

            return await _teacherAvailabilityServiceBL.UpdateTeacherAvailability(id, patchDoc);
        }

        [HttpDelete("{id}")]
        public async Task DeleteTeacherAvailability(int id)
        {
            await _teacherAvailabilityServiceBL.DeleteTeacherAvailability(id);


        }
    }
}
