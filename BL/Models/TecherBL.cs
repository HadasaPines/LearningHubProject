using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class TecherBL
    {
        public int TeacherId { get; set; }

        public string? Bio { get; set; }

        public string Gender { get; set; } = null!;

        public DateOnly? BirthDate { get; set; }

       


    }
}
