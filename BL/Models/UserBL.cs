using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BL.Models
{
    public class UserBL
    {
        public int UserId { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [MaxLength(10), MinLength(10)]

        public string Phone { get; set; } = null!;
        //Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")

        public string Email { get; set; } = null!;
        // Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.

        //var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = null!;
    }
}
