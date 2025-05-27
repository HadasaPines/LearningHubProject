

using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class LessonServiceDAL
    {
        private readonly MyDbContext dbContext;
        public LessonServiceDAL(MyDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await dbContext.Lessons.ToListAsync();
        }
        public async Task<List<Lesson>> GetLessonsByTeacherName(string teacherName)
        {
            var teacher = await dbContext.Teachers
                .FirstOrDefaultAsync(t => t.TeacherNavigation.FullName.Equals(teacherName));
            if (teacher == null)
            {
                throw new Exception($"Teacher with Name {teacherName} not found.");
            }
            return await dbContext.Lessons
                .Where(l => l.TeacherId == teacher.TeacherId)
                .ToListAsync();

        }
        public async Task<List<Lesson>> GetLessonsBySubjectName(string subjectName)
        {
            var subject = await dbContext.Subjects
                .FirstOrDefaultAsync(s => s.Name.Equals(subjectName));
            if (subject == null)
            {
                throw new Exception($"Subject with Name {subjectName} not found.");
            }
            return await dbContext.Lessons
                .Where(l => l.SubjectId == subject.SubjectId)
                .ToListAsync();
        }
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
            if (lesson == null)
            {
                throw new Exception($"Lesson with ID {lessonId} not found.");
            }
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
            if (lesson == null)
            {
                throw new Exception($"Lesson with ID {lessonId} not found.");
            }
            dbContext.Lessons.Remove(lesson);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateLessonDate(int lessonId, DateOnly newDate)
        {
            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
            if (lesson == null)
            {
                throw new Exception($"Lesson with ID {lessonId} not found.");
            }
            lesson.LessonDate = newDate;
            dbContext.Lessons.Update(lesson);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateLessonTime(int lessonId, TimeOnly newStartTime, TimeOnly newEndTime)
        {
            var lesson = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lessonId);
            if (lesson == null)
            {
                throw new Exception($"Lesson with ID {lessonId} not found.");
            }
            lesson.StartTime = newStartTime;
            lesson.EndTime = newEndTime;
            dbContext.Lessons.Update(lesson);
            await dbContext.SaveChangesAsync();
        }







    }
}



