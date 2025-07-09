using DAL.Models;

namespace DAL.Api
{
    public interface IPaymentServiceDAL
    {
        Task AddPayment(Payment payment);
        Task DeletePayment(int paymentId);
        Task<List<Payment>> GetAllPayments();
        Task<Payment> GetPaymentById(int paymentId);
        Task<List<Payment>> GetPaymentsByStudentId(int studentId);
 
    }
}