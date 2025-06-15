using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ITeacherServiceBL
    {
        Task AddTeacher(TecherBL techerBL);
        Task<bool> DeleteTeacher(int teacherId);
        Task<List<TecherBL>> GetAllTeachers();
        Task<TecherBL?> GetTeacherBId(int teacherId);
        Task<TecherBL?> GetTeacherByName(string firstName, string lastName);
        Task<List<TecherBL>> GetTeachersBySubject(int subjectId);
        Task<TecherBL> UpdateTeacher(int id, JsonPatchDocument<Teacher> patchDoc);
    }
}