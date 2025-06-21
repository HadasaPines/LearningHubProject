using DAL.Contexts;
using DAL.Models;
using DAL.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Api
{
    public class RegistrationServiceDAL : IRegistrationServiceDAL
    {
        private readonly LearningHubDbContext dbContext;
        public RegistrationServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Registration>> GetRegistrationsToStudentAsync(string firstName, string lastName)
        {
            var student = await dbContext.Users
                .FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);

            if (student == null)
            {
                throw new Exception($"User with Name {firstName}{lastName} not found.");
            }

            var registrations = await dbContext.Registrations
                .Include(r => r.Student)
                .Where(r => r.Student.StudentId == student.UserId).ToListAsync();
            if (registrations == null || !registrations.Any())
            {
                throw new Exception($"Student with Name {firstName}{lastName} doesnt have registrations.");
            }
            return registrations;
        }
        public async Task AddRegistrationAsync(Registration registration)
        {
            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == registration.LessonId);
            if (lesson == null)
            {
                throw new Exception($"Lesson with ID {registration.LessonId} not found.");
            }
            lesson.Status = "Not available";
            dbContext.Lessons.Update(lesson);
            dbContext.Registrations.Add(registration);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteRegistration(int registrationId)
        {
            var registration = await dbContext.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);
            if (registration == null)
            {
                throw new Exception($"Registration with ID {registrationId} not found.");
            }

            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == registration.LessonId);
            if (lesson != null)
            {
                lesson.Status = "Available";
                dbContext.Lessons.Update(lesson);
            }

            dbContext.Registrations.Remove(registration);
            await dbContext.SaveChangesAsync();
        }
    }
}
