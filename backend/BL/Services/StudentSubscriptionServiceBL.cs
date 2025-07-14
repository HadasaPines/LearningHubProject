using AutoMapper;
using BL.Api;
using BL.Exceptions.StudentSubscriptionExceptions;
using BL.Exceptions.SubscriptionExceptions;
using BL.Exceptions.UserExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class StudentSubscriptionServiceBL : IStudentSubscriptionServiceBL
    {
        private readonly IStudentSubscriptionServiceDAL _studentSubscriptionServiceDAL;
        private readonly ISubscriptionServiceDAL _subscriptionServiceDAL;
        private readonly IStudentServiceDAL _studentServiceDAL;

        private readonly IMapper _mapper;

        public StudentSubscriptionServiceBL(IStudentSubscriptionServiceDAL studentSubscriptionServiceDAL, ISubscriptionServiceDAL subscriptionServiceDAL, IStudentServiceDAL studentServiceDAL, IMapper mapper)
        {
            _studentSubscriptionServiceDAL = studentSubscriptionServiceDAL;
            _subscriptionServiceDAL = subscriptionServiceDAL;
            _studentServiceDAL = studentServiceDAL;
            _mapper = mapper;
        }

        public async Task AddStudentSubscription(StudentSubscriptionBL studentSubscriptionBL)
        {
            if (studentSubscriptionBL == null)
                throw new ArgumentNullException(nameof(studentSubscriptionBL), "Student subscription cannot be null");
            var subscription = await  _subscriptionServiceDAL.GetSubscriptionById(studentSubscriptionBL.SubscriptionId);
            if (subscription == null)
                throw new SubscriptionNotFoundException($"Subscription with ID '{studentSubscriptionBL.SubscriptionId}' not found");
            if (subscription.IsActive == false)
                throw new SubscriptionNotActiveException("This subscription is not active, you can not buy it.");
            var student= await _studentServiceDAL.GetStudentById(studentSubscriptionBL.StudentId);
            if (student == null)
                throw new UserNotFoundException($"Student with ID {studentSubscriptionBL.StudentId} not found.");
            var studentSubscriptions = await _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentSubscriptionBL.StudentId);
            if (studentSubscriptions.Any(sub => sub.IsActive))
                throw new ActiveSubscriptionAlreadyExistException("Student already has an active subscription");
            var studentSubscription = _mapper.Map<StudentSubscription>(studentSubscriptionBL);
            await _studentSubscriptionServiceDAL.AddStudentSubscription(studentSubscription);
        }
        public async Task<bool> CheckAddStudentSubscription(StudentSubscriptionBL studentSubscriptionBL)
        {
            if (studentSubscriptionBL == null)
                return false;
            var subscription = await _subscriptionServiceDAL.GetSubscriptionById(studentSubscriptionBL.SubscriptionId);
            if (subscription == null)
                return false;
            if (subscription.IsActive == false)
                return false;
            var student = await _studentServiceDAL.GetStudentById(studentSubscriptionBL.StudentId);
            if (student == null)
                return false;
            var studentSubscriptions = await _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentSubscriptionBL.StudentId);
            if (studentSubscriptions.Any(sub => sub.IsActive))
                return false;
            return true;
        }
        public async Task<List<StudentSubscriptionBL>> GetAllStudentSubscriptions()
        {
            var studentSubscriptions = await _studentSubscriptionServiceDAL.GetAllStudentSubscriptions();
            return _mapper.Map<List<StudentSubscriptionBL>>(studentSubscriptions);
        }
        public async Task<StudentSubscriptionBL> GetStudentSubscriptionById(int id)
        {
            var studentSubscription = await _studentSubscriptionServiceDAL.GetStudentSubscriptionById(id);
            if (studentSubscription == null)
                throw new StudentSubscriptionNotFoundException($"Student subscription with ID '{id}' not found");

            return _mapper.Map<StudentSubscriptionBL>(studentSubscription);
        }
        public async Task DeleteStudentSubscription(int id)
        {
            var studentSubscription = await _studentSubscriptionServiceDAL.GetStudentSubscriptionById(id);
            if (studentSubscription == null)
                throw new StudentSubscriptionNotFoundException($"Student subscription with ID '{id}' not found");

            await _studentSubscriptionServiceDAL.DeleteStudentSubscription(id);
        }
        public async Task UpdateLessonsUsedForActiveStudentSubscription(int studentId)
        {
            var studentSubscription = await _studentSubscriptionServiceDAL.GetActiveStudentSubscriptionsByStudentId(studentId);
            if (studentSubscription == null)
                throw new StudentSubscriptionNotFoundException($"No active student subscription for student with id '{studentId}' not found");
            //var studentSubscriptionBL = _mapper.Map<StudentSubscriptionBL>(studentSubscription);
            //studentSubscriptionBL.LessonsUsed++;
            //var updatedStudentSubscription = _mapper.Map<StudentSubscription>(studentSubscriptionBL);
            await _studentSubscriptionServiceDAL.UpdateLessonsUsedForActiveStudentSubscription(studentSubscription);

        }

        public async Task<List<StudentSubscriptionBL>> GetStudentSubscriptionsByStudentId(int studentId)
        {
            var studentSubscriptions = await _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentId);

            return _mapper.Map<List<StudentSubscriptionBL>>(studentSubscriptions);
        }


        public async Task<StudentSubscriptionBL> GetActiveStudentSubscriptionsByStudentId(int studentId)
        {
            var studentSubscription = _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentId);
            if (studentSubscription == null)
                throw new StudentSubscriptionNotFoundException($"No active student subscription for student with id '{studentId}' not found");
            return _mapper.Map<StudentSubscriptionBL>(studentSubscription);

        }

    }
}
