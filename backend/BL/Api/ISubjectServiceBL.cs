using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ISubjectServiceBL
    {
        Task AddSubject(SubjectBL subjectBL);
        Task DeleteSubjectByName(string name);
        Task<List<SubjectBL>> GetAllSubjects();
        Task<SubjectBL> GetSubjectById(int id);
        Task<SubjectBL> GetSubjectByName(string name);
        Task UpdateSubject(int id, JsonPatchDocument<SubjectBL> patchDoc);

        Task<List<TeacherBL>> GetTeachersBySubjectName(string name);


        Task<List<LessonBL>> GetLessonsBySubjectName(string name);
       
        
    }
}