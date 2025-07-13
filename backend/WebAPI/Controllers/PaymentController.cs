using BL.Api;
using BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentServiceBL _paymentServiceBL;

        public PaymentController(IPaymentServiceBL paymentServiceBL)
        {
            _paymentServiceBL = paymentServiceBL;
        }

        [HttpGet("getAllPayments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentServiceBL.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("getPaymentById/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _paymentServiceBL.GetPaymentById(id);

            return Ok(payment);
        }

        [HttpPost("addPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentBL paymentBL)
        {

            await _paymentServiceBL.AddPayment(paymentBL);
            return Ok("Payment added successfully.");
        }

        [HttpDelete("deletePayment/{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            await _paymentServiceBL.DeletePayment(id);
            return Ok("Payment deleted successfully.");
        }

        [HttpGet("getPaymentsByStudentId/{studentId}")]
        public async Task<IActionResult> GetPaymentsByStudentId(int studentId)
        {
            var payments = await _paymentServiceBL.GetPaymentsByStudentId(studentId);
            return Ok(payments);



        }



    }
}
