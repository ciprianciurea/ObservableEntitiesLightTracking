using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithMixedValidation : ObservableObject, IValidatableObject, IValidatableObjectWithSeverityLevel
    {
        int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _name;
        [Required]
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetProperty(ref _unitPrice, value); }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Id <= 0)
            {
                validationResults.Add(new ValidationResult("Id must have a positive value", new string[] { "Id" }));
            }

            if (validationResults.Count() > 0)
                return validationResults.ToArray();
            else
                return new ValidationResult[] { ValidationResult.Success };
        }

        public IEnumerable<ValidationResultWithSeverityLevel> ValidateWithSeverityLevels(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResultWithSeverityLevel>();

            if (UnitPrice <= 0)
            {
                validationResults.Add(new ValidationResultWithSeverityLevel("Unit Price must have a positive value", new string[] { "UnitPrice" }, ValidationSeverityLevel.Warning, this));
            }

            if (validationResults.Count() > 0)
                return validationResults.ToArray();
            else
                return new ValidationResultWithSeverityLevel[] { ValidationResultWithSeverityLevel.Success };
        }
    }
}
