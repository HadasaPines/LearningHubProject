using DAL.Api;
using DAL.Contexts;
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
        private readonly LearningHubDbContext dbContext;
        public UserServiceDAL(LearningHubDbContext context)
        {
            dbContext = context;
        }
        public async Task<User> GetUserById(int userId)
        {
            return await dbContext.Users.FindAsync(userId);
        }
        public async Task<User> GetUserByName(string firstName,string lastName)
        {

            return await dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == firstName&&u.LastName==lastName);
        }
        public async Task<bool> IsPasswordMatchToName(string firstName, string lastName, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
            if (user == null)
                return false;
            return user.PasswordHash == password;
        }
        public async Task AddUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateUserName(int id, string firstName, string lastName)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
          
            user.FirstName = firstName;
            user.LastName=lastName;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserEmail(int id, string email)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
            
            user.Email = email;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserPassword(int id, string password)
        {
            var user = await dbContext.Users
               .FirstOrDefaultAsync(u => u.UserId == id);
           
            user.PasswordHash = password;
            dbContext.Users.Update(user);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteUser(int userId)
        {
            var user = await GetUserById(userId);
           
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await dbContext.Users.ToListAsync();
        }






    }
}
