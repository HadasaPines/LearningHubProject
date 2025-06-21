using BL.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    [LessonTimeValidation]

    public class LessonBL
    {


        [Required(ErrorMessage = "TeacherId is required.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "SubjectId is required.")]
        public int SubjectId { get; set; }

        [Required(ErrorMessage = "LessonDate is required.")]
        public DateOnly LessonDate { get; set; }

        [Required(ErrorMessage = "StartTime is required.")]
        public TimeOnly StartTime { get; set; }

        [Required(ErrorMessage = "EndTime is required.")]
        public TimeOnly EndTime { get; set; }

        [Range(0, 120, ErrorMessage = "MinAge must be between 0 and 120.")]
        public int? MinAge { get; set; }

        [Range(0, 120, ErrorMessage = "MaxAge must be between 0 and 120.")]
        public int? MaxAge { get; set; }

        [RegularExpression("^(M|F)$", ErrorMessage = "Gender must be M or F")]
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; } = null!;



        [RegularExpression("^(Available|Booked|Past|Cancelled)$", ErrorMessage = "Status must be one of the following: Available, Booked, Past, Cancelled")]
        public string Status { get; set; } = null!;

    }
}
