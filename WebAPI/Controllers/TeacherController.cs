using BL.Api;
using BL.Models;
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
        ITeacherServiceBL _teacherServiceBL;
        public TeacherController(ITeacherServiceBL teacherServiceBL)
        {

            _teacherServiceBL = teacherServiceBL;
        }


        [HttpPost("addAvailability/{teacherId}")]
        public Task AddAvailabilityToTeacher(int teacherId, [FromBody] TeacherAvailabilityBL availability)
        {
            return (_teacherServiceBL.AddAvailabilityToTeacher(teacherId, availability));



        }
        [HttpPost("addLesson/{teacherId}")]
        public Task AddLessonToTeacher(int teacherId, [FromBody] LessonBL lessonBL)
        {
            return (_teacherServiceBL.AddLessonToTeacher(teacherId, lessonBL));
        }
        [HttpPost("addSubject/{teacherId}")]

        public Task AddSubjectToTeacher(int teacherId, [FromBody] TeachersToSubjectBL teachersToSubjectBL)
        {
            return (_teacherServiceBL.AddSubjectToTeacher(teacherId, teachersToSubjectBL));
        }
        [HttpPost("addTeacher")]
        public Task AddTeacher([FromBody] TeacherBL TeacherBL)
        {
            return (_teacherServiceBL.AddTeacher(TeacherBL));
        }

        [HttpDelete("deleteTeacher/{teacherId}")]
        public Task DeleteTeacher(int teacherId)
        {
            return (_teacherServiceBL.DeleteTeacher(teacherId));
        }

        [HttpGet("getAllTeachers")]
        public Task<List<TeacherBL>> GetAllTeachers()
        {
            return (_teacherServiceBL.GetAllTeachers());
        }
        [HttpGet("getTeacherById/{teacherId}")]
        public Task<TeacherBL?> GetTeacherById(int teacherId)
        {
            return (_teacherServiceBL.GetTeacherById(teacherId));
        }
        [HttpGet("getTeacherByName/{firstName}/{lastName}")]
        public Task<TeacherBL?> GetTeacherByName(string firstName, string lastName)
        {
            return (_teacherServiceBL.GetTeacherByName(firstName, lastName));
        }
        [HttpGet("getTeachersByAvailability/{date}/{startTime}/{endTime}")]
        public Task<List<TeacherBL>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return (_teacherServiceBL.GetTeachersByAvailability(date, startTime, endTime));
        }

        [HttpGet("getTeachersBySubject/{subjectId}")]
        public Task<List<TeacherBL>> GetTeachersBySubject(int subjectId)
        {
            return (_teacherServiceBL.GetTeachersBySubject(subjectId));
        }
        [HttpGet("getLessonsByTeacherId/{teacherId}")]
        public Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId)
        {
            return _teacherServiceBL.GetLessonsByTeacherId(teacherId);
        }

        [HttpGet("getTeacherAvailabilities/{teacherId}")]
        public Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilities(int teacherId)
        {
            return _teacherServiceBL.GetTeacherAvailabilities(teacherId);
        }
        [HttpGet("getTeachersSubjects/{teacherId}")]
        public Task<List<TeachersToSubjectBL>> GetTeachersSubjects(int teacherId)
        {
            return _teacherServiceBL.GetTeachersSubjects(teacherId);
        }
        [HttpPatch("updateTeacher/{id}")]
        public Task<TeacherBL> UpdateTeacher(int id, [FromBody] JsonPatchDocument<TeacherBL> patchDoc)
        {
            return _teacherServiceBL.UpdateTeacher(id, patchDoc);
        }


    }
}
