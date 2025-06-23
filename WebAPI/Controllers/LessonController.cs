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
    public class LessonController : ControllerBase
    {
        ILessonServiceBL _lessonServiceBL;
        public LessonController(ILessonServiceBL lessonServiceBL)
        {
            _lessonServiceBL = lessonServiceBL;

        }
        [HttpGet("getAllLessons")]
        public async Task<IActionResult> GetAllLessons()
        {
            var lessons = await _lessonServiceBL.GetAllLessons();
            if(lessons == null)
            {
                return NotFound("No Lessons found");

            }
            return Ok(lessons);
        }
        [HttpGet("getLessonById")]

        public async Task<IActionResult> GetLessonById([FromQuery] int id)
        {

            var lesson = await _lessonServiceBL.GetLessonById(id);
            return Ok(lesson);


        }
        [HttpPost("addLesson")]
        public async Task<IActionResult> AddLesson([FromBody] LessonBL lessonBL)
        {

            await _lessonServiceBL.AddLesson(lessonBL);
            return Ok("Lesson added successfully");
            //return CreatedAtAction(nameof(GetLessonById), new { id = lessonBL. }, lessonBL);


        }
        [HttpPatch("updateLesson {id:int}")]
        public async Task<IActionResult> UpdateLesson(int id, [FromBody] JsonPatchDocument<LessonBL> patchDoc)
        {
            await _lessonServiceBL.UpdateLesson(id, patchDoc);
            return Ok("Lesson updated successfully");
        }
        [HttpDelete("deleteLesson{id:int}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {

            await _lessonServiceBL.DeleteLesson(id);
            return Ok("Lesson deleted successfully");

        }
        [HttpGet("details")]
        public async Task<IActionResult> GetLessonsByDetails([FromQuery] LessonFilterDto filter)
        {

            var lessons = await _lessonServiceBL.GetLessonsByDetails(filter);
            if (lessons == null)
            {
                return NotFound("No Lessons found");

            }
            return Ok(lessons);


        }
    }
}
