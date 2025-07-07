using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ILessonServiceBL
    {
        Task AddLesson(LessonBL lessonBL);
        Task DeleteLesson(int id);
        Task<List<LessonBL>> GetAllLessons();
        Task<LessonBL> GetLessonById(int lessonId);
        Task<List<LessonBL>> GetLessonsByDetails(LessonFilterDto filter);
        Task UpdateLesson(int id, JsonPatchDocument<LessonBL> patchDoc);
        Task GenerateLessonsAsync(DateOnly startDate, DateOnly endDate);
        Task UpdatePastLessonsAsync();
        Task<UserBL> GetStudentToLesson(int lessonId);

        Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId);
    }
}