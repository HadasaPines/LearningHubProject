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

        public async Task AddTeacher(TecherBL techerBL)
        {
            if (techerBL == null)
                throw new ArgumentNullException("Teacher cannot be null");

            var teacher = _mapper.Map<Teacher>(techerBL);
            await _teacherService.AddTeacher(teacher);
        }
        public async Task DeleteTeacher(int teacherId)
        {

            await _teacherService.DeleteTeacher(teacherId);
        }
        public async Task<List<TecherBL>> GetAllTeachers()
        {
            var teachers = await _teacherService.GetAllTeachers();
            return _mapper.Map<List<TecherBL>>(teachers);
        }

        public async Task<TecherBL?> GetTeacherByName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                throw new RequiredFieldsNotFilledException();

            var teacher = await _teacherService.GetTeacherByName(firstName, lastName);
            return _mapper.Map<TecherBL>(teacher);
        }

        public async Task<TecherBL?> GetTeacherBId(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Teacher ID must be a positive integer", nameof(teacherId));

            var teacher = await _teacherService.GetTeacherById(teacherId);
            return _mapper.Map<TecherBL>(teacher);
        }
        public async Task<List<TecherBL>> GetTeachersBySubject(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be a positive integer", nameof(subjectId));

            var teachers = await _teacherService.GetTeachersBySubject(subjectId);
            return _mapper.Map<List<TecherBL>>(teachers);
        }


        public async Task<TecherBL> UpdateTeacher(int id, JsonPatchDocument<Teacher> patchDoc)
        {
            var teacher = await _teacherService.GetTeacherById(id);
            if (teacher == null)
                throw new UserIdNotFoundException(id);
            patchDoc.ApplyTo(teacher);
            var updateTeacher = await _teacherService.UpdateTeacher(id, teacher);
            return _mapper.Map<TecherBL>(updateTeacher);
        }

        public async Task AddLessonToTeacher(int teacherId, Lesson lesson)
        {
            if (lesson == null)
                throw new ArgumentNullException(nameof(lesson), "Lesson cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);
            if (teacher.TeacherAvailabilities.Any(t => t.StartTime <= lesson.StartTime
            && t.EndTime >= lesson.EndTime) && teacher.Gender == lesson.Gender &&
            teacher.TeachersToSubjects.Any(t => t.SubjectId == lesson.SubjectId))
            {
                teacher.Lessons.Add(lesson);
                await _teacherService.UpdateTeacher(teacher.TeacherId, null);

            }
            else throw new MismatchTeacherAndLessonException();

        }
        public async Task<List<Lesson>> GetLessonsByTeacherId(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            return teacher.Lessons.ToList();
        }

        public async Task<List<TecherBL>> GetTeachersByAvailability(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            if (date == default || startTime == default || endTime == default)
                throw new ArgumentException("Date and time parameters cannot be default values");

            var teachers = await _teacherService.GetTeachersByAvailability(date, startTime, endTime);
            return _mapper.Map<List<TecherBL>>(teachers);
        }

        public async Task AddAvailabilityToTeacher(int teacherId, TeacherAvailability availability)
        {
            if (availability == null)
                throw new ArgumentNullException("Availability cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);


            if (teacherId != availability.TeacherId)
            {
                throw new MismatchTeacherAndAvailabilityException();
            }

            teacher.TeacherAvailabilities.Add(availability);
            await _teacherService.UpdateTeacher(teacher.TeacherId, teacher);





        }

        public async Task<List<TeacherAvailability>> GetTeacherAvailabilities(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            return teacher.TeacherAvailabilities.ToList();
        }

        public async Task AddSubjectToTeacher(int teacherId, TeachersToSubject teachersToSubject)
        {
            if (teachersToSubject == null)
                throw new ArgumentNullException("TeachersToSubject cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            if (teacherId != teachersToSubject.TeacherId)
            {
                throw new MismatchTeacherAndSubjectException();
            }

            teacher.TeachersToSubjects.Add(teachersToSubject);
            await _teacherService.UpdateTeacher(teacher.TeacherId, teacher);
        }

        public async Task<List<TeachersToSubject>> GetTeachersSubjects(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserIdNotFoundException(teacherId);

            return teacher.TeachersToSubjects.ToList();
        }
    }
}
