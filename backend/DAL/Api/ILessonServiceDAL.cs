using DAL.Models;

namespace DAL.Api
{
    public interface ILessonServiceDAL
    {
        Task AddLesson(Lesson lesson);
        Task DeleteLessonById(int lessonId);
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(int lessonId);
        Task<List<Lesson>> GetAllLessonsIncludeDetails();
        Task<Lesson> UpdateLesson(Lesson lesson);
        Task SaveChangesAsync();
    }
}