using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ObservableEntitiesLightTracking.ComponentModel;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithCustomValidationSeveritySupport : ObservableObject, IValidatableObjectWithSeverityLevel
    {
        int _id;
        public int Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _name;
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

        public IEnumerable<ValidationResultWithSeverityLevel> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResultWithSeverityLevel>();

            if (Id <= 0)
            {
                validationResults.Add(new ValidationResultWithSeverityLevel("Id must have a positive value", new string[] { "Id" }, ValidationSeverityLevel.Error, this));
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(new ValidationResultWithSeverityLevel("Name cannot be empty.", new string[] { "Name" }, ValidationSeverityLevel.Error, this));
            }

            if (UnitPrice <= 0)
            {
                validationResults.Add(new ValidationResultWithSeverityLevel("Unit Price must have a positive value", new string[] { "UnitPrice" }, ValidationSeverityLevel.Warning, this));
            }

            if (validationResults.Count(p => (ValidationSeverityLevel)p.ErrorSeverity == ValidationSeverityLevel.Error) > 0)
                return validationResults.ToArray();
            else
                return new ValidationResultWithSeverityLevel[] { ValidationResultWithSeverityLevel.Success };
        }
    }
}
