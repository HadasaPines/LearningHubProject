using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    internal class TecherServiceDAL
    {
        private readonly MyDbContext dbContext;
        public TecherServiceDAL(MyDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .ToListAsync();
        }
        public async Task<Teacher?> GetTeacherByNameAsync(int teacherId)
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId);
        }
        public async Task<Teacher?> GetTeacherByUserIdAsync(int userId)
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherNavigation)
                .Include(t => t.Lessons)
                .FirstOrDefaultAsync(t => t.TeacherNavigation.UserId == userId);
        }
        public async Task<Teacher> AddTeacherAsync(Teacher teacher)
        {
            dbContext.Teachers.Add(teacher);
            await dbContext.SaveChangesAsync();
            return teacher;            
        }
        public async Task<Teacher> UpdateTeacherBioAsync(string bio , string name )
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.FullName == name);
            if(user == null)
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
        public async Task<bool> DeleteTeacherAsync(int teacherId)
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
        public async Task<List<Teacher>> GetTeachersBySubjectAsync(int subjectId)
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
