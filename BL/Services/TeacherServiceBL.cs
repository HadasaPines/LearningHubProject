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
        public async Task<bool> DeleteTeacher(int teacherId)
        {

            return await _teacherService.DeleteTeacher(teacherId);
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

            var teacher = await _teacherService.GetTeacherByUserId(teacherId);
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

            var teacher = await _teacherService.UpdateTeacher(id, patchDoc);
            return _mapper.Map<TecherBL>(teacher);
        }

        public async Task AddLessonToTeacher(int teacherId, Lesson lesson)
        {

            var teacher = await _teacherService.GetTeacherByUserId(teacherId);
            if (teacher.TeacherAvailabilities.Any(t => t.StartTime <= lesson.StartTime
            && t.EndTime >= lesson.EndTime) && teacher.Gender == lesson.Gender &&
            teacher.TeachersToSubjects.Any(t => t.SubjectId == lesson.SubjectId)) ;
            {
                teacher.Lessons.Add(lesson);
                await _teacherService.UpdateTeacher(teacher.TeacherId, null);

            }
            throw new MismatchTeacherAndLessonException();

        }
        public async Task<List<Lesson>> GetLessonsByTeacherId(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherByUserId(teacherId);
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

       





    }
}