using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserIncludeRoleBL
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Role { get; set; } = null!;
        public virtual StudentBL? Student { get; set; }

        public virtual TeacherBL? Teacher { get; set; }
    }
}
