


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Api;
using DAL.Contexts;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
namespace DAL.Services
{
    public class SubjectServiceDAL : ISubjectServiceDAL
    {
        private readonly LearningHubDbContext _context;
        public SubjectServiceDAL(LearningHubDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<Subject>> GetAllSubjects()
        {
            return await _context.Subjects.ToListAsync();
        }
        public async Task<Subject> GetSubjectByName(string name)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.Equals(name));
            if (subject == null)
            {
                throw new Exception($"Subject with Name {name} not found.");
            }
            return subject;
        }
        public async Task AddSubject(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSubjectByName(string name)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.Equals(name));
            if (subject == null)
            {
                throw new Exception($"Subject with Name {name} not found.");
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
        }
        //public async Task UpdateSubject(Subject subject)
        //{
        //    var existingSubject = _context.Subjects.FirstOrDefault(s => s.SubjectId == subject.SubjectId);
        //    if (existingSubject != null)
        //    {
        //        existingSubject.Name = subject.Name;
        //        existingSubject.Description = subject.Description;
        //        _context.SaveChanges();
        //    }
        //}
        //public async Task<List<Subject>> GetSubjectsToTeacherByTeacherName(string teacherName)
        //{
        //    var subjects = await _context.Subjects
        //        .Where(s => s..FullName.Equals(teacherName))
        //        .ToListAsync();
        //}

    }
}

