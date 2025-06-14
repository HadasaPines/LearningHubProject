using DAL.Models;

namespace DAL.Api
{
    public interface ITeachersToSubjectServiceDAL
    {
        Task AddTeacherToSubject(TeachersToSubject teachersToSubject);
        Task DeleteTeacherToSubjectByTeacherAndSubjectNam(string firstName, string lastName, string subjectName);
        Task<List<TeachersToSubject>> GetAllTeachersToSubjects();
        Task<List<Subject>> GetSubjectToTeacherByTeacherName(string firstName, string lastName);
        Task<List<Teacher>> GetTeachersToSubjectByTeacherName(string subjectName);
    }
}