using DAL.Models;

namespace DAL.Api
{
    public interface ITecherServiceDAL
    {
        Task<Teacher> AddTeacher(Teacher teacher);
        Task<bool> DeleteTeacher(int teacherId);
        Task<List<Teacher>> GetAllTeachers();
        Task<Teacher?> GetTeacherByName(string firstName, string lastName);
        Task<Teacher?> GetTeacherByUserId(int teacherId);
        Task<List<Teacher>> GetTeachersBySubject(int subjectId);
        Task<Teacher> UpdateTeacherBio(string bio, string firstName,string lastName);
    }
}