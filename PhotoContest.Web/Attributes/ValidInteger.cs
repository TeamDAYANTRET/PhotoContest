using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoContest.Web.Attributes
{
    public class ValidInteger : ValidationAttribute
    {
        public int Max { get; set; }

        public int Min { get; set; }

        public ValidInteger(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            int number = (int)value;
            if (number >= this.Min && number <= this.Max)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format("The value shhould be >= {0} and <= {1}", this.Min, this.Max));
        }        
    }
}