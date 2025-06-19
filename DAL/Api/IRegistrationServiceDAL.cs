using DAL.Models;

namespace DAL.Api
{
    public interface IRegistrationServiceDAL
    {
        Task<List<Registration>> GetAllRegistrations();
        Task<Registration> GetRegistrationById(int registrationId);
        Task<List<Registration>> GetRegistrationsToLesson(Lesson lesson);

        Task AddRegistration(Registration registration);
        Task DeleteRegistration(int registrationId);
        Task<List<Registration>> GetRegistrationsToStudent(Student student);
        Task UpdateRegistration(Registration registration);
    }
}