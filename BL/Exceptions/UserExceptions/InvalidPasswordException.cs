using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(int id) : base($"Invalid password")
        {

        }
        public int? StatusCode { get; } = 440;
    }

}

