using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.SubscriptionExceptions
{
   public class SubscriptionNotFoundException:Exception
    {
  
        public SubscriptionNotFoundException(string message):base(message) { }
        public int? StatusCode { get; } = 404;
    }
}
