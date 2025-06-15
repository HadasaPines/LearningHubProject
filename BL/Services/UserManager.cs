using AutoMapper;
using BL.Api;
using BL.Models;
using DAL.Api;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BL.Exceptions.UserExceptions;

namespace BL.Services
{
    public class UserManager : IUserManager
    {
        private readonly IMapper _mapper;
        private readonly IUserServiceDAL _userService;

        public UserManager(IMapper mapper, IUserServiceDAL userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        public async Task AddUser(UserBL userBL)
        {
            if (!IsValidIsraeliId(userBL.UserId.ToString()))
                throw new InvalidIdException();

            var passwordHasher = new PasswordHasher<UserBL>();
            userBL.PasswordHash = passwordHasher.HashPassword(userBL, userBL.PasswordHash);


            User user = _mapper.Map<User>(userBL);
            await _userService.AddUser(user);
        }

        private static bool IsValidIsraeliId(string id)
        {
            id = id.Trim().PadLeft(9, '0');
            if (!Regex.IsMatch(id, @"^\d{9}$")) return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = int.Parse(id[i].ToString());
                int factor = (i % 2 == 0) ? 1 : 2;
                int product = digit * factor;
                sum += (product > 9) ? product - 9 : product;
            }

            return sum % 10 == 0;
        }

        public async Task<User> GetUserByNameAndPassword(string firstName, string lastName, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("שם משתמש או סיסמה אינם תקינים");

            User user = await _userService.GetUserByName(firstName, lastName);

            if (user == null)
                throw new UnauthorizedAccessException("שם משתמש או סיסמה שגויים");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("שם משתמש או סיסמה שגויים");

            return user;
        }

        public async Task DeleteUser(string firstName,string lastName, int id)
        {
            if (string.IsNullOrWhiteSpace(firstName)|| string.IsNullOrWhiteSpace(lastName) )
                throw new ArgumentException("שם משתמש או סיסמה אינם תקינים");
            User user = await _userService.GetUserByName(firstName,lastName);
            if (user.UserId != id)
            {
                throw new Exception("name and id not mach");
            }
            await _userService.DeleteUser(id);
        }

    }
}
