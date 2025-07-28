using DAL.Api;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class SiteReviewServiceDAL : ISiteReviewServiceDAL
    {
        private readonly LearningHubDbContext dbContext;

        public SiteReviewServiceDAL(LearningHubDbContext context)
        {
            dbContext = context;
        }
        public async Task<List<SiteReview>> GetAllSiteReviews()
        {
            return await dbContext.SiteReviews.ToListAsync();
        }

        public async Task AddSiteReview(SiteReview siteReview)
        {
            dbContext.SiteReviews.Add(siteReview);

            await dbContext.SaveChangesAsync();

        }


    }
}
