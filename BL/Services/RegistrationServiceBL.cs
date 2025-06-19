using AutoMapper;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class RegistrationServiceBL
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
            return _mapper.Map<RegistrationBL>(await _registrationServiceDAL.GetRegistrationById(registrationId));
        }

        public async Task AddRegistration(RegistrationBL registrationBL)
        {
            var registration = _mapper.Map<Registration>(registrationBL);
            await _registrationServiceDAL.AddRegistration(registration);
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
                throw new KeyNotFoundException($"Registration with ID {id} not found");
            }
            var registrationBL = _mapper.Map<RegistrationBL>(registration);
            patchDoc.ApplyTo(registrationBL);
            _registrationServiceDAL.UpdateRegistration(registration);
        }

        public async Task DeleteRegistration(int registrationId)
        {
            var registration = await _registrationServiceDAL.GetRegistrationById(registrationId);
            if (registration == null)
            {
                throw new KeyNotFoundException($"Registration with ID {registrationId} not found");
            }
            await _registrationServiceDAL.DeleteRegistration(registrationId);
        }
        public async Task<List<RegistrationBL>> GetRegistrationsToLesson(LessonBL lessonBL)
        {
            var lesson = _mapper.Map<Lesson>(lessonBL);
            var registrations = await _registrationServiceDAL.GetRegistrationsToLesson(lesson);
            return _mapper.Map<List<RegistrationBL>>(registrations);
        }
        public async Task<List<RegistrationBL>> GetRegistrationsToStudent(StudentBL studentBL)
        {
            var student = _mapper.Map<Student>(studentBL);
            var registrations = await _registrationServiceDAL.GetRegistrationsToStudent(student);
            return _mapper.Map<List<RegistrationBL>>(registrations);
        }
    } }
