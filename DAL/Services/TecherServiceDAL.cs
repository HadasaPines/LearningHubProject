using DAL.Api;
using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<Teacher> UpdateTeacher(int Id, JsonPatchDocument<Teacher> patchDoc)
        {
            var teacher = await dbContext.Teachers
                .FirstOrDefaultAsync(t => t.TeacherId ==Id);

            patchDoc.ApplyTo(teacher);

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
        public async Task<List<Teacher>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            return await dbContext.Teachers
                .Include(t => t.TeacherAvailabilities)
                .Where(t => t.TeacherAvailabilities.
                Any(ta =>
                   ((ta.StartTime.CompareTo(startTime) == -1) &&
                    (ta.EndTime.CompareTo(endTime) == 1)))).ToListAsync();
                
        }


    }
}
