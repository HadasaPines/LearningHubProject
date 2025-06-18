using AutoMapper;
using BL.Exceptions.TeacherAvailabilityExceptions;
using BL.Models;
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
        private readonly TeacherAvailabilityServiceDAL _teacherAvailabilityServiceDAL;
        private readonly IMapper _mapper;
        public TeacherAvailabilityServiceBL(TeacherAvailabilityServiceDAL teacherAvailabilityServiceDAL, IMapper mapper)
        {
            _teacherAvailabilityServiceDAL = teacherAvailabilityServiceDAL;
            _mapper = mapper;
        }

        public async Task AddTeacherAvailability(TeacherAvailabilityBL teacherAvailabilityBL)
        {
            TeacherAvailability teacherAvailability = _mapper.Map<TeacherAvailability>(teacherAvailabilityBL);
            await _teacherAvailabilityServiceDAL.AddTeacherAvailability(teacherAvailability);
        }

        public async Task<TeacherAvailabilityBL> UpdateTeacherAvailability(int id, JsonPatchDocument<TeacherAvailabilityBL> patchDoc)
        {
            var teacherAvailability = await _teacherAvailabilityServiceDAL.GetTeacherAvailabilitiesById(id);
            var teacherAvailabilityBL = _mapper.Map<TeacherAvailabilityBL>(teacherAvailability);
            if (teacherAvailability == null)

                throw new TeacherAvailabilityNotFoundException("teacher availability not found to update");
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
