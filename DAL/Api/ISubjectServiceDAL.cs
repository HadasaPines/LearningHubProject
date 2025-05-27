using DAL.Models;

namespace DAL.Api
{
    public interface ISubjectServiceDAL
    {
        Task AddSubject(Subject subject);
        Task DeleteSubjectByName(string name);
        Task<List<Subject>> GetAllSubjects();
        Task<Subject> GetSubjectByName(string name);
    }
}