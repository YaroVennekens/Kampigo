using System;
using System.ComponentModel.DataAnnotations;

namespace Ziekenfonds_Kampigo_Project.ViewModels.Custom_Validators
{
    public class NotInThePastAttribute : ValidationAttribute
    {
        public NotInThePastAttribute()
            : base("The date cannot be in the past.") { }

        public override bool IsValid(object? value)
        {
            if (value is DateTime dateValue)
            {

                return dateValue.Date >= DateTime.Now.Date;
            }
            return true;
        }
    }
}
