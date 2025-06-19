using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BL.ValidationAttributes
{
  

    public class IsraeliIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string id = value.ToString()!.PadLeft(9, '0');

            if (!Regex.IsMatch(id, @"^\d{9}$"))
                return new ValidationResult("ID number must be exactly 9 digits.");

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                int digit = int.Parse(id[i].ToString());
                int mult = (i % 2 == 0) ? digit : digit * 2;
                if (mult > 9) mult -= 9;
                sum += mult;
            }

            if (sum % 10 == 0)
                return ValidationResult.Success;

            return new ValidationResult("Invalid Israeli ID number.");
        }
    }

}
