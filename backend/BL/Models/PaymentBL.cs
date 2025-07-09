using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
   public class PaymentBL
    {
        public int PaymentId { get; set; }

        public int UserId { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PaymentDate { get; set; }
    }
}
