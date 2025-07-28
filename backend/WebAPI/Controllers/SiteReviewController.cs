using BL.Api;
using BL.Models;
using BL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteReviewController : ControllerBase
    {

        ISiteReviewServiceBL _siteReviewServiceBL;
        public SiteReviewController(ISiteReviewServiceBL siteReviewServiceBL)
        {
            _siteReviewServiceBL=siteReviewServiceBL;


        }
        [HttpGet("getAllSiteReviews")]
        public async Task<IActionResult> GetAllSiteReviews()
        {
            var siteReviews = await _siteReviewServiceBL.GetAllSiteReviews();
            if (siteReviews == null || !siteReviews.Any())
            {
                return NotFound("No siteReviews found.");
            }
            return Ok(siteReviews);
        }

        [HttpPost("addSiteReview")]
        public async Task<IActionResult> AddSiteReview([FromBody] SiteReviewBL siteReviewBL)
        {
            await _siteReviewServiceBL.AddSiteReview(siteReviewBL);
            return Ok("siteReview added successfully");
        }

    }
}
