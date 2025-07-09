using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Services
{
    public interface ISubscriptionServiceBL
    {
        Task AddSubscription(SubscriptionBL subscription);
        Task DeleteSubscriptionAsync(int id);
        Task<List<SubscriptionBL>> GetAllSubscriptions();
        Task<SubscriptionBL> GetSubscriptionById(int id);
        Task UpdateSubscriptionAsync(int id, JsonPatchDocument<SubscriptionBL> patchDoc);
    }
}