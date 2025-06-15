using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException() : base($"Invalid email adress")
        {

        }
        public int? StatusCode { get; } = 440;
    }

}

