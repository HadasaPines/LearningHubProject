
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
        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
        public async Task<List<Lesson>> GetAllLessons()
        {
            return await dbContext.Lessons.ToListAsync();
        }
        public async Task<List<Lesson>> GetAllLessonsIncludeDetails()
        {
            return await dbContext.Lessons
                .Include(l => l.Teacher)
                .Include(l => l.Registration)
                .ThenInclude(l => l.Student)
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
            if(lesson.Status== "Booked")
            {
                lesson.Status = "Cancelled"; 

                dbContext.Entry(lesson).State = EntityState.Modified; 
                await dbContext.SaveChangesAsync();
                return; 
            }

            dbContext.Lessons.Remove(lesson);
            await dbContext.SaveChangesAsync();
        }
        
        public async Task<Lesson> UpdateLesson(Lesson lesson)
        {
            var lessonToUpdate = await dbContext.Lessons
                .FirstOrDefaultAsync(l => l.LessonId == lesson.LessonId);

           
            dbContext.Entry(lessonToUpdate).CurrentValues.SetValues(lesson);
            await dbContext.SaveChangesAsync();
            return lessonToUpdate;

        }

        public async Task<List<Lesson>> GetLessonsByTeacherId(int teacherId)
        {
            return await dbContext.Lessons
                .Where(l => l.TeacherId == teacherId)
                .ToListAsync();

        }
  














    }
}



