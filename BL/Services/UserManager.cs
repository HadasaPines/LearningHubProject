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
            // בדיקת תקינות ת"ז
            if (!IsValidIsraeliId(userBL.UserId.ToString()))
                throw new ArgumentException("מספר תעודת זהות שגוי");

            // בדיקת ייחודיות ת"ז
            //bool exists = await _userService.UserIdExistsAsync(userBL.UserId);
            //if (exists)
            //    throw new InvalidOperationException("משתמש עם תעודת זהות זו כבר קיים");

            // מיפוי ושמירה
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

    }
}
