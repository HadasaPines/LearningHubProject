


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

            return subject;
        }
        public async Task<Subject> GetSubjectById(int id)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.SubjectId == id);

            return subject;
        }
        public async Task AddSubject(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSubject(Subject subject)
        {
            var existingSubject = await _context.Subjects.FindAsync(subject.SubjectId);
            existingSubject = subject;
            _context.Subjects.Update(existingSubject);
           await  _context.SaveChangesAsync();
        }
        public async Task DeleteSubjectByName(string name)
        {
            var subject = await _context.Subjects
               .Include(s => s.TeachersToSubjects)
               .Include(l=>l.Lessons)
               .FirstOrDefaultAsync(s => s.Name == name);


            _context.TeachersToSubjects.RemoveRange(subject.TeachersToSubjects);
            _context.Lessons.RemoveRange(subject.Lessons);
            _context.Subjects.Remove(subject);

            await _context.SaveChangesAsync();
        }

      

        public async Task<List<Teacher>> GetTeachersBySubjectName(string name)
        {
            var subject = await _context.Subjects
                    .Include(s => s.TeachersToSubjects)
                    .ThenInclude(ts => ts.Teacher)
                    .FirstOrDefaultAsync(s => s.Name == name);
            return subject.TeachersToSubjects.Select(ts=>ts.Teacher).ToList();
        }

        public async Task<List<Lesson>> GetLessonsBySubjectName(string name)
        {
            var subject = await _context.Subjects
                .Include(s => s.Lessons)
                .FirstOrDefaultAsync(s => s.Name==name);

            return subject.Lessons.ToList();
        }


      

        }
}
