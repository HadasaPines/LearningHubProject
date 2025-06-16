using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.TeacherExceptions
{
   public class MismatchTeacherAndAvailabilityException:Exception
    {
        public MismatchTeacherAndAvailabilityException() : base($"Mismatch between teacher and availability")
        {

        }
        public int? StatusCode { get; } = 450;
    }
}
