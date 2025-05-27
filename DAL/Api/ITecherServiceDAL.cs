using DAL.Models;

namespace DAL.Api
{
    public interface ITecherServiceDAL
    {
        Task<Teacher> AddTeacherAsync(Teacher teacher);
        Task<bool> DeleteTeacherAsync(int teacherId);
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByNameAsync(int teacherId);
        Task<Teacher?> GetTeacherByUserIdAsync(int userId);
        Task<List<Teacher>> GetTeachersBySubjectAsync(int subjectId);
        Task<Teacher> UpdateTeacherBioAsync(string bio, string name);
    }
}