using AutoMapper;
using BL.Api;
using BL.Exceptions.PaymentExceptions;
using BL.Models;
using DAL.Api;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class PaymentServiceBL : IPaymentServiceBL
    {
        IPaymentServiceDAL _paymentServiceDAL;
        IMapper _mapper;
        public PaymentServiceBL(IPaymentServiceDAL paymentServiceDAL, IMapper mapper)
        {
            _paymentServiceDAL = paymentServiceDAL;
            _mapper = mapper;
        }
        public async Task AddPayment(PaymentBL paymentBL)
        {
            if (paymentBL == null)
                throw new ArgumentNullException(nameof(paymentBL), "Payment cannot be null");
            var payment = _mapper.Map<Payment>(paymentBL);
            await _paymentServiceDAL.AddPayment(payment);
        }
        public async Task<List<PaymentBL>> GetAllPayments()
        {
            var payments = await _paymentServiceDAL.GetAllPayments();
            if (payments == null || !payments.Any())
                throw new PaymentNotFoundException("No payments found");
            return _mapper.Map<List<PaymentBL>>(payments);
        }
        public async Task<PaymentBL> GetPaymentById(int paymentId)
        {
            var payment = await _paymentServiceDAL.GetPaymentById(paymentId);
            if (payment == null)
                throw new PaymentNotFoundException($"Payment with ID '{paymentId}' not found");
            return _mapper.Map<PaymentBL>(payment);
        }

        public async Task DeletePayment(int paymentId)
        {
            var payment = await _paymentServiceDAL.GetPaymentById(paymentId);
            if (payment == null)
                throw new PaymentNotFoundException($"Payment with ID '{paymentId}' not found");
            await _paymentServiceDAL.DeletePayment(paymentId);
        }
        public async Task<List<PaymentBL>> GetPaymentsByStudentId(int studentId)
        {
            var payments = await _paymentServiceDAL.GetPaymentsByStudentId(studentId);
            if (payments == null || !payments.Any())
                return new List<PaymentBL>();
            return _mapper.Map<List<PaymentBL>>(payments);

        }

    }
}
