using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.StudentSubscriptionExceptoins
{
   public class UnActiveStudentSubscriptionException:Exception
    {

        public UnActiveStudentSubscriptionException(string message) : base(message) { }

        public int? StatusCode { get; } = 400; 

    }
}
