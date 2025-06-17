using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    internal interface IUserServiceBL
    {
        Task<UserBL> AddUser(UserBL userBL);
        Task DeleteUser(int userId);
        Task<List<UserBL>> GetAllUsers();
        Task<UserBL?> GetUserById(int userId);
        Task<UserBL?> GetUserByIdIncludeRole(int userId);
        Task<UserBL?> GetUserByName(string firstName, string lastName);
        Task<UserBL> GetUserByNameIncludeRole(string firstName, string lastName);
        Task<UserBL> UpdateStudent(int userId, JsonPatchDocument<UserBL> patchDoc, JsonPatchDocument<TeacherBL>? patchDoc1, JsonPatchDocument<StudentBL>? patchDoc2);
    }
}