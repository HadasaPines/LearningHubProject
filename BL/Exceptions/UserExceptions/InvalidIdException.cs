using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base($"Invalid ID number")
        {

        }
        public int? StatusCode { get; } = 440;
    }

}

