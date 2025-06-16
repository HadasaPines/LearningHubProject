using DAL.Models;

namespace DAL.Api
{
    public interface IUserServiceDAL
    {
        Task AddUser(User user, Teacher? techer, Student? student);
        Task DeleteUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByIdIncludeRole(int userId);
        Task<User?> GetUserByName(string firstName, string lastName);
        Task<User> GetUserByNameIncludeRole(string firstName, string lastName);
        Task<bool> IsPasswordMatchToName(string firstName, string lastName, string password);
    }
}