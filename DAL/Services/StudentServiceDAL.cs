using DAL.Api;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class StudentServiceDAL : IStudentServiceDAL
    {
        private readonly MyDbContext dbContext;

        public StudentServiceDAL(MyDbContext context)
        {
            dbContext = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            return await dbContext.Students.FindAsync(studentId);
        }

        public async Task<Student> GetStudentByName(string studentName)
        {
            var user = dbContext.Users
                .Where(u => u.FullName == studentName)
                .Select(u => u.Student)
                .FirstOrDefaultAsync();
            return await dbContext.Students
                .FirstOrDefaultAsync(s => s.StudentId == user.Id);
        }

        public async Task AddStudent(Student student)
        {
            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteStudent(int studentId)
        {
            var student = await GetStudentById(studentId);
            if (student == null)
                throw new Exception($"Student with ID {studentId} not found.");
            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

        }
    }
}
