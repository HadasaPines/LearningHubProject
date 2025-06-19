using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface IUserServiceBL
    {
        Task<UserBL> AddUser(UserBL userBL);
        Task DeleteUser(int userId);
        Task<List<UserBL>> GetAllUsers();
        Task<UserBL?> GetUserById(int userId);
        Task<UserBL?> GetUserByIdIncludeRole(int userId);
        Task<UserBL?> GetUserByName(string firstName, string lastName);
        Task<UserBL> GetUserByNameIncludeRole(string firstName, string lastName);
        Task<UserBL?> GetUserByEmail(string email);
        Task<UserBL> UpdateUser(int userId, JsonPatchDocument<UserBL> patchDoc);
        Task<UserBL> GetUserByEmailAndPassword(string email, string password);
        Task<UserBL> GetUserByIdAndPassaword(int id, string passaword);
        Task<UserBL> GetUserByNameAndPassword(string firstName, string lastName, string password);
    }
}