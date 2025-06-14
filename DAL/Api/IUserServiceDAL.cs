using DAL.Models;

namespace DAL.Api
{
    public interface IUserServiceDAL
    {
        Task AddUser(User user);
        Task DeleteUser(int userId);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<User> GetUserByName(string firstName, string lastName);
        Task UpdateUserEmail(int id, string email);
        Task UpdateUserName(int id, string firstName, string lastName);
        Task UpdateUserPassword(int id, string password);
        Task<bool> IsPasswordMatchToName(string firstName, string lastName, string password);

    }
}