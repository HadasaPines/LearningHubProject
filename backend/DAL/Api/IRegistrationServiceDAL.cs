using DAL.Models;

namespace DAL.Api
{
    public interface IRegistrationServiceDAL
    {
        Task AddRegistration(Registration registration, Lesson lesson);
        Task DeleteRegistration(int reregistrationId, Lesson lesson);
        Task<List<Registration>> GetAllRegistrations();
        Task<Lesson> GetLessonByRegistrationId(int id);
        Task<Registration> GetRegistrationById(int registrationId);
        Task<Registration> GetRegistrationByLessonId(int lessonId);
        Task<List<Registration>> GetRegistrationsToStudent(int studentId);
        Task<Student> GetStudentByRegistrationId(int id);
        Task UpdateRegistration(Registration registration);

        Task DeleteRegistrationByLessonId(int lessonId);
    }
}