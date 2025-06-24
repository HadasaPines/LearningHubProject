using DAL.Models;

namespace DAL.Api
{
    public interface ISubjectServiceDAL
    {
        Task AddSubject(Subject subject);
        Task DeleteSubjectByName(string name);
        Task UpdateSubject(Subject subject);
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectByName(string name);
        Task<Subject> GetSubjectById(int id);
        Task<List<Lesson>> GetLessonsBySubjectName(string name);
        Task<List<Teacher>> GetTeachersBySubjectName(string name);
    }
}