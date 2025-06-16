using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ITeacherServiceBL
    {
        Task AddAvailabilityToTeacher(int teacherId, TeacherAvailability availability);
        Task AddLessonToTeacher(int teacherId, Lesson lesson);
        Task AddSubjectToTeacher(int teacherId, TeachersToSubject teachersToSubject);
        Task AddTeacher(TecherBL techerBL);
        Task DeleteTeacher(int teacherId);
        Task<List<TecherBL>> GetAllTeachers();
        Task<List<Lesson>> GetLessonsByTeacherId(int teacherId);
        Task<List<TeacherAvailability>> GetTeacherAvailabilities(int teacherId);
        Task<TecherBL?> GetTeacherBId(int teacherId);
        Task<TecherBL?> GetTeacherByName(string firstName, string lastName);
        Task<List<TecherBL>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime);
        Task<List<TecherBL>> GetTeachersBySubject(int subjectId);
        Task<List<TeachersToSubject>> GetTeachersSubjects(int teacherId);
        Task<TecherBL> UpdateTeacher(int id, JsonPatchDocument<Teacher> patchDoc);
    }
}