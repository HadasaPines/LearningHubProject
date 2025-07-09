using BL.Models;

namespace BL.Api
{
    public interface IStudentSubscriptionServiceBL
    {
        Task AddStudentSubscription(StudentSubscriptionBL studentSubscriptionBL);
        Task DeleteStudentSubscription(int id);
        Task<List<StudentSubscriptionBL>> GetAllStudentSubscriptions();
        Task<StudentSubscriptionBL> GetStudentSubscriptionById(int id);
        Task<List<StudentSubscriptionBL>> GetStudentSubscriptionsByStudentId(int studentId);
        Task UpdateLessonsUsed(int id);
    }
}