using AutoMapper;
using BL.Exceptions.UserExceptions;
using BL.Models;
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
using BL.Api;

namespace BL.Services
{
    public class UserServiceBL : IUserServiceBL
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
        public async Task<List<UserIncludeRoleBL>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return _mapper.Map<List<UserIncludeRoleBL>>(users);
        }

        public async Task<UserWithoutPassBL?> GetUserById(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            return _mapper.Map<UserWithoutPassBL?>(user);
        }

        public async Task<UserWithoutPassBL?> GetUserByName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException("first name/last name can't be null or empty");

            var user = await _userService.GetUserByName(firstName, lastName);
            if (user == null)
            {
                throw new UserNotFoundException($"User with name {firstName} {lastName} not found");
            }

            return _mapper.Map<UserWithoutPassBL?>(user);
        }

        public async Task<UserIncludeRoleBL?> GetUserByIdIncludeRole(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserByIdIncludeRole(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }

            return _mapper.Map<UserIncludeRoleBL?>(user);
        }

        public async Task<UserIncludeRoleBL?> GetUserByNameIncludeRole(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException("first name / last name can't be null or empty");

            var user = await _userService.GetUserByNameIncludeRole(firstName, lastName);
            if (user == null)
            {
                throw new UserNotFoundException($"User with name {firstName}{lastName} not found");

            }

            return _mapper.Map<UserIncludeRoleBL?>(user);
        }

        public async Task<UserWithoutPassBL?> GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new RequiredFieldsNotFilledException("Email can't be null or empty");

            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                throw new UserNotFoundException($"User with email {email} not found");
            }

            return _mapper.Map<UserWithoutPassBL?>(user);
        }

        public async Task<UserIncludeRoleBL?> GetUserByEmailAndPassword(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new RequiredFieldsNotFilledException("email/password can't be null or empty");

            var user = await _userService.GetUserByEmail(email);
            if (user == null)
            {
                throw new UserNotFoundException($"User with email {email} not found");
            }
            if (!IsCorrectPassword(user, password))
            {
                throw new WrongPasswordException("Wrong password.");
            }

            return _mapper.Map<UserIncludeRoleBL?>(user);
        }
        public async Task<UserIncludeRoleBL?> GetUserByNameAndPassword(string firstName, string lastName, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(password))
                throw new RequiredFieldsNotFilledException("first name/last name/password can't be null or empty");
            var user = await _userService.GetUserByName(firstName, lastName);
            if (user == null)
            {
                throw new UserNotFoundException($"User with name {firstName} {lastName} not found");
            }

            if (!IsCorrectPassword(user, password))
            {
                throw new WrongPasswordException("Wrong password");
            }
            return _mapper.Map<UserIncludeRoleBL?>(user);
        }

        public async Task<UserIncludeRoleBL?> GetUserByIdAndPassword(int id, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new RequiredFieldsNotFilledException("Password cannot be null or empty.");


            if (id <= 0)
                throw new ArgumentException("User ID must be greater than zero", nameof(id));
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {id} not found");
            }
            if (!IsCorrectPassword(user, password))
            {
                throw new WrongPasswordException("Wrong password.");
            }
            return _mapper.Map<UserIncludeRoleBL?>(user);

        }

        public async Task<UserWithoutPassBL?> AddUser(UserBL userBL)

        {
            if (userBL == null)
                throw new ArgumentNullException("User cannot be null");
            var existingUser = await _userService.GetUserById(userBL.UserId);
            if (existingUser != null)
            {
                throw new UserAlreadyExistsException($"User with ID {userBL.UserId} already exists.");
            }

            if (string.IsNullOrWhiteSpace(userBL.FirstName) || string.IsNullOrWhiteSpace(userBL.LastName))
                throw new RequiredFieldsNotFilledException("user details can't be null or empty");
            string passwordHash = HashPassword(userBL.Password);

            var user = _mapper.Map<User>(userBL);
            user.PasswordHash = passwordHash;
            await _userService.AddUser(user);

            return _mapper.Map<UserWithoutPassBL?>(user);
        }

        public async Task DeleteUser(int userId)
        {
            //if (userId <= 0)
            //    throw new ArgumentException("User ID must be greater than zero");

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
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

        public async Task<UserBL> UpdateUser(int userId, JsonPatchDocument<UserBL> patchDoc)
        {
            if (patchDoc == null)
            {
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            }
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {userId} not found");
            }
            var userBL = _mapper.Map<UserBL>(user);
            patchDoc.ApplyTo(userBL);

            var updatedUser = _mapper.Map<User>(userBL);
            await _userService.UpdateUser(updatedUser);

            return userBL;
        }

        private bool IsCorrectPassword(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new RequiredFieldsNotFilledException("Password can't be null or empty");
            }
            string passwordHash = HashPassword(password);
            return user.PasswordHash == passwordHash;


        }
        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
