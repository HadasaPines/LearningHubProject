using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class SiteReviewBL
    {
        public int ReviewId { get; set; }

        [StringLength(100)]
        public string StudentName { get; set; } = null!;

        public string Text { get; set; } = null!;

        public int Rating { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ReviewDate { get; set; }

    }
}
