using DAL.Api;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class RegistrationServiceDAL : IRegistrationServiceDAL
    {
        private readonly LearningHubDbContext dbContext;
        public RegistrationServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Registration>> GetAllRegistrations()
        {
            return await dbContext.Registrations
                .Include(r => r.Student)
                .Include(r => r.Lesson)
                .ToListAsync();
        }
        public async Task<Registration> GetRegistrationById(int registrationId)
        {
            return await dbContext.Registrations
                .Include(r => r.Student)
                .Include(r => r.Lesson)
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);
        }
        public async Task<List<Registration>> GetRegistrationsToLesson(Lesson lesson)
        {
            var registrations = await dbContext.Registrations
                .Include(r => r.Lesson)
                .Where(r => r.Lesson.LessonId == lesson.LessonId).ToListAsync();
            return registrations;
        }
        public async Task<List<Registration>> GetRegistrationsToStudent(Student student)
        {


            var registrations = await dbContext.Registrations
                .Include(r => r.Student)
                .Where(r => r.Student.StudentId == student.StudentId).ToListAsync();
            return registrations;
        }
        public async Task AddRegistration(Registration registration)
        {
            //var lesson = await dbContext.Lessons
            //    .FirstOrDefaultAsync(l => l.LessonId == registration.LessonId);
            //if (lesson == null)
            //{
            //    throw new Exception($"Lesson with ID {registration.LessonId} not found.");
            //}
            //lesson.Status = "Not available";
            //dbContext.Lessons.Update(lesson);
            dbContext.Registrations.Add(registration);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteRegistration(int registrationId)
        {
            var registration = await dbContext.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);
            //if (registration == null)
            //{
            //    throw new Exception($"Registration with ID {registrationId} not found.");
            //}

            //var lesson = await dbContext.Lessons
            //    .FirstOrDefaultAsync(l => l.LessonId == registration.LessonId);
            //if (lesson != null)
            //{
            //    lesson.Status = "Available";
            //    dbContext.Lessons.Update(lesson);
            //}

            dbContext.Registrations.Remove(registration);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateRegistration(Registration registration)
        {
            var existingRegistration = await dbContext.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationId == registration.RegistrationId);
            existingRegistration = registration;
            dbContext.Registrations.Update(existingRegistration);
            await dbContext.SaveChangesAsync();

        }
    }
}
