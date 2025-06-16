using AutoMapper;
using BL.Api;
using BL.Exceptions.StudentExceptions;
using BL.Exceptions.UserExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    internal class StudentServiceBL : IStudentServiceBL
    {
        private readonly IStudentServiceDAL _studentServiceDAL;
        private readonly IMapper _mapper;

        public StudentServiceBL(IStudentServiceDAL studentServiceDAL, IMapper mapper)

        {
            _mapper = mapper;
            _studentServiceDAL = studentServiceDAL;
        }

        public async Task<List<StudentBL>> GetAllStudents()
        {
            var students = await _studentServiceDAL.GetAllStudents();
            return _mapper.Map<List<StudentBL>>(students);

        }
        public async Task<StudentBL> GetStudentById(int studentId)
        {
            var student = await _studentServiceDAL.GetStudentById(studentId);
            if (student == null)
            {
                throw new Exception($"Student with ID {studentId} not found.");
            }
            return _mapper.Map<StudentBL>(student);
        }
        public async Task<StudentBL> GetStudentByName(string firstName, string lastName)
        {
            var student = await _studentServiceDAL.GetStudentByName(firstName, lastName);
            if (student == null)
            {
                throw new Exception($"Student with Name {firstName} {lastName} not found.");
            }
            return _mapper.Map<StudentBL>(student);
        }
        public async Task AddStudent(StudentBL studentBL)
        {
            var student = _mapper.Map<Student>(studentBL);
            await _studentServiceDAL.AddStudent(student);
        }
        public async Task DeleteStudent(int studentId)
        {
            var student = await _studentServiceDAL.GetStudentById(studentId);
            if (student == null)
            {
                throw new Exception($"Student with ID {studentId} not found.");
            }
            await _studentServiceDAL.DeleteStudent(studentId);
        }
        public async Task DeleteStudent(StudentBL studentBL)
        {
            if (studentBL == null)
            {
                throw new ArgumentNullException(nameof(studentBL), "Student cannot be null");
            }
            var student = _mapper.Map<Student>(studentBL);
            await _studentServiceDAL.DeleteStudent(student.StudentId);
        }
        public async Task<StudentBL> UpdateStudent(int studentId, JsonPatchDocument<StudentBL> patchDoc)
        {
            var student = await _studentServiceDAL.GetStudentById(studentId);
            var studentBL = _mapper.Map<StudentBL>(student);
            patchDoc.ApplyTo(studentBL);
            var updatedStudent = _mapper.Map<Student>(studentBL);
            await _studentServiceDAL.UpdateStudent(updatedStudent);
            return studentBL;
        }
        public async Task<RegistrationBL> AddRegistrationToStudent(int studentId, RegistrationBL registrationBL)
        {
            if (registrationBL == null)
            {
                throw new ArgumentNullException(nameof(registrationBL), "Registration cannot be null");
            }
            if (registrationBL.StudentId != studentId)
            {
                throw new RegisterDoesNotMatchTheStudent($"Registration StudentId {registrationBL.StudentId} does not match the provided StudentId {studentId}.");
            }
            var student = await _studentServiceDAL.GetStudentById(studentId);
            if (student == null)
            {
                throw new StudentNotFoundException($"Student with ID {studentId} not found.");
            }
            var registration = _mapper.Map<Registration>(registrationBL);
            registration.Student = student;
            await _studentServiceDAL.AddRegistrationToStudent(registration);
            return _mapper.Map<RegistrationBL>(registration);
        }
    }

}
