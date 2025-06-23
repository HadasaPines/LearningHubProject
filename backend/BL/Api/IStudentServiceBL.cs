using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface IStudentServiceBL
    {
        Task AddStudent(StudentBL studentBL);
        Task DeleteStudent(int studentId);
        Task DeleteStudent(StudentBL studentBL);
        Task<List<StudentBL>> GetAllStudents();
        Task<StudentBL> GetStudentById(int studentId);
        Task<StudentBL> GetStudentByName(string firstName, string lastName);
        Task<StudentBL> UpdateStudent(int studentId, JsonPatchDocument<StudentBL> patchDoc);
        Task<RegistrationBL> AddRegistrationToStudent(int studentId, RegistrationBL registrationBL);

    }
}