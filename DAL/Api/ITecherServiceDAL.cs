using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace DAL.Api
{
    public interface ITecherServiceDAL
    {
        Task<Teacher> AddTeacher(Teacher teacher);
        Task DeleteTeacher(int teacherId);
        Task<List<Teacher>> GetAllTeachers();
        Task<Teacher?> GetTeacherByName(string firstName, string lastName);
        Task<Teacher?> GetTeacherById(int teacherId);
        Task<List<Teacher>> GetTeachersBySubject(int subjectId);
        Task<Teacher> UpdateTeacher(int Id, Teacher teacher);

        Task<List<Teacher>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime);
    }
}