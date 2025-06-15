using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.UserExceptions
{
    public class RequiredFieldsNotFilledException:Exception
    {
        public RequiredFieldsNotFilledException() : base($"Not all required fields have been filled in.")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}
