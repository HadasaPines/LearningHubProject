using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class RegistrationBL
    {
        [Required(ErrorMessage = "RegistrationId is required")]
        public int RegistrationId { get; set; }

        [Required(ErrorMessage = "LessonId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "LessonId must be a positive number")]
        public int LessonId { get; set; }

        [Required(ErrorMessage = "StudentId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "StudentId must be a positive number")]
        public int StudentId { get; set; }


        public DateTime RegistrationDate { get; set; } = DateTime.Now;

    }
}
