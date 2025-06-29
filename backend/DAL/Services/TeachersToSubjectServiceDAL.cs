﻿using DAL.Api;
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
    public class TeachersToSubjectServiceDAL : ITeachersToSubjectServiceDAL
    {
        private readonly LearningHubDbContext _context;
        public TeachersToSubjectServiceDAL(LearningHubDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<List<TeachersToSubject>> GetAllTeachersToSubjects()
        {
            return await _context.TeachersToSubjects.ToListAsync();
        }
        public async Task<List<Teacher>> GetTeachersToSubjectByTeacherName(string subjectName)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.Equals(subjectName));
            if (subject == null)
            {
                throw new Exception($"Subject with Name {subjectName} not found.");
            }


            var teachersToSubject = await _context.TeachersToSubjects
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .Where(t => t.Subject.SubjectId == subject.SubjectId).ToListAsync();
            if (teachersToSubject == null || !teachersToSubject.Any())
            {
                throw new Exception($"Subject with Name {subjectName} doesnt have teachers assigned.");
            }
            return teachersToSubject.Select(t => t.Teacher).ToList();

        }
        public async Task<List<Subject>> GetSubjectToTeacherByTeacherName(string firstName, string lastName)
        {

            var teacher = await _context.Users.FirstOrDefaultAsync(t => t.FirstName==firstName&&t.LastName==lastName);
            if (teacher == null)
            {
                throw new Exception($"Teacher with Name {firstName}{lastName} not found.");
            }

            var teachersToSubject = await _context.TeachersToSubjects
                .Include(t => t.Teacher)
                .Include(t => t.Subject)
                .Where(t => t.Teacher.TeacherId == teacher.UserId).ToListAsync();
            if (teachersToSubject == null)
            {
                throw new Exception($"Teacher with Name {firstName}{lastName} doesnt have subjects.");
            }

            return teachersToSubject.Select(s => s.Subject).ToList();
        }
        public async Task AddTeacherToSubject(TeachersToSubject teachersToSubject)
        {
            await _context.TeachersToSubjects.AddAsync(teachersToSubject);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTeacherToSubjectByTeacherAndSubjectNam(string firstName, string lastName, string subjectName)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(t=> t.FirstName == firstName && t.LastName == lastName);
            if (teacher == null)
            {
                throw new Exception($"Teacher with Name {firstName}{lastName} not found.");
            }

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name.Equals(subjectName));
            if (subject == null)
            {
                throw new Exception($"Subject with Name {subjectName} not found.");
            }

            var teachersToSubject = await _context.TeachersToSubjects
                .FirstOrDefaultAsync(ts => ts.Teacher.TeacherId == teacher.UserId && ts.Subject.SubjectId == subject.SubjectId);

            if (teachersToSubject == null)
            {
                throw new Exception($"Teacher {firstName}{lastName}  is not assigned to Subject {subjectName}.");
            }

            _context.TeachersToSubjects.Remove(teachersToSubject);
            await _context.SaveChangesAsync();
        }
    }
}
