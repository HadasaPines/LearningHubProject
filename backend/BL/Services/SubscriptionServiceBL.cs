using AutoMapper;
using BL.Exceptions.SubjectExceptions;
using BL.Exceptions.SubscriptionExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class SubscriptionServiceBL : ISubscriptionServiceBL
    {
        private readonly ISubscriptionServiceDAL _subscriptionServiceDAL;
        private readonly IMapper _mapper;

        public SubscriptionServiceBL(ISubscriptionServiceDAL subscriptionServiceDAL, IMapper mapper)
        {
            _subscriptionServiceDAL = subscriptionServiceDAL;
            _mapper = mapper;
        }

        public async Task AddSubscription(Subscription subscription)
        {
            await _subscriptionServiceDAL.AddSubscription(subscription);
        }

        public async Task<List<Subscription>> GetAllSubscriptions()
        {
            return await _subscriptionServiceDAL.GetAllSubscriptions();
        }

        public async Task<Subscription> GetSubscriptionById(int id)
        {
            return await _subscriptionServiceDAL.GetSubscriptionById(id);
        }

        public async Task UpdateSubscriptionAsync(int id, JsonPatchDocument<SubscriptionBL> patchDoc)
        {
            if (patchDoc == null)
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            var subscription = await _subscriptionServiceDAL.GetSubscriptionById(id);
            if (subscription == null)
                throw new SubscriptionNotFoundException($"Subscription with ID '{id}' not found");
            var subscriptionBL = _mapper.Map<SubscriptionBL>(subscription);
            patchDoc.ApplyTo(subscriptionBL);
            var subjectToUpdate = _mapper.Map<Subscription>(subscriptionBL);
            await _subscriptionServiceDAL.UpdateSubscription(subjectToUpdate);
        }

        public async Task DeleteSubscriptionAsync(int id)
        {
            await _subscriptionServiceDAL.DeleteSubscription(id);
        }
    }
}
