using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.TeacherExceptions
{
    public class MismatchTeacherAndLessonException : Exception
    {
        public MismatchTeacherAndLessonException(string massage) : base(massage)
        {

        }
        public int? StatusCode { get; } = 403;
    }
}
