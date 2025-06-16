using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.TeacherExceptions
{
    public class MismatchTeacherAndSubjectException : Exception
    {
        public MismatchTeacherAndSubjectException() : base($"Mismatch between teacher and subject")
        {

        }
        public int? StatusCode { get; } = 450;
    }
}
