using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class WrongPasswordException : Exception
    {
        public WrongPasswordException(int id) : base($"Wrong password")
        {

        }
        public int? StatusCode { get; } = 440;
    }

}

