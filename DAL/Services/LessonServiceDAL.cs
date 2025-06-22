

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
    public class LessonServiceDAL : ILessonServiceDAL
    {
        private readonly LearningHubDbContext dbContext;
        public LessonServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await dbContext.Lessons.ToListAsync();
        }
        //public async Task<List<Lesson>> GetLessonsByTeacherName(string firstName,string lastName)
        //{
        //    var teacher = await dbContext.Teachers
        //        .FirstOrDefaultAsync(t => t.TeacherNavigation.FirstName==firstName&& t.TeacherNavigation.LastName==lastName);
        //    return await dbContext.Lessons
        //        .Where(l => l.TeacherId == teacher.TeacherId)
        //        .ToListAsync();

        //}
        //public async Task<List<Lesson>> GetLessonsBySubjectName(string subjectName)
        //{
        //    var subject = await dbContext.Subjects
        //        .FirstOrDefaultAsync(s => s.Name.Equals(subjectName));
        //    if (subject == null)
        //    {
        //        throw new Exception($"Subject with Name {subjectName} not found.");
        //    }
        //    return await dbContext.Lessons
        //        .Where(l => l.SubjectId == subject.SubjectId)
        //        .ToListAsync();
        //}
        public async Task<List<Lesson>> GetLessonsByDate(DateOnly date)
        {
            return await dbContext.Lessons
                .Where(l => l.LessonDate == date)
                .ToListAsync();
        }
        public async Task<Lesson> GetLessonById(int lessonId)
        {
            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            return lesson;
        }
        public async Task AddLesson(Lesson lesson)
        {
            await dbContext.Lessons.AddAsync(lesson);
            await dbContext.SaveChangesAsync();
        }
        public async Task DeleteLessonById(int lessonId)
        {
            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);

            dbContext.Lessons.Remove(lesson);
            await dbContext.SaveChangesAsync();
        }
        public async Task<Lesson> UpdateLesson(Lesson lesson)
        {
            var lessonToUpdate = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lesson.LessonId);

            lessonToUpdate = lesson;
            dbContext.Lessons.Update(lessonToUpdate);
            await dbContext.SaveChangesAsync();
            return lessonToUpdate;
        }
        public async Task<List<Lesson>> GetLessonsByTeacherId(int teacherId)
        {
            return await dbContext.Lessons
                .Where(l => l.TeacherId == teacherId)
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsBySubjectId(int subjectId)
        {
            return await dbContext.Lessons
                .Where(l => l.SubjectId == subjectId)
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByStatus(string status)
        {
            return await dbContext.Lessons
                .Where(l => l.Status == status)
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await dbContext.Lessons
                .Where(l => l.LessonDate >= startDate && l.LessonDate <= endDate)
                .ToListAsync();
        }
        //public async Task<List<Lesson>> GetLessonsByTeacherAndDate(int teacherId, DateOnly date)
        //{
        //    return await dbContext.Lessons
        //        .Where(l => l.TeacherId == teacherId && l.LessonDate == date)
        //        .ToListAsync();
        //}
        public async Task<List<Lesson>> GetLessonsByTeacherAndSubject(int teacherId, int subjectId)
        {
            return await dbContext.Lessons
                .Where(l => l.TeacherId == teacherId && l.SubjectId == subjectId)
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByTimeRange(TimeOnly startTime, TimeOnly endTime)
        {
            return await dbContext.Lessons
                .Where(l => l.StartTime >= startTime && l.EndTime <= endTime)
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByAgeRange(int? minAge, int? maxAge)
        {
            return await dbContext.Lessons
                .Where(l => (l.MinAge == null || l.MinAge <= minAge) && (l.MaxAge == null || l.MaxAge >= maxAge))
                .ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByGender(string gender)
        {
            return await dbContext.Lessons
                .Where(l => l.Gender == gender)
                .ToListAsync();
        }











    }
}



