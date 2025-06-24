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
    public class TeacherAvailabilityServiceDAL : ITeacherAvailabilityServiceDAL
    {
        private readonly LearningHubDbContext dbContext;
        public TeacherAvailabilityServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;

        }
        public async Task AddTeacherAvailability(TeacherAvailability teacherAvailability)
        {
            await dbContext.TeacherAvailabilities.AddAsync(teacherAvailability);
            await dbContext.SaveChangesAsync();
        }
        public async Task<TeacherAvailability> UpdateTeacherAvailability(TeacherAvailability teacherAvailability)
        {


            dbContext.TeacherAvailabilities.Update(teacherAvailability);
            await dbContext.SaveChangesAsync();
            return teacherAvailability;

        }

        public async Task DeleteTeacherAvailability(TeacherAvailability teacherAvailability)
        {

            dbContext.TeacherAvailabilities.Remove(teacherAvailability);
            await dbContext.SaveChangesAsync();
        }


        public async Task<List<TeacherAvailability>> GetAllTeacherAvailabilities()
        {
            return await dbContext.TeacherAvailabilities.ToListAsync();
        }

        public async Task<List<TeacherAvailability>> GetTeacherAvailabilitiesByWeekDay(int weekDay)
        {
            return await dbContext.TeacherAvailabilities
                .Where(ta => ta.WeekDay == weekDay)
                .ToListAsync();
        }
        public async Task<List<TeacherAvailability>> GetTeacherAvailabilitiesByTimeRange(TimeOnly startTime, TimeOnly endTime)
        {
            return await dbContext.TeacherAvailabilities
                .Where(ta => ta.StartTime >= startTime && ta.EndTime <= endTime)
                .ToListAsync();



        }
        public async Task<TeacherAvailability> GetTeacherAvailabilitiesById(int id)
        {
            return await dbContext.TeacherAvailabilities
                .FirstOrDefaultAsync(ta => ta.AvailabilityId == id);




        }
    }
}
