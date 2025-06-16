using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class StudentBL
    {
        public int StudentId { get; set; }

        public string Gender { get; set; } = null!;

        public DateOnly BirthDate { get; set; }


        public int? Age { get; set; }

        //public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

        //public virtual User StudentNavigation { get; set; } = null!;
    }
}
