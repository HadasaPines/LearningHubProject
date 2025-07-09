using DAL.Contexts;
using DAL.Models;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class PaymentServiceDAL : IPaymentServiceDAL
    {
        private readonly LearningHubDbContext dbContext;

        public PaymentServiceDAL(LearningHubDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            return await dbContext.Payments.ToListAsync();
        }
        public async Task<Payment> GetPaymentById(int paymentId)
        {
            return await dbContext.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
        }
        public async Task AddPayment(Payment payment)
        {
            await dbContext.Payments.AddAsync(payment);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdatePayment(Payment payment)
        {
            var existingPayment = await dbContext.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == payment.PaymentId);
            if (existingPayment != null)
            {
                dbContext.Entry(existingPayment).CurrentValues.SetValues(payment);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task DeletePayment(int paymentId)
        {
            var payment = await dbContext.Payments
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment != null)
            {
                dbContext.Payments.Remove(payment);
                await dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<Payment>> GetPaymentsByStudentId(int studentId)
        {
            return await dbContext.Payments
                .Where(p => p.UserId == studentId)
                .Include(p => p.User)
                .Include(p => p.User.Student)
                .ToListAsync();
        }

    }
}
