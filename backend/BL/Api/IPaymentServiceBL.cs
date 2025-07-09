using BL.Models;

namespace BL.Api
{
    public interface IPaymentServiceBL
    {
        Task AddPayment(PaymentBL paymentBL);
        Task DeletePayment(int paymentId);
        Task<List<PaymentBL>> GetAllPayments();
        Task<PaymentBL> GetPaymentById(int paymentId);
        Task<List<PaymentBL>> GetPaymentsByStudentId(int studentId);
    }
}