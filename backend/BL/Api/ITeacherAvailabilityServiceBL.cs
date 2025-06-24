using BL.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BL.Api
{
    public interface ITeacherAvailabilityServiceBL
    {
        Task AddTeacherAvailability(TeacherAvailabilityBL teacherAvailabilityBL);
        Task DeleteTeacherAvailability(int id);
        Task<List<TeacherAvailabilityBL>> GetAllTeacherAvailabilities();
        Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByTimeRange(TimeOnly startTime, TimeOnly endTime);
        Task<List<TeacherAvailabilityBL>> GetTeacherAvailabilitiesByWeekDay(int weekDay);
        Task<TeacherAvailabilityBL> UpdateTeacherAvailability(int id, JsonPatchDocument<TeacherAvailabilityBL> patchDoc);
    }
}