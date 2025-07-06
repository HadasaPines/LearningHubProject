using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.SubjectExceptions
{
    public class CanNotDeleteSubject: Exception
    {
        public CanNotDeleteSubject(string message) : base(message)
        {

        }
        public int? StatusCode { get; } = 409;
    }
}
