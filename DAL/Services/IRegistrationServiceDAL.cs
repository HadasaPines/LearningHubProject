using DAL.Models;

namespace DAL.Services
{
    public interface IRegistrationServiceDAL
    {
        Task AddRegistrationAsync(Registration registration);
        Task DeleteRegistration(int registrationId);
        Task<List<Registration>> GetRegistrationsToStudentAsync(string firstName, string lastName);
    }
}