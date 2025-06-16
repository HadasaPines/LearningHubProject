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
        public async Task<User?> GetUserByIdIncludeRole(int userId)
        {
            var user = await dbContext.Users
                          .Include(u => u.Teacher)
                          .Include(u => u.Student)
                          .FirstOrDefaultAsync(u => u.UserId == userId);
            return user;


        }
        public async Task<User?> GetUserById(int userId)
        {
            var user = await dbContext.Users
                          .FirstOrDefaultAsync(u => u.UserId == userId);
            return user;
        }
        public async Task<User> GetUserByNameIncludeRole(string firstName, string lastName)
        {
            var user = await dbContext.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                .FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
            return user;

           
        }

        public async Task<User?> GetUserByName(string firstName, string lastName)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
            return user;
        }

        public async Task<bool> IsPasswordMatchToName(string firstName, string lastName, string password)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.FirstName == firstName && u.LastName == lastName);
            if (user == null)
                return false;
            return user.PasswordHash == password;
        }
        public async Task AddUser(User user, Teacher? techer, Student? student)
        {
            dbContext.Users.Add(user);
            if (techer != null)
            {
                dbContext.Teachers.Add(techer);
            }
            if (student != null)
            {
                dbContext.Students.Add(student);
            }
            await dbContext.SaveChangesAsync();

        }

        public async Task DeleteUser(User user)
        {


            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await dbContext.Users.ToListAsync();
        }







    }
}
