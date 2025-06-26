using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface IUserServiceBL
    {
        Task<UserWithoutPassBL?> AddUser(UserBL userBL);
        Task DeleteUser(int userId);
        Task<List<UserIncludeRoleBL>> GetAllUsers();
        Task<UserWithoutPassBL?> GetUserByEmail(string email);
        Task<UserIncludeRoleBL?> GetUserByEmailAndPassword(string email, string password);
        Task<UserWithoutPassBL?> GetUserById(int userId);
        Task<UserIncludeRoleBL?> GetUserByIdAndPassword(int id, string password);
        Task<UserIncludeRoleBL?> GetUserByIdIncludeRole(int userId);
        Task<UserWithoutPassBL?> GetUserByName(string firstName, string lastName);
        Task<UserIncludeRoleBL?> GetUserByNameAndPassword(string firstName, string lastName, string password);
        Task<UserIncludeRoleBL?> GetUserByNameIncludeRole(string firstName, string lastName);
        Task<UserBL> UpdateUser(int userId, JsonPatchDocument<UserBL> patchDoc);
    }
}