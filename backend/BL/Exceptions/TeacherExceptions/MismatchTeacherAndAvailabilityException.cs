using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.TeacherExceptions
{
   public class MismatchTeacherAndAvailabilityException:Exception
    {
        public MismatchTeacherAndAvailabilityException(string massage) : base(massage)
        {

        }
        public int? StatusCode { get; } = 403;
    }
}
