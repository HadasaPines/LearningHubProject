using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class TeacherAvailabilityBL
    {
        public int AvailabilityId { get; set; }
        public int TeacherId { get; set; }

        public int WeekDay { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }
    }
}
