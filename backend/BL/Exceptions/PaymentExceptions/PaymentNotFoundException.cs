using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.PaymentExceptions
{
   public class PaymentNotFoundException: Exception
    {
     
        public PaymentNotFoundException(string message) :base(message){ }
        public int? StatusCode { get; } = 404;
    }
}
