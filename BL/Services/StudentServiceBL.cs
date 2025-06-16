using AutoMapper;
using BL.Models;
using DAL.Api;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    internal class StudentServiceBL
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
        public async Task UpdateStudent(int studentId, )
        {
            if (studentBL == null)
            {
                throw new ArgumentNullException(nameof(studentBL), "Student cannot be null");
            }
            var student = _mapper.Map<Student>(studentBL);
            await _studentServiceDAL.UpdateStudent(student);
        }
    }

}
