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
                throw new ArgumentNullException(nameof(TeacherBL), "Teacher cannot be null");
            

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
                throw new ArgumentException("First name and last name cannot be null or empty", nameof(firstName));


            var teacher = await _teacherService.GetTeacherByName(firstName, lastName);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with name {firstName} {lastName} not found");
            return _mapper.Map<TeacherBL>(teacher);
        }

        public async Task<TeacherBL?> GetTeacherById(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Teacher ID must be a positive integer", nameof(teacherId));

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");
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
            if (id <= 0)
                throw new ArgumentException("Teacher ID must be a positive integer", nameof(id));
            if (patchDoc == null)
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            var teacher = await _teacherService.GetTeacherById(id);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {id} not found\"");
            var techerBL = _mapper.Map<TeacherBL>(teacher);
            patchDoc.ApplyTo(techerBL);
            var updateTeacher = _mapper.Map<Teacher>(techerBL);
            await _teacherService.UpdateTeacher( updateTeacher);
            return techerBL;
        }

        public async Task AddLessonToTeacher(int teacherId, LessonBL lessonBL)
        {
            if (lessonBL == null)
                throw new ArgumentNullException(nameof(lessonBL), "Lesson cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");
            if (teacher.TeacherAvailabilities.Any(t => t.StartTime <= lessonBL.StartTime
            && t.EndTime >= lessonBL.EndTime) && teacher.Gender == lessonBL.Gender &&
            teacher.TeachersToSubjects.Any(t => t.SubjectId == lessonBL.SubjectId))
            {
                var lesson = _mapper.Map<Lesson>(lessonBL);
                teacher.Lessons.Add(lesson);
                await _teacherService.UpdateTeacher(teacher);

            }
            else throw new MismatchTeacherAndLessonException("Mismatch between teacher and lesson the teacher can't teach this lesson");

        }
        public async Task<List<LessonBL>> GetLessonsByTeacherId(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");
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
                throw new ArgumentNullException(nameof(availabilityBL), "Teacher availability cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");


            if (teacherId != availabilityBL.TeacherId)
            {
                throw new MismatchTeacherAndAvailabilityException($"Mismatch between teacher and availability");
            }
            var availability = _mapper.Map<TeacherAvailability>(availabilityBL);
            teacher.TeacherAvailabilities.Add(availability);
            await _teacherService.UpdateTeacher(teacher);





        }

        public async Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilities(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");

            return _mapper.Map<List<TeacherAvailabilityBL>>(teacher.TeacherAvailabilities);
        }

        public async Task AddSubjectToTeacher(int teacherId, TeachersToSubjectBL teachersToSubjectBL)
        {
            if (teachersToSubjectBL == null)
                throw new ArgumentNullException("TeachersToSubject cannot be null");

            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");

            if (teacherId != teachersToSubjectBL.TeacherId)
            {
                throw new MismatchTeacherAndSubjectException("$Mismatch between teacher and subject ,the teacher don't teach this subject");
            }
            var teachersToSubject = _mapper.Map<TeachersToSubject>(teachersToSubjectBL);
            teacher.TeachersToSubjects.Add(teachersToSubject);
            await _teacherService.UpdateTeacher( teacher);
        }

        public async Task<List<TeachersToSubjectBL>> GetTeachersSubjects(int teacherId)
        {
            var teacher = await _teacherService.GetTeacherById(teacherId);
            if (teacher == null)
                throw new UserNotFoundException($"Teacher with ID {teacherId} not found");

            return _mapper.Map<List<TeachersToSubjectBL>>(teacher.TeachersToSubjects);
        }






    }
}
