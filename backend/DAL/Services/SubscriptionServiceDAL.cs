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
    public class SubscriptionServiceDAL : ISubscriptionServiceDAL
    {
        private readonly LearningHubDbContext _dbContext;
        public SubscriptionServiceDAL(LearningHubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddSubscription(Subscription subscription)
        {
            await _dbContext.Subscriptions.AddAsync(subscription);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await _dbContext.Subscriptions.ToListAsync();
        }
        public async Task<Subscription> GetSubscriptionById(int id)
        {
            return await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
        }

        public async Task UpdateSubscription(Subscription subscription)
        {
            var existingSubscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.SubscriptionId == subscription.SubscriptionId);
            if (existingSubscription != null)
            {
                _dbContext.Entry(existingSubscription).CurrentValues.SetValues(subscription);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task DeleteSubscription(int id)
        {
            var subscription = await _dbContext.Subscriptions
                .FirstOrDefaultAsync(s => s.SubscriptionId == id);
            if (subscription != null)
            {
                _dbContext.Subscriptions.Remove(subscription);
                await _dbContext.SaveChangesAsync();
            }
        }


    }
}
