using BL.ValidationAttributes;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class StudentBL
    {



        [Key]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^(M|F)$", ErrorMessage = "Gender must be M or F")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "BirthDate is required")]
        [DataType(DataType.Date)]
        [BirthDateInPast(ErrorMessage = "BirthDate must be in the past")]
        public DateOnly BirthDate { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int? Age { get; set; }
    }


    //public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    //public virtual User StudentNavigation { get; set; } = null!;

}
