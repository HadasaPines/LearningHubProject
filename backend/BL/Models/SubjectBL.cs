using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class SubjectBL
    {

        //public int SubjectId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be up to 50 characters long")]
        [RegularExpression(@"^[A-Za-z\s\-']+$", ErrorMessage = "Name can contain only letters, spaces, hyphens and apostrophes")]
        public string Name { get; set; } = null!;
    }
}
