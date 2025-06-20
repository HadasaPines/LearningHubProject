using DAL.Models;

namespace DAL.Api
{
    public interface IRegistrationServiceDAL
    {
        Task<List<Registration>> GetAllRegistrations();
        Task<Registration> GetRegistrationById(int registrationId);
        Task<Lesson> GetLessonByRegistrationId(int id);
        Task<Student> GetStudentByRegistrationId(int id);
        Task<List<Registration>> GetRegistrationsToLesson(Lesson lesson);
        Task AddRegistration(Registration registration,Lesson lesson);
        Task DeleteRegistration(Registration registration,Lesson lesson);
        Task<List<Registration>> GetRegistrationsToStudent(Student student);
        Task UpdateRegistration(Registration registration);
    }
}