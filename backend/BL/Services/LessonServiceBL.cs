using AutoMapper;
using BL.Api;
using BL.Exceptions.LessonExceptions;
using BL.Exceptions.RegistrationExceptions;
using BL.Models;
using BL.service;
using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class LessonServiceBL : ILessonServiceBL
    {
        private readonly ILessonServiceDAL _lessonServiceDAL;
        private readonly IMapper _mapper;
        private readonly ITeacherAvailabilityServiceDAL _teacherAvailabilityServiceDAL;
        private readonly IRegistrationServiceDAL _registrationServiceDAL;

        public LessonServiceBL(ILessonServiceDAL lessonServiceDAL, IMapper mapper, ITeacherAvailabilityServiceDAL teacherAvailabilityServiceDAL, IRegistrationServiceDAL registrationServiceDAL)
        {
            _lessonServiceDAL = lessonServiceDAL;
            _mapper = mapper;
            _teacherAvailabilityServiceDAL = teacherAvailabilityServiceDAL;
            _registrationServiceDAL = registrationServiceDAL;
        }
        public async Task<List<LessonBL>> GetAllLessons()
        {
            return _mapper.Map<List<LessonBL>>(await _lessonServiceDAL.GetAllLessons());
        }

        public async Task<LessonBL> GetLessonById(int lessonId)
        {
            var lesson = await _lessonServiceDAL.GetLessonById(lessonId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {lessonId} not found");
            }
            return _mapper.Map<LessonBL>(lesson);
        }

        public async Task AddLesson(LessonBL lessonBL)
        {
            if (lessonBL == null)
            {
                throw new ArgumentNullException(nameof(lessonBL), "Lesson cannot be null");
            }
            await _lessonServiceDAL.AddLesson(_mapper.Map<Lesson>(lessonBL));
        }

        public async Task UpdateLesson(int id, JsonPatchDocument<LessonBL> patchDoc)
        {
            if (patchDoc == null)
            {
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            }

            var existingLesson = await _lessonServiceDAL.GetLessonById(id);
            if (existingLesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {id} not found");
            }

            var lessonBL = _mapper.Map<LessonBL>(existingLesson);


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
                {                   if (TimeOnly.TryParseExact(operation.value.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedEndTime))
                    {
                        operation.value = parsedEndTime;
                    }
                    else if (TimeOnly.TryParse(operation.value.ToString(), out var parsedEndDateTime))
                    {
                        operation.value = parsedEndDateTime;
                    }
                }
                else if (operation.path.Contains("lessonDate") && operation.value != null)
                {
                    if (DateOnly.TryParseExact(operation.value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedLessonDate))
                    {
                        operation.value = parsedLessonDate;
                    }
                }
            }
            patchDoc.ApplyTo(lessonBL);

            var updatedLesson = _mapper.Map<Lesson>(lessonBL);
            await _lessonServiceDAL.UpdateLesson(updatedLesson);
        }


        public async Task DeleteLesson(int id)
        {
            var existingLesson = await _lessonServiceDAL.GetLessonById(id);
            if (existingLesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {id} not found");
            }
            await _lessonServiceDAL.DeleteLessonById(id);
        }


        public async Task<List<LessonBL>> GetLessonsByDetails(LessonFilterDto filter)
        {
            IEnumerable<Lesson> lessons = await _lessonServiceDAL.GetAllLessonsIncludeDetails();
            var query = lessons.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Gender))
                query = query.Where(l => l.Gender == filter.Gender);

            if (filter.age.HasValue)
            {
                query = query.Where(l =>
                    (!l.MinAge.HasValue || l.MinAge <= filter.age) &&
                    (!l.MaxAge.HasValue || l.MaxAge >= filter.age));
            }

            if (filter.SpecificDate.HasValue)
                query = query.Where(l => l.LessonDate == filter.SpecificDate);

            if (filter.DateFrom.HasValue)
                query = query.Where(l => l.LessonDate >= filter.DateFrom);

            if (filter.DateTo.HasValue)
                query = query.Where(l => l.LessonDate <= filter.DateTo);

            if (filter.StartTime.HasValue)
                query = query.Where(l => l.StartTime >= filter.StartTime);

            if (filter.EndTime.HasValue)
                query = query.Where(l => l.EndTime <= filter.EndTime);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(l => l.Status == filter.Status);

            if (filter.SubjectId.HasValue)
                query = query.Where(l => l.SubjectId == filter.SubjectId);

            if (filter.TeacherId.HasValue)
                query = query.Where(l => l.TeacherId == filter.TeacherId);

            return _mapper.Map<List<LessonBL>>(query);
        }
        public async Task GenerateLessonsAsync(DateOnly startDate, DateOnly endDate)
        {
            var availabilities = await _teacherAvailabilityServiceDAL.GetAllTeacherAvailabilities();
            TimeSpan lessonLength = TimeSpan.FromMinutes(45);
            var holidayChecker = AvailableQueueManager.Instance;

            foreach (var availability in availabilities)
            {
                for (var date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    if ((int)date.DayOfWeek != availability.WeekDay)
                        continue;

                    var isHolidayOrShabbat = await holidayChecker.IsHolidayOrShabbatAsync(date.ToDateTime(TimeOnly.MinValue));
                    if (isHolidayOrShabbat)
                        continue;

                    var start = availability.StartTime;
                    var end = availability.EndTime;
                    while (start.Add(lessonLength) <= end)
                    {
                        var lesson = new Lesson
                        {
                            TeacherId = availability.TeacherId,
                            SubjectId = 0,
                            LessonDate = date,
                            StartTime = start,
                            EndTime = start.Add(lessonLength),
                            Gender = availability.Teacher.Gender,
                            Status = "available"
                        };

                        await _lessonServiceDAL.AddLesson(lesson);
                        start = start.Add(lessonLength);
                    }
                }
            }

            await _lessonServiceDAL.SaveChangesAsync();
        }
        public async Task UpdatePastLessonsAsync()
        {
            var allLessons = await _lessonServiceDAL.GetAllLessons();

            var today = DateOnly.FromDateTime(DateTime.Today);

            var pastLessons = allLessons
                .Where(lesson => lesson.LessonDate < today && lesson.Status != "Past")
                .ToList();

            foreach (var lesson in pastLessons)
            {
                lesson.Status = "Past";
                await _lessonServiceDAL.UpdateLesson(lesson);
            }

            await _lessonServiceDAL.SaveChangesAsync();
        }


        public async Task<UserBL> GetStudentToLesson(int lessonId)
        {
            var lesson = await _lessonServiceDAL.GetLessonById(lessonId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {lessonId} not found");
            }

            var registration = await _registrationServiceDAL.GetRegistrationByLessonId(lessonId);
            if (registration == null)
            {
                throw new RegistrationNotFoundException($"Registration for lesson with ID {lessonId} not found");
            }


            return _mapper.Map<UserBL>(registration.Student.StudentNavigation);


        }
        public async Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId)
        {
            var lessons = await _lessonServiceDAL.GetLessonsByTeacherId(teacherId);

            return _mapper.Map<List<LessonBL>>(lessons);
        }


        }
}
