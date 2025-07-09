using AutoMapper;
using BL.Api;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Exceptions.RegistrationExceptions;
using BL.Exceptions.LessonExceptions;
using BL.Exceptions.UserExceptions;

namespace BL.Services
{
    public class RegistrationServiceBL : IRegistrationServiceBL
    {
        private readonly IRegistrationServiceDAL _registrationServiceDAL;
        private readonly ILessonServiceDAL _lessonServiceDAL;
        private readonly IUserServiceDAL _userServiceDAL;
        private readonly IMapper _mapper;

        public RegistrationServiceBL(IRegistrationServiceDAL registrationServiceDAL,ILessonServiceDAL lessonServiceDAL,IUserServiceDAL userServiceDAL, IMapper mapper)
        {
            _registrationServiceDAL = registrationServiceDAL;
            _lessonServiceDAL = lessonServiceDAL;
            _userServiceDAL = userServiceDAL;
            _mapper = mapper;
        }

        public async Task<List<RegistrationBL>> GetAllRegistrations()
        {
            return _mapper.Map<List<RegistrationBL>>(await _registrationServiceDAL.GetAllRegistrations());

        }

        public async Task<RegistrationBL> GetRegistrationById(int registrationId)
        {
            var registration = await _registrationServiceDAL.GetRegistrationById(registrationId);
            if (registration == null)
            {
                throw new RegistrationNotFoundException($"Registration with ID {registrationId} not found");
            }
            return _mapper.Map<RegistrationBL>(registration);
        }

        public async Task AddRegistration(RegistrationBL registrationBL)
        {
            if (registrationBL == null)
            {
                throw new ArgumentNullException(nameof(registrationBL), "Registration cannot be null");
            }
            var lesson = await _lessonServiceDAL.GetLessonById(registrationBL.LessonId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {registrationBL.LessonId} not found");
            }
            var user = await _userServiceDAL.GetUserByIdIncludeRole(registrationBL.StudentId);
            if (user == null)
            {
                throw new UserNotFoundException($"User with ID {registrationBL.StudentId} not found");
            }
            if(lesson.Status!= "Available")
            {
                throw new NotAvailableLessonException($"Lesson with ID {registrationBL.LessonId} is not available for registration");
            }
            if(user.Role != "Student" || user.Student?.Age>lesson.MaxAge|| user.Student?.Age<lesson.MinAge||user.Student?.Gender!=lesson.Gender)
            {
                throw new RegistrationNotAllowedException($"Registration not allowed for user with ID {registrationBL.StudentId} to lesson with ID {registrationBL.LessonId}");

            }

            await _registrationServiceDAL.AddRegistration(_mapper.Map<Registration>(registrationBL), lesson);
        }

        public async Task UpdateRegistration(int id, JsonPatchDocument<RegistrationBL> patchDoc)
        {
            if (patchDoc == null)
            {
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            }
            var registration = await _registrationServiceDAL.GetRegistrationById(id);
            if (registration == null)
            {
                throw new RegistrationNotFoundException($"Registration with ID {id} not found");
            }
            var registrationBL = _mapper.Map<RegistrationBL>(registration);
            patchDoc.ApplyTo(registrationBL);
           await  _registrationServiceDAL.UpdateRegistration(registration);
        }

        public async Task DeleteRegistration(int id)
        {
            if (id == 0)
            { return; }
            var registration = await _registrationServiceDAL.GetRegistrationById(id);

            if(registration == null)
            {
                throw new RegistrationNotFoundException($"registration with id: {id} not found");

                
            }
            var lesson=await  _lessonServiceDAL.GetLessonById(registration.LessonId);
            if(lesson == null)
            {
                throw new LessonNotFoundException($"this lesson not found");
               
            }
            await _registrationServiceDAL.DeleteRegistration(id, lesson);




     }
        public async Task DeleteRegistrationByLessonId(int lessonId)
        {
            if (lessonId == 0)
            { return; }
            var lesson = await _lessonServiceDAL.GetLessonById(lessonId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {lessonId} not found");
            }
            await _registrationServiceDAL.DeleteRegistrationByLessonId(lessonId);


        }
        public async Task<RegistrationBL> GetRegistrationByLessonId(int lessonId)
        {
            var registration = await _registrationServiceDAL.GetRegistrationByLessonId(lessonId);
            if (registration == null)
            {
                throw new RegistrationNotFoundException($"Registration for lesson with ID {lessonId} not found");
            }
            return _mapper.Map<RegistrationBL>(registration);

        }
        public async Task<List<RegistrationBL>> GetRegistrationsToStudent(int studentId)
        {
           
      
            var registrations = await _registrationServiceDAL.GetRegistrationsToStudent(studentId);

            return _mapper.Map<List<RegistrationBL>>(registrations);
        }
        public async Task<LessonBL> GetLessonByRegistrationId(int registrationId)
        {
            var lesson = await _registrationServiceDAL.GetLessonByRegistrationId(registrationId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {registrationId} not found");
            }

            return _mapper.Map<LessonBL>(lesson);
        }
        public async Task<StudentBL> GetStudentByRegistrationId(int registrationId)
        {
            var student = await _registrationServiceDAL.GetStudentByRegistrationId(registrationId);
            if (student == null)
            {
                throw new UserNotFoundException($"Student with ID {registrationId} not found");
            }

            return _mapper.Map<StudentBL>(student);
        }
    }
}
