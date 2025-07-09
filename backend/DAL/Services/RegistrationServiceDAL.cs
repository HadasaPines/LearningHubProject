using DAL.Api;

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
        public async Task<Registration> GetRegistrationByLessonId(int lessonId)
        {
            return await dbContext.Registrations
                .Include(r => r.Student)
                .Include(r=>r.Student.StudentNavigation)
                .Include(r => r.Lesson)
                .FirstOrDefaultAsync(r => r.Lesson.LessonId == lessonId);


        }
        public async Task<Lesson> GetLessonByRegistrationId(int id)
        {
            var registration = await dbContext.Registrations
                .Include(r => r.Lesson)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);

            return registration.Lesson;

        }
        public async Task<Student> GetStudentByRegistrationId(int id)
        {
            var registration = await dbContext.Registrations
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.RegistrationId == id);

            return registration.Student;
        }
        public async Task<List<Registration>> GetRegistrationsToStudent(int studentId)
        {
            var registrations = await dbContext.Registrations
                .Include(r => r.Student)
                .Include(r=>r.Lesson)
                .Where(r => r.Student.StudentId == studentId).ToListAsync();
            return registrations;
        }
        public async Task AddRegistration(Registration registration, Lesson lesson)
        {
            try
            {
                lesson.Status = "booked";
                dbContext.Registrations.Add(registration);
                dbContext.Lessons.Update(lesson);


                await dbContext.SaveChangesAsync();
                Console.WriteLine("Changes Saved!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error during SaveChangesAsync: " + ex.Message);
            }
        }


        public async Task DeleteRegistration(int registrationId, Lesson lesson)
        {
            if (lesson.LessonDate >= DateOnly.FromDateTime(DateTime.Today))
            {
                lesson.Status = "Available";
               
            }
            else
            {
                lesson.Status = "passed";

            }
            dbContext.Lessons.Update(lesson);
            var registration= await dbContext.Registrations
                .FirstOrDefaultAsync(r => r.RegistrationId == registrationId);

            dbContext.Registrations.Remove(registration);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteRegistrationByLessonId(int lessonId)
        {
            var lesson =await dbContext.Lessons.FirstOrDefaultAsync(l => l.LessonId == lessonId);
            var registration = await dbContext.Registrations
                .Where(r => r.Lesson.LessonId == lessonId)
                .Select(r => r.RegistrationId)
                .FirstOrDefaultAsync();
            if (registration == 0) return;
           await DeleteRegistration(registration,lesson);
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
