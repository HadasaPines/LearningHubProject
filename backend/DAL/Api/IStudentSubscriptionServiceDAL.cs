using DAL.Models;

namespace DAL.Api
{
    public interface IStudentSubscriptionServiceDAL
    {
        Task AddStudentSubscription(StudentSubscription studentSubscription);
        Task DeleteStudentSubscription(int id);
        Task<List<StudentSubscription>> GetAllStudentSubscriptions();
        Task<List<StudentSubscription>> GetStudentSubscriptionById(int id);
        Task UpdateLessonsUsed(StudentSubscription studentSubscription);
    }
}