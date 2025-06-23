using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.RegistrationExceptions
{
    public class RegistrationNotFoundException:Exception
    {
        public RegistrationNotFoundException(string messege) : base(messege)
        {

        }
        public int? StatusCode { get; } = 404;
    }
}
