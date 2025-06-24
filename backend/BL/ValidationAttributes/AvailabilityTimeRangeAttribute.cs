using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;

namespace BL.ValidationAttributes
{
 

    public class AvailabilityTimeRangeAttribute : ValidationAttribute
    {
        private readonly TimeOnly _earliest = new(9, 0);  // 09:00
        private readonly TimeOnly _latest = new(21, 0);   // 21:00

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dto = validationContext.ObjectInstance;
            var startTimeProp = validationContext.ObjectType.GetProperty("StartTime");
            var endTimeProp = validationContext.ObjectType.GetProperty("EndTime");

            if (startTimeProp == null || endTimeProp == null)
                return new ValidationResult("StartTime or EndTime property not found.");

            var start = (TimeOnly?)startTimeProp.GetValue(dto);
            var end = (TimeOnly?)endTimeProp.GetValue(dto);

            if (start == null || end == null)
                return new ValidationResult("StartTime and EndTime must be provided.");

            if (start < _earliest || start > _latest)
                return new ValidationResult("StartTime must be between 09:00 and 21:00");

            if (end < _earliest || end > _latest)
                return new ValidationResult("EndTime must be between 09:00 and 21:00");

            if (end <= start)
                return new ValidationResult("EndTime must be after StartTime");

            return ValidationResult.Success;
        }
    }

}
