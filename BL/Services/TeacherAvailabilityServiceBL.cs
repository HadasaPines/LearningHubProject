using AutoMapper;
using BL.Api;
using BL.Exceptions.TeacherAvailabilityExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class TeacherAvailabilityServiceBL : ITeacherAvailabilityServiceBL
    {
        private readonly ITeacherAvailabilityServiceDAL _teacherAvailabilityServiceDAL;
        private readonly IMapper _mapper;
        public TeacherAvailabilityServiceBL(ITeacherAvailabilityServiceDAL teacherAvailabilityServiceDAL, IMapper mapper)
        {
            _teacherAvailabilityServiceDAL = teacherAvailabilityServiceDAL;
            _mapper = mapper;
        }

        public async Task AddTeacherAvailability(TeacherAvailabilityBL teacherAvailabilityBL)
        {

            if (teacherAvailabilityBL == null)
                throw new ArgumentNullException(nameof(teacherAvailabilityBL), "Teacher availability cannot be null");
            TeacherAvailability teacherAvailability = _mapper.Map<TeacherAvailability>(teacherAvailabilityBL);
            await _teacherAvailabilityServiceDAL.AddTeacherAvailability(teacherAvailability);
        }

        public async Task<TeacherAvailabilityBL> UpdateTeacherAvailability(int id, JsonPatchDocument<TeacherAvailabilityBL> patchDoc)
        {
            if (patchDoc == null)
                throw new ArgumentNullException("TeachersToSubject cannot be null");
            var teacherAvailability = await _teacherAvailabilityServiceDAL.GetTeacherAvailabilitiesById(id);
           
            if (teacherAvailability == null)

                throw new TeacherAvailabilityNotFoundException("teacher availability not found to update");
            var teacherAvailabilityBL = _mapper.Map<TeacherAvailabilityBL>(teacherAvailability);
            patchDoc.ApplyTo(teacherAvailabilityBL);
            var updateTeacherAvailability = _mapper.Map<TeacherAvailability>(teacherAvailabilityBL);
            await _teacherAvailabilityServiceDAL.UpdateTeacherAvailability(updateTeacherAvailability);
            return teacherAvailabilityBL;

        }

        public async Task DeleteTeacherAvailability(int id)
        {
            var teacherAvailability = await _teacherAvailabilityServiceDAL.GetTeacherAvailabilitiesById(id);
            if (teacherAvailability == null)
            {
                throw new TeacherAvailabilityNotFoundException("teacher availability not found to delete");
            }
            await _teacherAvailabilityServiceDAL.DeleteTeacherAvailability(teacherAvailability);
        }

        public async Task<List<TeacherAvailabilityBL>> GetAllTeacherAvailabilities()
        {

            var teacherAvailabilities = await _teacherAvailabilityServiceDAL.GetAllTeacherAvailabilities();
            return _mapper.Map<List<TeacherAvailabilityBL>>(teacherAvailabilities);
        }
        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByWeekDay(int weekDay)
        {
            var teacherAvailabilities = await _teacherAvailabilityServiceDAL.GetTeacherAvailabilitiesByWeekDay(weekDay);
            return _mapper.Map<List<TeacherAvailabilityBL>>(teacherAvailabilities);
        }
        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByTimeRange(TimeOnly startTime, TimeOnly endTime)
        {
            var teacherAvailabilities = await _teacherAvailabilityServiceDAL.GetTeacherAvailabilitiesByTimeRange(startTime, endTime);
            return _mapper.Map<List<TeacherAvailabilityBL>>(teacherAvailabilities);
        }


    }
}
