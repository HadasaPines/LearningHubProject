using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions
{
    public class MismatchTeacherAndLessonException:Exception
    {
        public MismatchTeacherAndLessonException() : base($"Mismatch between teacher and class")
        {

        }
        public int? StatusCode { get; } = 440;
    }
}
