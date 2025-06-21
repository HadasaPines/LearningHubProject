using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Exceptions.LessonExceptions
{
    public class LessonNotFoundException : Exception
    {
        public LessonNotFoundException(string messege) : base(messege)
        {

        }
        public int? StatusCode { get; } = 404;
    }
}
