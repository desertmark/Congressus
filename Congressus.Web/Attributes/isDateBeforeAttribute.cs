using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Congressus.Web.Attributes
{
    public class IsDateBeforeAttribute : ValidationAttribute
    {
        public string DateToComparePropertyName { set; get; }
        public IsDateBeforeAttribute(string dateToComparePropertyName)
        {
            DateToComparePropertyName = dateToComparePropertyName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var DateToCompareInfo = validationContext.ObjectType.GetProperty(DateToComparePropertyName);
            DateTime DateToCompare = (DateTime)DateToCompareInfo.GetValue(validationContext.ObjectInstance);

            if (value == null || (DateTime)value<DateToCompare)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);
        }
    }
}