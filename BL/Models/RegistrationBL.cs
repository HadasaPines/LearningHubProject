using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class RegistrationBL
    {
        public int RegistrationId { get; set; }

        public int LessonId { get; set; }

        public int StudentId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public virtual Lesson Lesson { get; set; } = null!;

        public virtual Student Student { get; set; } = null!;
    }
}
