using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class NameIdMismatchException:Exception
    {
        public NameIdMismatchException() : base($"Name does not match ID")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}
