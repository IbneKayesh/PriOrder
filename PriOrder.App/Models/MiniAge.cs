using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class MiniAge : ValidationAttribute
    {
        private int _minAge;
        private int _maxAge;
        public MiniAge(int minAge, int maxAge)
        { // The constructor which we use in modal.
            this._minAge = minAge;
            this._maxAge = maxAge;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object val = value == null ? DateTime.Today : value;
            DateTime bday = DateTime.Parse(val.ToString());
            DateTime today = DateTime.Today;
            int age = today.Year - bday.Year;
            if (bday > today.AddYears(-age))
            {
                age--;
            }
            if (age < _minAge)
            {
                var result = new ValidationResult("Sorry you are not old enough");
                return result;
            }
            if (age > _maxAge)
            {
                var result = new ValidationResult("Sorry you are outdated");
                return result;
            }
            return null;
        }
    }
}