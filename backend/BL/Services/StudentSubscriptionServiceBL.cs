using AutoMapper;
using BL.Api;
using BL.Exceptions.StudentSubscriptionExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class StudentSubscriptionServiceBL : IStudentSubscriptionServiceBL
    {
        private readonly IStudentSubscriptionServiceDAL _studentSubscriptionServiceDAL;
        private readonly IMapper _mapper;

        public StudentSubscriptionServiceBL(IStudentSubscriptionServiceDAL studentSubscriptionServiceDAL, IMapper mapper)
        {
            _studentSubscriptionServiceDAL = studentSubscriptionServiceDAL;
            _mapper = mapper;
        }

        public async Task AddStudentSubscription(StudentSubscriptionBL studentSubscriptionBL)
        {
            if (studentSubscriptionBL == null)
                throw new ArgumentNullException(nameof(studentSubscriptionBL), "Student subscription cannot be null");
            var studentSubscriptions=await _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentSubscriptionBL.StudentId);
            if(studentSubscriptions.Any(sub => sub.IsActive))
            throw new ActiveSubscriptionAlreadyExistException("Student already has an active subscription");
            var studentSubscription = _mapper.Map<StudentSubscription>(studentSubscriptionBL);
            await _studentSubscriptionServiceDAL.AddStudentSubscription(studentSubscription);
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
            if(studentSubscription==null)
            throw new StudentSubscriptionNotFoundException($"No active student subscription for student with id '{studentId}' not found");
            return _mapper.Map<StudentSubscriptionBL>(studentSubscription);

        }

    }
}
