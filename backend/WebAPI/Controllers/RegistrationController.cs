using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        IRegistrationServiceBL _registrationServiceBL;
        public RegistrationController(IRegistrationServiceBL registrationServiceBL)
        {
            _registrationServiceBL = registrationServiceBL;


        }
        [HttpGet("getAllRegistrations")]
        public async Task<IActionResult> GetAllRegistrations()
        {
            var registrations = await _registrationServiceBL.GetAllRegistrations();
            if (registrations == null || !registrations.Any())
            {
                return NotFound("No registrations found");
            }

            return Ok(registrations);
        }
        [HttpGet("getRegistrationById/{registrationId}")]
        public IActionResult GetRegistrationById(int registrationId)
        {
            return Ok(_registrationServiceBL.GetRegistrationById(registrationId));
        }
        [HttpPost("addRegistration")]
        public async Task<IActionResult> AddRegistration([FromBody] RegistrationBL registrationBL)
        {
            await _registrationServiceBL.AddRegistration(registrationBL);
            return Ok("Registration added successfully");
        }
        [HttpPatch("updateRegistration/{id}")]
        public IActionResult UpdateRegistration(int id, [FromBody] JsonPatchDocument<RegistrationBL> patchDoc)
        {

            _registrationServiceBL.UpdateRegistration(id, patchDoc);
            return Ok("Registration updated successfully");

        }
        [HttpDelete("deleteRegistration/{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            await _registrationServiceBL.DeleteRegistration(id);
            return Ok("Registration deleted successfully");
        }


        [HttpDelete("deleteRegistrationByLessonId/{lessonId}")]
        public async Task<IActionResult> DeleteRegistrationByLessonId(int lessonId)
        {
            await _registrationServiceBL.DeleteRegistrationByLessonId(lessonId);
            return Ok($"Registration for lesson ID {lessonId} deleted successfully.");
        }

        [HttpGet("getRegistrationByLessonId/{lessonId}")]
        public async Task<IActionResult> GetRegistrationByLessonId(int lessonId)
        {
            var registration = await _registrationServiceBL.GetRegistrationByLessonId(lessonId);
            if (registration == null)
            {
                return NotFound($"No registration found for lesson ID {lessonId}.");
            }
            return Ok(registration);

        }



        [HttpGet("getRegistrationsToStudent")]
        public async Task<IActionResult> GetRegistrationsToStudent([FromBody] StudentBL studentBL)
        {
            var registrations = await _registrationServiceBL.GetRegistrationsToStudent(studentBL);
            if (registrations == null || !registrations.Any())
            {
                return NotFound("No registrations found for the specified student.");
            }
            return Ok(registrations);
        }
        [HttpGet("getLessonByRegistrationId/{registrationId}")]

        public IActionResult GetLessonByRegistrationId(int registrationId)
        {
            return Ok(_registrationServiceBL.GetLessonByRegistrationId(registrationId));
        }
        [HttpGet("getStudentByRegistrationId/{registrationId}")]
        public IActionResult GetStudentByRegistrationId(int registrationId)
        {
            return Ok(_registrationServiceBL.GetStudentByRegistrationId(registrationId));
        }

    }
}
