using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Api
{
    public interface IUserServiceDAL
    {
        Task AddUser(User user);
        Task DeleteUser(User user);
        Task<List<User>> GetAllUsers();
        Task<User?> GetUserById(int userId);
        Task<User?> GetUserByIdIncludeRole(int userId);
        Task<User?> GetUserByName(string firstName, string lastName);
        Task<User?> GetUserByEmail(string email);   
        Task<User> GetUserByNameIncludeRole(string firstName, string lastName);
        Task UpdateUser(User user);
    }
}