using BL.Models;

namespace BL.Api
{
    public interface ISiteReviewServiceBL
    {
        Task AddSiteReview(SiteReviewBL SiteReviewBL);
        Task<List<SiteReviewBL>> GetAllSiteReviews();
    }
}