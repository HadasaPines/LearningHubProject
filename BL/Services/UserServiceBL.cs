using AutoMapper;
using BL.Exceptions.UserExceptions;
using BL.Models;
using BL.Api;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BL.Services
{
    internal class UserServiceBL : IUserServiceBL
    {
        private readonly IMapper _mapper;
        private readonly IUserServiceDAL _userService;
        private readonly IStudentServiceBL _studentServiceBL;
        private readonly ITeacherServiceBL _teacherServiceBL;
        public UserServiceBL(IMapper mapper, IUserServiceDAL userServiceDAL, IStudentServiceBL studentServiceBL, ITeacherServiceBL teacherServiceBL)
        {
            _mapper = mapper;
            _userService = userServiceDAL;
            _studentServiceBL = studentServiceBL;
            _teacherServiceBL = teacherServiceBL;

        }
        public async Task<List<UserBL>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return _mapper.Map<List<UserBL>>(users);
        }

        public async Task<UserBL?> GetUserById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new UserIdNotFoundException(userId);
            }

            return _mapper.Map<UserBL>(user);
        }

        public async Task<UserBL?> GetUserByName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException();

            var user = await _userService.GetUserByName(firstName, lastName);
            if (user == null)
            {
                throw new UserNameNotFoundException(firstName, lastName);
            }

            return _mapper.Map<UserBL>(user);
        }

        public async Task<UserBL?> GetUserByIdIncludeRole(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserByIdIncludeRole(userId);
            if (user == null)
            {
                throw new UserIdNotFoundException(userId);
            }

            return _mapper.Map<UserBL>(user);
        }

        public async Task<UserBL> GetUserByNameIncludeRole(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException();

            var user = await _userService.GetUserByNameIncludeRole(firstName, lastName);
            if (user == null)
            {
                throw new UserNameNotFoundException(firstName, lastName);
            }

            return _mapper.Map<UserBL>(user);
        }

        public async Task<UserBL> AddUser(UserBL userBL)
        {
            if (userBL == null)
                throw new ArgumentNullException("User cannot be null");

            if (string.IsNullOrWhiteSpace(userBL.FirstName) || string.IsNullOrWhiteSpace(userBL.LastName))
                throw new RequiredFieldsNotFilledException();

            var user = _mapper.Map<User>(userBL);
            await _userService.AddUser(user);
            return userBL;

        }

        public async Task DeleteUser(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new UserIdNotFoundException(userId);
            }
            //if (user.Role == "Admin")
            //{
            //    throw new InvalidOperationException("Cannot delete an admin user.");
            //}
            if (user.Student != null)
            {
                await _studentServiceBL.DeleteStudent(user.Student.StudentId);
            }
            if (user.Teacher != null)
            {
                await _teacherServiceBL.DeleteTeacher(user.Teacher.TeacherId);
            }

            await _userService.DeleteUser(user);
        }

        public async Task<UserBL> UpdateStudent(int userId, JsonPatchDocument<UserBL> patchDoc, JsonPatchDocument<TeacherBL>? patchDoc1, JsonPatchDocument<StudentBL>? patchDoc2)
        {
            var user = await _userService.GetUserByIdIncludeRole(userId);
            if (user == null)
            {

                throw new UserIdNotFoundException(userId);
            }
            var userBL = _mapper.Map<UserBL>(user);
            patchDoc.ApplyTo(userBL);
            var updatedUser = _mapper.Map<User>(userBL);
            await _userService.UpdateUser(updatedUser);

            if (userBL.Role == "Student" && patchDoc2 != null)
            {
                await _studentServiceBL.UpdateStudent(user.Student.StudentId, patchDoc2);
            }

            else if (userBL.Role == "Teacher" && patchDoc1 != null)
            {
                await _teacherServiceBL.UpdateTeacher(user.Teacher.TeacherId, patchDoc1);

            }

            return userBL;
        }






    }
}
