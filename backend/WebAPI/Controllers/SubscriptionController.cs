using BL.Models;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionServiceBL _subscriptionServiceBL;
        public SubscriptionController(ISubscriptionServiceBL subscriptionServiceBL)
        {
            _subscriptionServiceBL = subscriptionServiceBL;
        }
        [HttpGet("getAllSubscriptions")]
        public async Task<IActionResult> GetAllSubscriptions() {

        var subscriptions = await _subscriptionServiceBL.GetAllSubscriptions();
      return Ok(subscriptions);
            
 
        }
        [HttpGet("getSubscriptionById/{id}")]
        public async Task<IActionResult> GetSubscriptionById(int id)
        {
            var subscription = await _subscriptionServiceBL.GetSubscriptionById(id);
            if (subscription == null)
                return NotFound($"Subscription with ID '{id}' not found");
            return Ok(subscription);
        }
        [HttpPost("addSubscription")]
        public async Task<IActionResult> AddSubscription([FromBody] SubscriptionBL subscriptionBL)
        {
          
            await _subscriptionServiceBL.AddSubscription(subscriptionBL);
            return Ok("subscription added successfully.");
        }

        [HttpPut("updateSubscription/{id}")]
        public async Task<IActionResult> UpdateSubscription(int id, [FromBody] JsonPatchDocument<SubscriptionBL> patchDoc)
        {
                await _subscriptionServiceBL.UpdateSubscriptionAsync(id, patchDoc);
                return Ok("Subscription updated successfully.");
           
        }
        [HttpDelete("deleteSubscription/{id}")]
        public async Task<IActionResult> DeleteSubscription(int id)
        {
            await _subscriptionServiceBL.DeleteSubscriptionAsync(id);
            return Ok("Subscription deleted successfully.");
        }

    }
}
