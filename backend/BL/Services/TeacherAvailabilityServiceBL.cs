using AutoMapper;
using BL.Api;
using BL.Exceptions.LessonExceptions;
using BL.Exceptions.TeacherAvailabilityExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (patchDoc == null)
            {
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            }

            var teacherAvailabilityBL = _mapper.Map<TeacherAvailabilityBL>(teacherAvailability);


            foreach (var operation in patchDoc.Operations)
            {
                if (operation.path.Contains("startTime") && operation.value != null)
                {

                    if (TimeOnly.TryParseExact(operation.value.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedStartTime))
                    {
                        operation.value = parsedStartTime;
                    }
                    else if (TimeOnly.TryParse(operation.value.ToString(), out var parsedStartDateTime))
                    {
                        operation.value = parsedStartDateTime;
                    }
                }
                else if (operation.path.Contains("endTime") && operation.value != null)
                {
                    if (TimeOnly.TryParseExact(operation.value.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedEndTime))
                    {
                        operation.value = parsedEndTime;
                    }
                    else if (TimeOnly.TryParse(operation.value.ToString(), out var parsedEndDateTime))
                    {
                        operation.value = parsedEndDateTime;
                    }
                }
            }

                patchDoc.ApplyTo(teacherAvailabilityBL);

                var updatedLesson = _mapper.Map<TeacherAvailability>(teacherAvailabilityBL);
                await _teacherAvailabilityServiceDAL.UpdateTeacherAvailability(updatedLesson); ;
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
