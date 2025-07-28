using AutoMapper;
using BL.Api;
using BL.Models;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class SiteReviewServiceBL : ISiteReviewServiceBL
    {
        private readonly ISiteReviewServiceDAL _siteReviewServiceDAL;
        private readonly IMapper _mapper;

        public SiteReviewServiceBL(ISiteReviewServiceDAL siteReviewServiceDAL, IMapper mapper)
        {
            _siteReviewServiceDAL = siteReviewServiceDAL;

            _mapper = mapper;
        }

        public async Task<List<SiteReviewBL>> GetAllSiteReviews()
        {
            return _mapper.Map<List<SiteReviewBL>>(await _siteReviewServiceDAL.GetAllSiteReviews());

        }

        public async Task AddSiteReview(SiteReviewBL SiteReviewBL)
        {
            if (SiteReviewBL == null)
            {
                throw new ArgumentNullException(nameof(SiteReviewBL), "SiteReview cannot be null");
            }
            var siteReview = _mapper.Map<SiteReview>(SiteReviewBL);
            await _siteReviewServiceDAL.AddSiteReview(siteReview);
        }
    }
}
