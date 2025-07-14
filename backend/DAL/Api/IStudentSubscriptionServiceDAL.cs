using DAL.Models;

namespace DAL.Api
{
    public interface IStudentSubscriptionServiceDAL
    {
        Task AddStudentSubscription(StudentSubscription studentSubscription);
        Task DeleteStudentSubscription(int id);
        Task<List<StudentSubscription>> GetAllStudentSubscriptions();
        Task<StudentSubscription> GetStudentSubscriptionById(int id);
        Task UpdateLessonsUsedForActiveStudentSubscription(StudentSubscription studentSubscription);
        Task<StudentSubscription> GetActiveStudentSubscriptionsByStudentId(int studentId);
        Task<List<StudentSubscription>> GetStudentSubscriptionsByStudentId(int studentId);
    }
}