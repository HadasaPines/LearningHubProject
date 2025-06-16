using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BL.Api;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentServiceBL _studentServiceBL;
        public StudentController(IStudentServiceBL studentServiceBL)
        {
            _studentServiceBL = studentServiceBL;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentServiceBL.GetAllStudents();
            return Ok(students);
        }
        [HttpGet("{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var student = await _studentServiceBL.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }
            return Ok(student);
        }
        [HttpGet("name/{firstName}/{lastName}")]
        public async Task<IActionResult> GetStudentByName(string firstName, string lastName)
        {
            var student = await _studentServiceBL.GetStudentByName(firstName, lastName);
            if (student == null)
            {
                return NotFound($"Student with Name {firstName} {lastName} not found.");
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] StudentBL studentBL)
        {
            if (studentBL == null)
            {
                return BadRequest("Student data is null.");
            }
            await _studentServiceBL.AddStudent(studentBL);
            return CreatedAtAction(nameof(GetStudentById), new { studentId = studentBL.StudentId }, studentBL);
        }
        [HttpDelete("{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            try
            {
                await _studentServiceBL.DeleteStudent(studentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPatch("{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] JsonPatchDocument<StudentBL> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Patch document is null.");
            }
            try
            {
                var updatedStudent = await _studentServiceBL.UpdateStudent(studentId, patchDoc);
                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("{studentId}/registration")]
        public async Task<IActionResult> AddRegistrationToStudent(int studentId, [FromBody] RegistrationBL registrationBL)
        {
            if (registrationBL == null)
            {
                return BadRequest("Registration data is null.");
            }
            try
            {
                var registration = await _studentServiceBL.AddRegistrationToStudent(studentId, registrationBL);
                return CreatedAtAction(nameof(GetStudentById), new { studentId = studentId }, registration);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }


    }
}
