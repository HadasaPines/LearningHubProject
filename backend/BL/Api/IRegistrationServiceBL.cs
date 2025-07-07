using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface IRegistrationServiceBL
    {
        Task AddRegistration(RegistrationBL registrationBL);
        Task DeleteRegistration(int id);
        Task<List<RegistrationBL>> GetAllRegistrations();
        Task<RegistrationBL> GetRegistrationById(int registrationId);
        Task<RegistrationBL> GetRegistrationByLessonId(int lessonId);
        Task<List<RegistrationBL>> GetRegistrationsToStudent(StudentBL studentBL);
        Task UpdateRegistration(int id, JsonPatchDocument<RegistrationBL> patchDoc);
        Task<LessonBL> GetLessonByRegistrationId(int  registrationId);
        Task<StudentBL> GetStudentByRegistrationId(int  registrationId);

        Task DeleteRegistrationByLessonId(int lessonId);
    }
}