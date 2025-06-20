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
        public IActionResult GetAllRegistrations()
        {
            return Ok(_registrationServiceBL.GetAllRegistrations());
        }
        [HttpGet("getRegistrationById/{registrationId}")]
        public IActionResult GetRegistrationById(int registrationId)
        {
            return Ok(_registrationServiceBL.GetRegistrationById(registrationId));
        }
        [HttpPost("addRegistration")]
        public IActionResult AddRegistration([FromBody] RegistrationBL registrationBL)
        {
            _registrationServiceBL.AddRegistration(registrationBL);
            return Ok("Registration added successfully");
        }
        [HttpPatch("updateRegistration/{id}")]
        public IActionResult UpdateRegistration(int id, [FromBody] JsonPatchDocument<RegistrationBL> patchDoc)
        {

            _registrationServiceBL.UpdateRegistration(id, patchDoc);
            return Ok("Registration updated successfully");

        }
        [HttpDelete("deleteRegistration")]
        public IActionResult DeleteRegistration([FromBody] RegistrationBL registrationBL)
        {
            _registrationServiceBL.DeleteRegistration(registrationBL);
            return Ok("Registration deleted successfully");
        }
        [HttpGet("getRegistrationsToLesson")]
        public IActionResult GetRegistrationsToLesson([FromBody] LessonBL lessonBL)
        {
            return Ok(_registrationServiceBL.GetRegistrationsToLesson(lessonBL));
        }
        [HttpGet("getRegistrationsToStudent")]
        public IActionResult GetRegistrationsToStudent([FromBody] StudentBL studentBL)
        {
            return Ok(_registrationServiceBL.GetRegistrationsToStudent(studentBL));


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
