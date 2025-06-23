using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using BL.Models;

namespace BL.ValidationAttributes
{
   

    public class LessonTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var lesson = validationContext.ObjectInstance as LessonBL;
            if (lesson == null)
                return ValidationResult.Success;

            // תאריך עבר
            if (lesson.LessonDate < DateOnly.FromDateTime(DateTime.Today))
            {
                return new ValidationResult("LessonDate cannot be in the past.");
            }

            // זמן סיום לפני זמן התחלה
            if (lesson.EndTime <= lesson.StartTime)
            {
                return new ValidationResult("EndTime must be after StartTime.");
            }

            return ValidationResult.Success;
        }
    }

}
