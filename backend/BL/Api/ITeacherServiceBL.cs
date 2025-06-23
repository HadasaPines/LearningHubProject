using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ITeacherServiceBL
    {
        Task AddAvailabilityToTeacher(int teacherId, TeacherAvailabilityBL availabilityBL);
        Task AddLessonToTeacher(int teacherId, LessonBL lessonBL);
        Task AddSubjectToTeacher(int teacherId, TeachersToSubjectBL teachersToSubjectBL);
        Task AddTeacher(TeacherBL TeacherBL);
        Task DeleteTeacher(int teacherId);
        Task<List<TeacherBL>> GetAllTeachers();
        Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId);
        Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilities(int teacherId);
        Task<TeacherBL?> GetTeacherById(int teacherId);
        Task<TeacherBL?> GetTeacherByName(string firstName, string lastName);
        Task<List<TeacherBL>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<List<TeacherBL>> GetTeachersBySubject(int subjectId);
        Task<List<TeachersToSubjectBL>> GetTeachersSubjects(int teacherId);
        Task<TeacherBL> UpdateTeacher(int id, JsonPatchDocument<TeacherBL> patchDoc);
    }
}