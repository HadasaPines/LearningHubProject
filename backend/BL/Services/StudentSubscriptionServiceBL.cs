using AutoMapper;
using BL.Api;
using BL.Exceptions.StudentSubscriptionExceptoins;
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
        public async Task UpdateLessonsUsed(int id)
        {
            var studentSubscription = await _studentSubscriptionServiceDAL.GetStudentSubscriptionById(id);
            if (studentSubscription == null)
                throw new StudentSubscriptionNotFoundException($"Student subscription with ID '{id}' not found");
            var studentSubscriptionBL = _mapper.Map<StudentSubscriptionBL>(studentSubscription);
            if (!studentSubscriptionBL.IsActive)
                throw new UnActiveStudentSubscriptionException("Student subscription is not active, cannot update lessons used");

            studentSubscriptionBL.LessonsUsed++;
            var updatedStudentSubscription = _mapper.Map<StudentSubscription>(studentSubscriptionBL);

            await _studentSubscriptionServiceDAL.UpdateLessonsUsed(updatedStudentSubscription);

        }

        public async Task<List<StudentSubscriptionBL>> GetStudentSubscriptionsByStudentId(int studentId)
        {
            var studentSubscriptions = await _studentSubscriptionServiceDAL.GetStudentSubscriptionsByStudentId(studentId);
            if (studentSubscriptions == null || !studentSubscriptions.Any())
                throw new StudentSubscriptionNotFoundException($"No student subscriptions found for student ID '{studentId}'");

            return _mapper.Map<List<StudentSubscriptionBL>>(studentSubscriptions);
        }




    }
}
