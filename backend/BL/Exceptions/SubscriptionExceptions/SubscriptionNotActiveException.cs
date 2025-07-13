using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.SubscriptionExceptions
{
    public class SubscriptionNotActiveException : Exception

    {
        public SubscriptionNotActiveException(string massege) : base(massege) { }

        public int? StatusCode { get; } = 400;


    }
}
