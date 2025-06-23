using DAL.Models;

namespace DAL.Api
{
    public interface ITeacherAvailabilityServiceDAL
    {
        Task AddTeacherAvailability(TeacherAvailability teacherAvailability);
        Task DeleteTeacherAvailability(TeacherAvailability teacherAvailability);
        Task<List<TeacherAvailability>> GetAllTeacherAvailabilities();
        Task<TeacherAvailability> GetTeacherAvailabilitiesById(int id);
        Task<List<TeacherAvailability>> GetTeacherAvailabilitiesByTimeRange(TimeOnly startTime, TimeOnly endTime);
        Task<List<TeacherAvailability>> GetTeacherAvailabilitiesByWeekDay(int weekDay);
        Task<TeacherAvailability> UpdateTeacherAvailability(TeacherAvailability teacherAvailability);
    }
}