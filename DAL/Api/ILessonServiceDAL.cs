using DAL.Models;

namespace DAL.Api
{
    public interface ILessonServiceDAL
    {
        Task AddLesson(Lesson lesson);
        Task DeleteLessonById(int lessonId);
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(int lessonId);
        Task<List<Lesson>> GetLessonsByDate(DateOnly date);
        Task<List<Lesson>> GetLessonsBySubjectName(string subjectName);
        Task<List<Lesson>> GetLessonsByTeacherName(string teacherName);
        Task UpdateLessonDate(int lessonId, DateOnly newDate);
        Task UpdateLessonTime(int lessonId, TimeOnly newStartTime, TimeOnly newEndTime);
    }
}