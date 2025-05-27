using DAL.Models;

namespace DAL.Api
{
    public interface IUserServiceDAL
    {
        Task AddUser(User user);
        Task DeleteUser(int userId);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<User> GetUserByName(string name);
        Task UpdateUserEmail(int id, string email);
        Task UpdateUserName(int id, string name);
        Task UpdateUserPassword(int id, string password);
    }
}