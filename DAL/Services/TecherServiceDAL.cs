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
    public class TecherServiceDAL : ITecherServiceDAL
    {
        private readonly LearningHubDbContext dbContext;
        public TecherServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .ToListAsync();
        }
        public async Task<Teacher?> GetTeacherByName(string firstName,string lastName)
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.TeacherNavigation.FirstName == firstName&&t.TeacherNavigation.LastName==lastName);
        }
        public async Task<Teacher?> GetTeacherByUserId(int teacherId)
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);
        }
        public async Task<Teacher> AddTeacher(Teacher teacher)
        {
            dbContext.Teachers.Add(teacher);
            await dbContext.SaveChangesAsync();
            return teacher;
        }
        public async Task<Teacher> UpdateTeacherBio(string bio, string firstName,string lastName)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.FirstName == firstName&&u.LastName==lastName);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            var teacher = await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .FirstOrDefaultAsync(t => t.TeacherNavigation.UserId == user.UserId);
            if (teacher == null)
            {
                throw new Exception("teacher not found");
            }
            teacher.Bio = bio;
            dbContext.Teachers.Update(teacher);
            await dbContext.SaveChangesAsync();
            return teacher;
        }
        public async Task<bool> DeleteTeacher(int teacherId)
        {
            var teacher = await dbContext.Teachers.FindAsync(teacherId);
            if (teacher == null)
            {
                return false;
            }

            dbContext.Teachers.Remove(teacher);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Teacher>> GetTeachersBySubject(int subjectId)
        {
            return await dbContext.Teachers
                .Include(t => t.TeachersToSubjects)
                .Where(t => t.TeachersToSubjects.Any(ts => ts.SubjectId == subjectId))
                .ToListAsync();
        }
        //public async Task<List<Teacher>> GetTeachersByAvailabilityAsync(DateTime date, TimeSpan startTime, TimeSpan endTime)
        //{
        //    return await dbContext.Teachers
        //        .Include(t => t.TeacherAvailabilities)
        //        .Where(t => t.TeacherAvailabilities.Any(ta =>
        //            ta.AvailabilityDate == date.Date &&
        //            ta.StartTime <= startTime &&
        //            ta.EndTime >= endTime))
        //        .ToListAsync();
        //}


    }
}
