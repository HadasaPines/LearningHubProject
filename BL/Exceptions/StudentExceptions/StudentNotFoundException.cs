using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.StudentExceptions
{
    public class StudentNotFoundException : Exception
    {

        public StudentNotFoundException(string messege) : base(messege)
        {

        }
        public int? StatusCode { get; } = 445;

    }
}
