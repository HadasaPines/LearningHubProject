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
        [HttpGet("getAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _studentServiceBL.GetAllStudents();
            if (students == null || !students.Any())
            {
                return NotFound("No students found.");
            }
            return Ok(students);
        }
        [HttpGet("getStudentById/{studentId}")]
        public async Task<IActionResult> GetStudentById(int studentId)
        {
            var student = await _studentServiceBL.GetStudentById(studentId);
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }
            return Ok(student);
        }
        [HttpGet("getStudentByName/{firstName}/{lastName}")]
        public async Task<IActionResult> GetStudentByName(string firstName, string lastName)
        {
            var student = await _studentServiceBL.GetStudentByName(firstName, lastName);
            return Ok(student);
        }

        [HttpPost("addStudent")]
        public async Task<IActionResult> AddStudent([FromBody] StudentBL studentBL)
        {
            await _studentServiceBL.AddStudent(studentBL);
            return CreatedAtAction(nameof(GetStudentById), new { studentId = studentBL.StudentId }, studentBL);
        }
        [HttpDelete("deleteStudent/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {

                await _studentServiceBL.DeleteStudent(studentId);
            return Ok($"Student with ID {studentId} deleted successfully.");

        }
        [HttpPatch("updateStudent/{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, [FromBody] JsonPatchDocument<StudentBL> patchDoc)
        {


                var updatedStudent = await _studentServiceBL.UpdateStudent(studentId, patchDoc);
                return Ok(updatedStudent);

        }
        [HttpPost("addRegistrationToStudent/{studentId}")]
        public async Task<IActionResult> AddRegistrationToStudent(int studentId, [FromBody] RegistrationBL registrationBL)
        {

                var registration = await _studentServiceBL.AddRegistrationToStudent(studentId, registrationBL);
            return Ok($"Registration added successfully to student with id {studentId}");


        }


    }
}
