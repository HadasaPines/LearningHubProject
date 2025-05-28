using BL.Models;

namespace BL.Api
{
    public interface IUserManager
    {
        Task AddUser(UserBL userBL);
    }
}