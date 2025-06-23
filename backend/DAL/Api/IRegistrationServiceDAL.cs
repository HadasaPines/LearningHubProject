using DAL.Models;

namespace DAL.Api
{
    public interface IRegistrationServiceDAL
    {
        Task AddRegistration(Registration registration, Lesson lesson);
        Task DeleteRegistration(Registration registration, Lesson lesson);
        Task<List<Registration>> GetAllRegistrations();
        Task<Lesson> GetLessonByRegistrationId(int id);
        Task<Registration> GetRegistrationById(int registrationId);
        Task<List<Registration>> GetRegistrationsToLesson(Lesson lesson);
        Task<List<Registration>> GetRegistrationsToStudent(Student student);
        Task<Student> GetStudentByRegistrationId(int id);
        Task UpdateRegistration(Registration registration);
    }
}