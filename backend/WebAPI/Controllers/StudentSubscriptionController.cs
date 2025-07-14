using BL.Api;
using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentSubscriptionController : ControllerBase
    {
        private readonly IStudentSubscriptionServiceBL _studentSubscriptionServiceBL;

        public StudentSubscriptionController(IStudentSubscriptionServiceBL studentSubscriptionServiceBL)
        {
            _studentSubscriptionServiceBL = studentSubscriptionServiceBL;
        }

        [HttpGet("getAllStudentSubscriptions")]
        public async Task<IActionResult> GetAllStudentSubscriptions()
        {
            var studentSubscriptions = await _studentSubscriptionServiceBL.GetAllStudentSubscriptions();
            return Ok(studentSubscriptions);
        }

        [HttpGet("getStudentSubscriptionById/{id}")]
        public async Task<IActionResult> GetStudentSubscriptionById(int id)
        {
            var studentSubscriptions = await _studentSubscriptionServiceBL.GetStudentSubscriptionById(id);
            return Ok(studentSubscriptions);
        }

        [HttpGet("getStudentSubscriptionsByStudentId/{id}")]
        public async Task<IActionResult> GetStudentSubscriptionsByStudentId(int id)
        {
            var studentSubscriptions = await _studentSubscriptionServiceBL.GetStudentSubscriptionsByStudentId(id);
            return Ok(studentSubscriptions);
        }

        [HttpPost("addStudentSubscription")]
        public async Task<IActionResult> AddStudentSubscription([FromBody] StudentSubscriptionBL studentSubscriptionBL)
        {


            await _studentSubscriptionServiceBL.AddStudentSubscription(studentSubscriptionBL);
            return Ok("Student subscription added successfully.");
        }
        [HttpPost("CheckAddStudentSubscription")]
        public async Task<IActionResult> CheckAddStudentSubscription([FromBody]StudentSubscriptionBL studentSubscriptionBL)
        {
            var studentSubscription = await _studentSubscriptionServiceBL.CheckAddStudentSubscription(studentSubscriptionBL); 
            return Ok(studentSubscription);

        }
        [HttpDelete("deleteStudentSubscription/{id}")]
        public async Task<IActionResult> DeleteStudentSubscription(int id)
        {
            await _studentSubscriptionServiceBL.DeleteStudentSubscription(id);
            return Ok("Student subscription deleted successfully.");
        }
        [HttpPut("updateLessonsUsedForActiveStudentSubscription/{studentId}")]
        public async Task<IActionResult> UpdateLessonsUsedForActiveStudentSubscription(int studentId)
        {
            await _studentSubscriptionServiceBL.UpdateLessonsUsedForActiveStudentSubscription(studentId);
            return Ok("Lessons used updated successfully.");
        }
        [HttpGet("getActiveStudentSubscriptionsByStudentId/{studentId}")]
        public async Task<IActionResult> GetActiveStudentSubscriptionsByStudentId(int studentId)
        {
             var studentSubscription=await _studentSubscriptionServiceBL.GetActiveStudentSubscriptionsByStudentId(studentId);
            return Ok(studentSubscription);
        }



    }
}
