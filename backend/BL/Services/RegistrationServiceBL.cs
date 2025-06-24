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
        private readonly IMapper _mapper;

        public RegistrationServiceBL(IRegistrationServiceDAL registrationServiceDAL, IMapper mapper)
        {
            _registrationServiceDAL = registrationServiceDAL;
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
            var lesson = await _registrationServiceDAL.GetLessonByRegistrationId(registrationBL.RegistrationId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {registrationBL.LessonId} not found");
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
            _registrationServiceDAL.UpdateRegistration(registration);
        }

        public async Task DeleteRegistration(RegistrationBL registrationBL)
        {
            if (registrationBL == null)
            {
                throw new ArgumentNullException(nameof(registrationBL), "Registration cannot be null");

            }
            var lesson = await _registrationServiceDAL.GetLessonByRegistrationId(registrationBL.RegistrationId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {registrationBL.LessonId} not found");
            }
            _registrationServiceDAL.DeleteRegistration(_mapper.Map<Registration>(registrationBL), lesson);
        }
        public async Task<List<RegistrationBL>> GetRegistrationsToLesson(LessonBL lessonBL)
        {
            var lesson = _mapper.Map<Lesson>(lessonBL);
            var registrations = await _registrationServiceDAL.GetRegistrationsToLesson(lesson);
            if (registrations == null || !registrations.Any())
            {
                throw new RegistrationNotFoundException($"No registrations found for lesson with ID {lesson.LessonId}");
            }
            return _mapper.Map<List<RegistrationBL>>(registrations);
        }
        public async Task<List<RegistrationBL>> GetRegistrationsToStudent(StudentBL studentBL)
        {
            var student = _mapper.Map<Student>(studentBL);
            var registrations = await _registrationServiceDAL.GetRegistrationsToStudent(student);

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
