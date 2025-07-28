using DAL.Models;

namespace DAL.Api
{
    public interface ISiteReviewServiceDAL
    {
        Task AddSiteReview(SiteReview siteReview);
        Task<List<SiteReview>> GetAllSiteReviews();
    }
}