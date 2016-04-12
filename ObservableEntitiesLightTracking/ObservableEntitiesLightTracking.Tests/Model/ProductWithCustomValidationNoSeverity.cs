using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithCustomValidationNoSeverity : ObservableObject, IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            if (Id <= 0)
            {
                validationResults.Add(new ValidationResult("Id must have a positive value", new string[] { "Id" }));
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                validationResults.Add(new ValidationResult("Name cannot be empty.", new string[] { "Name" }));
            }

            if (UnitPrice <= 0)
            {
                validationResults.Add(new ValidationResult("Unit Price must have a positive value", new string[] { "UnitPrice" }));
            }

            if (validationResults.Count() > 0)
                return validationResults.ToArray();
            else
                return new ValidationResult[] { ValidationResult.Success };
        }
    }
}
