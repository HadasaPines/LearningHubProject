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
        public IActionResult Get()
        {

            var subjects = _subjectServiceBL.GetAllSubjects();
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
            return Ok();
        }
       
        [HttpDelete("deleteSubjectByName/{subjectName}")]

        public IActionResult Delete(string name)
        {
            var subject = _subjectServiceBL.GetSubjectByName(name);

            _subjectServiceBL.DeleteSubjectByName(name);
            return NoContent();
        }
        [HttpPatch("updateSubject/{id}")]
        public IActionResult UpdateSubject(int id, [FromBody] JsonPatchDocument<SubjectBL> patchDoc)
        {

            var subject = _subjectServiceBL.UpdateSubject(id, patchDoc);
            return Ok(subject);

        }
        [HttpGet("getTeachersBySubjectName/{subjectName}")]
        public IActionResult GetTeachersBySubjectName(string name)
        {
            var teachers = _subjectServiceBL.GetTeachersBySubjectName(name);
            return Ok(teachers);
        }
        [HttpGet("getLessonsBySubjectName/{subjectName}")]
        public IActionResult GetLessonsBySubjectName(string name)
        {
            var lessons = _subjectServiceBL.GetLessonsBySubjectName(name);
            return Ok(lessons);
        }
    }
}
