﻿using AutoMapper;
using DAL.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Services;
using BL.Models;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Internal;
using BL.Api;
using Microsoft.EntityFrameworkCore;
using BL.Exceptions.SubjectExceptions;

namespace BL.Services
{
    public class SubjectServiceBL : ISubjectServiceBL
    {
        private readonly ISubjectServiceDAL _subjectServiceDAL;
        private readonly IMapper _mapper;


        public SubjectServiceBL(ISubjectServiceDAL subjectServiceDAL, IMapper mapper)
        {
            _subjectServiceDAL = subjectServiceDAL;
            _mapper = mapper;
        }

        public async Task<List<SubjectBL>> GetAllSubjects()
        {
            var subjects = await _subjectServiceDAL.GetAllSubjects();
            return _mapper.Map<List<SubjectBL>>(subjects);
        }

        public async Task<SubjectBL> GetSubjectByName(string name)
        {
            var subject = await _subjectServiceDAL.GetSubjectByName(name);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with name '{name}' not found");
            return _mapper.Map<SubjectBL>(subject);
        }
        public async Task<SubjectBL> GetSubjectById(int id)
        {
            var subject = await _subjectServiceDAL.GetSubjectById(id);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with ID '{id}' not found");
            return _mapper.Map<SubjectBL>(subject);
        }
        public async Task AddSubject(SubjectBL subjectBL)
        {
            if (subjectBL == null)
                throw new ArgumentNullException(nameof(subjectBL), "Subject cannot be null");

            var subject = _mapper.Map<Subject>(subjectBL);
            await _subjectServiceDAL.AddSubject(subject);
        }
        public async Task DeleteSubjectByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Subject name cannot be null or empty", nameof(name));

            var subject = await _subjectServiceDAL.GetSubjectByName(name);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with name '{name}' not found");
            await _subjectServiceDAL.DeleteSubjectByName(name);
        }
       
        public async Task UpdateSubject(int id, JsonPatchDocument<SubjectBL> patchDoc)
        {
            if (patchDoc == null)
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            var subject = await _subjectServiceDAL.GetSubjectById(id);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with ID '{id}' not found");
            var subjectBL = _mapper.Map<SubjectBL>(subject);
            patchDoc.ApplyTo(subjectBL);
            var subjectToUpdate = _mapper.Map<Subject>(subjectBL);
            await _subjectServiceDAL.UpdateSubject(subjectToUpdate);
        }

        public async Task<List<TeacherBL>> GetTeachersBySubjectName(string name)
        {
            var subject = await _subjectServiceDAL.GetSubjectByName(name);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with name '{name}' not found");
            var teachers = await _subjectServiceDAL.GetTeachersBySubjectName(name);
            return _mapper.Map<List<TeacherBL>>(teachers);
        }

        public async Task<List<LessonBL>> GetLessonsBySubjectName(string name)
        {
            var subject = await _subjectServiceDAL.GetSubjectByName(name);
            if (subject == null)
                throw new SubjectNotFoundException($"Subject with name '{name}' not found");
            var lessons = await _subjectServiceDAL.GetLessonsBySubjectName(name);
            return _mapper.Map<List<LessonBL>>(lessons);



        }



    }
}
