using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
   public class StudentSubscriptionBL
    {
        public int StudentSubscriptionId { get; set; }

        public int StudentId { get; set; }

        public int SubscriptionId { get; set; }

        public int LessonsUsed { get; set; }

        public bool IsActive { get; set; }
    }
}
