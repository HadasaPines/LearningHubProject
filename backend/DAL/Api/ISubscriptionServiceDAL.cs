using DAL.Models;

namespace DAL.Api
{
    public interface ISubscriptionServiceDAL
    {
        Task AddSubscription(Subscription subscription);
        Task DeleteSubscription(int id);
        Task<List<Subscription>> GetAllSubscriptions();
        Task<Subscription> GetSubscriptionById(int id);
        Task UpdateSubscription(Subscription subscription);
    }
}