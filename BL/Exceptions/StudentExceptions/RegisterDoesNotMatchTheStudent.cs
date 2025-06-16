using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.StudentExceptions
{
    internal class RegisterDoesNotMatchTheStudent: Exception
    {
        public RegisterDoesNotMatchTheStudent() : base($"The student's ID does not match the student's ID in this registration.")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}
