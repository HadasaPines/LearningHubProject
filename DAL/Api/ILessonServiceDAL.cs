using DAL.Models;

namespace DAL.Api
{
    public interface ILessonServiceDAL
    {
        Task AddLesson(Lesson lesson);
        Task DeleteLessonById(int lessonId);
        Task<List<Lesson>> GetAllLessons();
        Task<Lesson> GetLessonById(int lessonId);
        Task<List<Lesson>> GetLessonsByAgeRange(int? minAge, int? maxAge);
        Task<List<Lesson>> GetLessonsByDate(DateOnly date);
        Task<List<Lesson>> GetLessonsByDateRange(DateOnly startDate, DateOnly endDate);
        Task<List<Lesson>> GetLessonsByGender(string gender);
        Task<List<Lesson>> GetLessonsByStatus(string status);
        Task<List<Lesson>> GetLessonsBySubjectId(int subjectId);
        Task<List<Lesson>> GetLessonsByTeacherAndSubject(int teacherId, int subjectId);
        Task<List<Lesson>> GetLessonsByTeacherId(int teacherId);
        Task<List<Lesson>> GetLessonsByTimeRange(TimeOnly startTime, TimeOnly endTime);
        Task<Lesson> UpdateLesson(Lesson lesson);
    }
}