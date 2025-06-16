using AutoMapper;
using BL.Exceptions.UserExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Exceptions;
using BL.Api;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using BL.Exceptions.TeacherExceptions;

namespace BL.Services
{
    public class TeacherServiceBL : ITeacherServiceBL
    {
        private readonly IMapper _mapper;
        private readonly ITecherServiceDAL _teacherService;
        public TeacherServiceBL(IMapper mapper, ITecherServiceDAL techerServiceDAL)
        {
            _mapper = mapper;
            _teacherService = techerServiceDAL;
        }

        public async Task AddTeacher(TeacherBL TeacherBL)
        {
            if (TeacherBL == null)
                throw new ArgumentNullException("Teacher cannot be null");

            var teacher = _mapper.Map<Teacher>(TeacherBL);
            await _teacherService.AddTeacher(teacher);
        }
        public async Task DeleteTeacher(int teacherId)
        {

            await _teacherService.DeleteTeacher(teacherId);
        }
        public async Task<List<TeacherBL>> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachers();
            return _mapper.Map<List<TeacherBL>>(teachers);
        }

        public async Task<TeacherBL?> GetTeacherByName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException();

            var teacher = await _teacherService.GetTeacherByName(firstName, lastName);
            return _mapper.Map<TeacherBL>(teacher);
        }

        public async Task<TeacherBL?> GetTeacherById(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Teacher ID must be a positive integer", nameof(teacherId));

            var teacher = await _teacherService.GetTeacherById(teacherId);
            return _mapper.Map<TeacherBL>(teacher);
        }
        public async Task<List<TeacherBL>> GetTeachersBySubject(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be a positive integer", nameof(subjectId));

            var teachers = await _teacherService.GetTeachersBySubject(subjectId);
            return _mapper.Map<List<TeacherBL>>(teachers);
        }


        public async Task<TeacherBL> UpdateTeacher(int id, JsonPatchDocument<TeacherBL> patchDoc)
        {
            var teacher = await _teacherService.GetTeacherById(id);
            var techerBL = _mapper.Map<TeacherBL>(teacher);
            if (teacher == null)
                throw new UserIdNotFoundException(id);
            patchDoc.ApplyTo(techerBL);
            var updateTeacher = _mapper.Map<Teacher>(techerBL);
            await _teacherService.UpdateTeacher(id, updateTeacher);
            return techerBL;
        }

        public async Task AddLessonToTeacher(int teacherId, LessonBL lessonBL)
        {
            if (lessonBL == null)
                throw new ArgumentNullException(nameof(lessonBL), "Lesson cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);
            if (teacher.TeacherAvailabilities.Any(t => t.StartTime <= lessonBL.StartTime
            && t.EndTime >= lessonBL.EndTime) && teacher.Gender == lessonBL.Gender &&
            teacher.TeachersToSubjects.Any(t => t.SubjectId == lessonBL.SubjectId))
            {
                var lesson = _mapper.Map<Lesson>(lessonBL);
                teacher.Lessons.Add(lesson);
                await _teacherService.UpdateTeacher(teacher.TeacherId, teacher);

            }
            else throw new MismatchTeacherAndLessonException();

        }
        public async Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);
            return _mapper.Map<List<LessonBL>>(teacher.Lessons);
        }

        public async Task<List<TeacherBL>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (date == default || startTime == default || endTime == default)
                throw new ArgumentException("Date and time parameters cannot be default values");

            var teachers = await _teacherService.GetTeachersByAvailability(date, startTime, endTime);
            return _mapper.Map<List<TeacherBL>>(teachers);
        }

        public async Task AddAvailabilityToTeacher(int teacherId, TeacherAvailabilityBL availabilityBL)
        {
            if (availabilityBL == null)
                throw new ArgumentNullException("Availability cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);


            if (teacherId != availabilityBL.TeacherId)
            {
                throw new MismatchTeacherAndAvailabilityException();
            }
            var availability = _mapper.Map<TeacherAvailability>(availabilityBL);
            teacher.TeacherAvailabilities.Add(availability);
            await _teacherService.UpdateTeacher(teacher.TeacherId, teacher);





        }

        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilities(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            return _mapper.Map<List<TeacherAvailabilityBL>>(teacher.TeacherAvailabilities);
        }

        public async Task AddSubjectToTeacher(int teacherId, TeachersToSubjectBL teachersToSubjectBL)
        {
            if (teachersToSubjectBL == null)
                throw new ArgumentNullException("TeachersToSubject cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            if (teacherId != teachersToSubjectBL.TeacherId)
            {
                throw new MismatchTeacherAndSubjectException();
            }
            var teachersToSubject = _mapper.Map<TeachersToSubject>(teachersToSubjectBL);
            teacher.TeachersToSubjects.Add(teachersToSubject);
            await _teacherService.UpdateTeacher(teacher.TeacherId, teacher);
        }

        public async Task<List<TeachersToSubjectBL>> GetTeachersSubjects(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            return _mapper.Map<List<TeachersToSubjectBL>>(teacher.TeachersToSubjects);
        }






    }
}
