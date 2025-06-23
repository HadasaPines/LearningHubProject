using BL.Api;
using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherServiceBL _teacherServiceBL;

        public TeacherController(ITeacherServiceBL teacherServiceBL)
        {
            _teacherServiceBL = teacherServiceBL;
        }

        [HttpPost("addAvailability/{teacherId}")]
        public async Task<IActionResult> AddAvailabilityToTeacher(int teacherId, [FromBody] TeacherAvailabilityBL availability)
        {
            await _teacherServiceBL.AddAvailabilityToTeacher(teacherId, availability);
            return Ok($"Teacher availability added successfully.");
        }

        [HttpPost("addLesson/{teacherId}")]
        public async Task<IActionResult> AddLessonToTeacher(int teacherId, [FromBody] LessonBL lessonBL)
        {
            await _teacherServiceBL.AddLessonToTeacher(teacherId, lessonBL);
            return Ok($"Lesson added successfully to teacher with id {teacherId}.");
        }

        [HttpPost("addSubject/{teacherId}")]
        public async Task<IActionResult> AddSubjectToTeacher(int teacherId, [FromBody] TeachersToSubjectBL teachersToSubjectBL)
        {
            await _teacherServiceBL.AddSubjectToTeacher(teacherId, teachersToSubjectBL);
            return Ok($"Subject with id {teachersToSubjectBL.SubjectId} added successfully to teacher with id {teacherId}.");
        }

        [HttpPost("addTeacher")]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherBL teacherBL)
        {
            await _teacherServiceBL.AddTeacher(teacherBL);
            return Ok($"Teacher with id {teacherBL.TeacherId} added successfully.");
        }

        [HttpDelete("deleteTeacher/{teacherId}")]
        public async Task<IActionResult> DeleteTeacher(int teacherId)
        {
            await _teacherServiceBL.DeleteTeacher(teacherId);
            return Ok($"Teacher with id {teacherId} deleted successfully.");
        }

        [HttpGet("getAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {

            var teachers = await _teacherServiceBL.GetAllTeachers();
            if (teachers == null || !teachers.Any())
            {
                return NotFound("No teachers found.");
            }

            return Ok(teachers);
        }

        [HttpGet("getTeacherById/{teacherId}")]
        public async Task<IActionResult> GetTeacherById(int teacherId)
        {
            var teacher = await _teacherServiceBL.GetTeacherById(teacherId);
    

            return Ok(teacher);
        }

        [HttpGet("getTeacherByName/{firstName}/{lastName}")]
        public async Task<IActionResult> GetTeacherByName(string firstName, string lastName)
        {
            var teacher = await _teacherServiceBL.GetTeacherByName(firstName, lastName);
    

            return Ok(teacher);
        }

        [HttpGet("getTeachersByAvailability/{date}/{startTime}/{endTime}")]
        public async Task<IActionResult> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var teachers = await _teacherServiceBL.GetTeachersByAvailability(date, startTime, endTime);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No teachers found available on {date.ToShortDateString()} between {startTime} and {endTime}.");
            }
            return Ok(teachers);
        }

        [HttpGet("getTeachersBySubject/{subjectId}")]
        public async Task<IActionResult> GetTeachersBySubject(int subjectId)
        {
            var teachers = await _teacherServiceBL.GetTeachersBySubject(subjectId);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No teachers found for subject with ID {subjectId}.");
            }
            return Ok(teachers);
        }

        [HttpGet("getLessonsByTeacherId/{teacherId}")]
        public async Task<IActionResult> GetLessonsByTeacherId(int teacherId)
        {
            var lessons = await _teacherServiceBL.GetLessonsByTeacherId(teacherId);
            if (lessons == null || !lessons.Any())
            {
                return NotFound($"No lessons found for teacher with ID {teacherId}.");
            }
            return Ok(lessons);
        }

        [HttpGet("getTeacherAvailabilities/{teacherId}")]
        public async Task<IActionResult> GetTeacherAvailabilities(int teacherId)
        {
            var teachers = await _teacherServiceBL.GetTeacherAvailabilities(teacherId);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No availabilities found for teacher with ID {teacherId}.");
            }
            return Ok(teachers);
        }

        [HttpGet("getTeachersSubjects/{teacherId}")]
        public async Task<IActionResult> GetTeachersSubjects(int teacherId)
        {
            var teachers = await _teacherServiceBL.GetTeachersSubjects(teacherId);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No subjects found for teacher with ID {teacherId}.");
            }
            return Ok(teachers);
        }

        [HttpPatch("updateTeacher/{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] JsonPatchDocument<TeacherBL> patchDoc)
        {
            var updated = await _teacherServiceBL.UpdateTeacher(id, patchDoc);

            return Ok(updated);
        }
    }
}
