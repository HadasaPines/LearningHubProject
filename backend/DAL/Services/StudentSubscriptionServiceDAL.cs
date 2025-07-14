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
    public class StudentSubscriptionServiceDAL : IStudentSubscriptionServiceDAL
    {
        private readonly LearningHubDbContext _context;
        public StudentSubscriptionServiceDAL(LearningHubDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<StudentSubscription>> GetAllStudentSubscriptions()
        {
            return await _context.StudentSubscriptions.ToListAsync();
        }
        public async Task<StudentSubscription> GetStudentSubscriptionById(int id)
        {
            return await _context.StudentSubscriptions
                .FirstOrDefaultAsync(ss => ss.StudentSubscriptionId == id);
        }
        public async Task AddStudentSubscription(StudentSubscription studentSubscription)
        {
            await _context.StudentSubscriptions.AddAsync(studentSubscription);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentSubscription(int id)
        {
            var studentSubscription = await _context.StudentSubscriptions
                .FirstOrDefaultAsync(ss => ss.StudentSubscriptionId == id);
            _context.StudentSubscriptions.Remove(studentSubscription);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateLessonsUsedForActiveStudentSubscription(StudentSubscription studentSubscription)
        {
            var existingSubscription = await _context.StudentSubscriptions
                .FirstOrDefaultAsync(ss => ss.StudentSubscriptionId == studentSubscription.StudentSubscriptionId)
                ;
            studentSubscription.LessonsUsed++;
            if (studentSubscription.LessonsUsed == studentSubscription.Subscription.LessonCount)
            {
                studentSubscription.IsActive = false;
            }
            _context.Entry(existingSubscription).CurrentValues.SetValues(studentSubscription);
            await _context.SaveChangesAsync();
        }
        public async Task<List<StudentSubscription>> GetStudentSubscriptionsByStudentId(int studentId)
        {
            return await _context.StudentSubscriptions
                .Where(ss => ss.StudentId == studentId)
                .Include(ss => ss.Student)
                .ToListAsync();


        }
        public async Task<StudentSubscription> GetActiveStudentSubscriptionsByStudentId(int studentId)
        {
            return await _context.StudentSubscriptions
                .Include(ss => ss.Subscription)
                .FirstOrDefaultAsync(ss => ss.StudentId == studentId && ss.IsActive);
                


        }

    }
}

