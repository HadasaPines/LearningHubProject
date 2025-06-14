using BL.Models;
using DAL.Models;

namespace BL.Api
{
    public interface IUserManager
    {
        Task AddUser(UserBL userBL);
        Task<User> GetUserByNameAndPassword(string firstName, string lastName, string password);
        Task DeleteUser(string firstName,string lastName, int id);
    }
}