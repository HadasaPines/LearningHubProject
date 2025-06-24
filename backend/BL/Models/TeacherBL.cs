using BL.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class TeacherBL
    {
        [Required(ErrorMessage = "Teacher ID is required")]
        [IsraeliId(ErrorMessage = "Invalid Israeli ID number")]
        public int TeacherId { get; set; } 

        public string? Bio { get; set; }
        [RegularExpression("^(M|F)$", ErrorMessage = "Gender must be M or F")]
        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; } = null!;

        [DataType(DataType.Date)]
        [BirthDateInPast(ErrorMessage = "BirthDate must be in the past")]
        public DateOnly? BirthDate { get; set; }

    }
}
