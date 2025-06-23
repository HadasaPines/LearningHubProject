using BL.Api;
using BL.Models;
using DAL.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectServiceBL _subjectServiceBL;
        public SubjectController(ISubjectServiceBL subjectServiceBL)
        {
            _subjectServiceBL = subjectServiceBL;
        }
        [HttpGet("getAllSubjects")]
        public async Task<IActionResult> Get()
        {
            var subjects = await _subjectServiceBL.GetAllSubjects();
            if (subjects == null || !subjects.Any())
            {
                return NotFound("No subjects found.");
            }
            return Ok(subjects);
        }
        [HttpGet("getSubjectById/{subjectId}")]
        public IActionResult GetSubjectByID(int id)
        {
            var subject = _subjectServiceBL.GetSubjectById(id);
            return Ok(subject);
        }
        [HttpPost("addSubject")]
        public IActionResult AddSubject([FromBody] SubjectBL subjectBL)
        {

            _subjectServiceBL.AddSubject(subjectBL);
            return Ok("subject added successfully");
        }

        [HttpDelete("deleteSubjectByName/{subjectName}")]

        public IActionResult Delete(string name)
        {
            var subject = _subjectServiceBL.GetSubjectByName(name);

            _subjectServiceBL.DeleteSubjectByName(name);
            return Ok("subject deleted successfully");
        }
        [HttpPatch("updateSubject/{id}")]
        public IActionResult UpdateSubject(int id, [FromBody] JsonPatchDocument<SubjectBL> patchDoc)
        {

            var subject = _subjectServiceBL.UpdateSubject(id, patchDoc);
            return Ok(subject);

        }
        [HttpGet("getTeachersBySubjectName/{subjectName}")]
        public async Task<IActionResult> GetTeachersBySubjectName(string name)
        {
            var teachers = await _subjectServiceBL.GetTeachersBySubjectName(name);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No teachers found for subject: {name}");
            }
            return Ok(teachers);
        }
        [HttpGet("getLessonsBySubjectName/{subjectName}")]
        public async Task<IActionResult> GetLessonsBySubjectName(string name)
        {
            var lessons = await _subjectServiceBL.GetLessonsBySubjectName(name);
            if (lessons == null || !lessons.Any())
            {
                return NotFound($"No lessons found for subject: {name}");
            }
            return Ok(lessons);
        }
    }
}
