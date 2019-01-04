using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductWithCustomValidationProviderNoSeverity : ObservableObject, IValidatableObject
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

            var validationService = validationContext.GetService(typeof(IProductValidationService)) as IProductValidationService;

            var serviceValidationResults = validationService.Validate(this);

            validationResults.AddRange(serviceValidationResults);

            if (validationResults.Count() > 0)
                return validationResults.ToArray();
            else
                return new ValidationResult[] { ValidationResult.Success };
        }
    }
}
