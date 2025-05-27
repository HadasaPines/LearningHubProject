using DAL.Models;

namespace DAL.Api
{
    public interface ITeachersToSubjectServiceDAL
    {
        Task AddTeacherToSubject(TeachersToSubject teachersToSubject);
        Task DeleteTeacherToSubjectByTeacherAndSubjectNam(string teacherName, string subjectName);
        Task<List<TeachersToSubject>> GetAllTeachersToSubjects();
        Task<List<Subject>> GetSubjectToTeacherByTeacherName(string teacherName);
        Task<List<Teacher>> GetTeachersToSubjectByTeacherName(string subjectName);
    }
}