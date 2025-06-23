using AutoMapper;
using BL.Api;
using BL.Exceptions.LessonExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class LessonServiceBL : ILessonServiceBL
    {
        private readonly ILessonServiceDAL _lessonServiceDAL;
        private readonly IMapper _mapper;
        public LessonServiceBL(ILessonServiceDAL lessonServiceDAL, IMapper mapper)
        {
            _lessonServiceDAL = lessonServiceDAL;
            _mapper = mapper;
        }
        public async Task<List<LessonBL>> GetAllLessons()
        {
            return _mapper.Map<List<LessonBL>>(await _lessonServiceDAL.GetAllLessons());
        }

        public async Task<LessonBL> GetLessonById(int lessonId)
        {
            var lesson = await _lessonServiceDAL.GetLessonById(lessonId);
            if (lesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {lessonId} not found");
            }
            return _mapper.Map<LessonBL>(lesson);
        }

        public async Task AddLesson(LessonBL lessonBL)
        {
            if (lessonBL == null)
            {
                throw new ArgumentNullException(nameof(lessonBL), "Lesson cannot be null");
            }
            await _lessonServiceDAL.AddLesson(_mapper.Map<Lesson>(lessonBL));
        }
        public async Task UpdateLesson(int id, JsonPatchDocument<LessonBL> patchDoc)
        {
            if (patchDoc == null)
            {
                throw new ArgumentNullException(nameof(patchDoc), "Patch document cannot be null");
            }
            var existingLesson = await _lessonServiceDAL.GetLessonById(id);
            if (existingLesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {id} not found");
            }
            var lessonBL = _mapper.Map<LessonBL>(existingLesson);
            patchDoc.ApplyTo(lessonBL);
            var updatedLesson = _mapper.Map<Lesson>(lessonBL);
            await _lessonServiceDAL.UpdateLesson(updatedLesson);

        }
        public async Task DeleteLesson(int id)
        {
            var existingLesson = await _lessonServiceDAL.GetLessonById(id);
            if (existingLesson == null)
            {
                throw new LessonNotFoundException($"Lesson with ID {id} not found");
            }
            await _lessonServiceDAL.DeleteLessonById(id);
        }


        public async Task<List<LessonBL>> GetLessonsByDetails(LessonFilterDto filter)
        {
            IEnumerable<Lesson> lessons = await _lessonServiceDAL.GetAllLessonsIncludeDetails();
            var query = lessons.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Gender))
                query = query.Where(l => l.Gender == filter.Gender);

            if (filter.age.HasValue)
            {
                query = query.Where(l =>
                    (!l.MinAge.HasValue || l.MinAge <= filter.age) &&
                    (!l.MaxAge.HasValue || l.MaxAge >= filter.age));
            }

            if (filter.SpecificDate.HasValue)
                query = query.Where(l => l.LessonDate == filter.SpecificDate);

            if (filter.DateFrom.HasValue)
                query = query.Where(l => l.LessonDate >= filter.DateFrom);

            if (filter.DateTo.HasValue)
                query = query.Where(l => l.LessonDate <= filter.DateTo);

            if (filter.StartTime.HasValue)
                query = query.Where(l => l.StartTime >= filter.StartTime);

            if (filter.EndTime.HasValue)
                query = query.Where(l => l.EndTime <= filter.EndTime);

            if (!string.IsNullOrEmpty(filter.Status))
                query = query.Where(l => l.Status == filter.Status);

            if (filter.SubjectId.HasValue)
                query = query.Where(l => l.SubjectId == filter.SubjectId);

            if (filter.TeacherId.HasValue)
                query = query.Where(l => l.TeacherId == filter.TeacherId);

            return _mapper.Map<List<LessonBL>>(query);
        }


    }
}
