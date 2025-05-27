using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class UserServiceDAL : IUserServiceDAL
    {
        private readonly MyDbContext dbContext;
        public UserServiceDAL(MyDbContext context)
        {
            dbContext = context;
        }
        public async Task<User> GetUserById(int userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }
        public async Task<User> GetUserByName(string name)
        {
            return await dbContext.Users.FirstOrDefaultAsync(u => u.FullName == name);
        }
        public async Task AddUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateUserName(int id, string name)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                throw new Exception($"User with ID {id} not found.");
            user.FullName = name;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserEmail(int id, string email)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                throw new Exception($"User with ID {id} not found.");
            user.Email = email;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserPassword(int id, string password)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                throw new Exception($"User with ID {id} not found.");
            user.PasswordHash = password;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteUser(int userId)
        {
            var user = await GetUserById(userId);
            if (user == null)
                throw new Exception($"User with ID {userId} not found.");
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await dbContext.Users.ToListAsync();
        }






    }
}
