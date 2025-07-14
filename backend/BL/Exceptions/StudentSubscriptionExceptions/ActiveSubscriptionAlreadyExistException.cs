using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.StudentSubscriptionExceptions
{
    public class ActiveSubscriptionAlreadyExistException:Exception
    {
        public ActiveSubscriptionAlreadyExistException(string message) : base(message) { }
        

        public int? StatusCode { get; } = 409;

    }
}
