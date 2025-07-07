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
        public async Task<IActionResult> GetSubjectByID(int subjectId)
        {
            var subject = await _subjectServiceBL.GetSubjectById(subjectId);
            return Ok(subject);
        }
        [HttpPost("addSubject")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectBL subjectBL)
        {

           await _subjectServiceBL.AddSubject(subjectBL);
            return Ok("subject added successfully");
        }

        [HttpDelete("deleteSubjectByName/{subjectName}")]

        public async Task<IActionResult>  Delete(string subjectName)
        { 

           await _subjectServiceBL.DeleteSubjectByName(subjectName);
            return Ok("subject deleted successfully");
        }
        [HttpPatch("updateSubject/{id}")]
        public IActionResult UpdateSubject(int id, [FromBody] JsonPatchDocument<SubjectBL> patchDoc)
        {

            var subject = _subjectServiceBL.UpdateSubject(id, patchDoc);
            return Ok(subject);

        }
        [HttpGet("getTeachersBySubjectName/{subjectName}")]
        public async Task<IActionResult> GetTeachersBySubjectName(string subjectName)
        {
            var teachers = await _subjectServiceBL.GetTeachersBySubjectName(subjectName);
            if (teachers == null || !teachers.Any())
            {
                return NotFound($"No teachers found for subject: {subjectName}");
            }
            return Ok(teachers);
        }
        [HttpGet("getLessonsBySubjectName/{subjectName}")]
        public async Task<IActionResult> GetLessonsBySubjectName(string subjectName)
        {
            var lessons = await _subjectServiceBL.GetLessonsBySubjectName(subjectName);
            if (lessons == null || !lessons.Any())
            {
                return NotFound($"No lessons found for subject: {subjectName}");
            }
            return Ok(lessons);
        }
    }
}
