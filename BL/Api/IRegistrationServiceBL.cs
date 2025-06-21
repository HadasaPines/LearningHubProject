using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface IRegistrationServiceBL
    {
        Task AddRegistration(RegistrationBL registrationBL);
        Task DeleteRegistration(RegistrationBL registrationBL);
        Task<List<RegistrationBL>> GetAllRegistrations();
        Task<RegistrationBL> GetRegistrationById(int registrationId);
        Task<List<RegistrationBL>> GetRegistrationsToLesson(LessonBL lessonBL);
        Task<List<RegistrationBL>> GetRegistrationsToStudent(StudentBL studentBL);
        Task UpdateRegistration(int id, JsonPatchDocument<RegistrationBL> patchDoc);
        Task<LessonBL> GetLessonByRegistrationId(int  registrationId);
        Task<StudentBL> GetStudentByRegistrationId(int  registrationId);
    }
}