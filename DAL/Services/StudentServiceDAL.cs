using DAL.Api;
using DAL.Contexts;
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
        private readonly LearningHubDbContext dbContext;

        public StudentServiceDAL(LearningHubDbContext context)
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

        public async Task<Student> GetStudentByName(string firstName, string lastName)
        {
            var student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.StudentNavigation.FirstName == firstName && s.StudentNavigation.LastName == lastName);
            return student;

        }

        public async Task AddStudent(Student student)
        {
            dbContext.Students.Add(student);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteStudent(int studentId)
        {
            var student = await GetStudentById(studentId);
            dbContext.Students.Remove(student);


        }

        public async Task UpdateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Student cannot be null");
            }

            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();
        }
    }
   
    }
