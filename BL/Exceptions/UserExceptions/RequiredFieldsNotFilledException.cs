using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class RequiredFieldsNotFilledException:Exception
    {
        public RequiredFieldsNotFilledException(string message) : base(message)
        {

        }
        public int? StatusCode { get; } = 400;
    }
}
