using BL.Models;
using DAL.Models;

namespace BL.Api
{
    public interface IUserManager
    {
        Task AddUser(UserBL userBL);
        Task<User> GetUserByNameAndPassword(string username, string password);
        Task DeleteUser(string name, int id);
    }
}