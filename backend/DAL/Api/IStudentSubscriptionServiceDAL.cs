using DAL.Models;

namespace DAL.Api
{
    public interface IStudentSubscriptionServiceDAL
    {
        Task AddStudentSubscription(StudentSubscription studentSubscription);
        Task DeleteStudentSubscription(int id);
        Task<List<StudentSubscription>> GetAllStudentSubscriptions();
        Task<StudentSubscription> GetStudentSubscriptionById(int id);
        Task UpdateLessonsUsed(StudentSubscription studentSubscription);

        Task<List<StudentSubscription>> GetStudentSubscriptionsByStudentId(int studentId);
    }
}