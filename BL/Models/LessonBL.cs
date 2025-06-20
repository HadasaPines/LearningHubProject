using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class LessonBL
    {
        public int LessonId { get; set; }

        public int TeacherId { get; set; }

        public int SubjectId { get; set; }

        public DateOnly LessonDate { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public int? MinAge { get; set; }

        public int? MaxAge { get; set; }

        public string Gender { get; set; } = null!;


        public string Status { get; set; } = null!;
    }
}
