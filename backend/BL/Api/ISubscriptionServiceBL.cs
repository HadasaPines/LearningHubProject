using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Services
{
    public interface ISubscriptionServiceBL
    {
        Task AddSubscription(Subscription subscription);
        Task DeleteSubscriptionAsync(int id);
        Task<List<Subscription>> GetAllSubscriptions();
        Task<Subscription> GetSubscriptionById(int id);
        Task UpdateSubscriptionAsync(int id, JsonPatchDocument<SubscriptionBL> patchDoc);
    }
}