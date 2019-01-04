using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class ProductValidationService : IProductValidationService
    {
        public IEnumerable<ValidationResult> Validate(ProductWithCustomValidationProviderNoSeverity product)
        {
            var validationResults = new List<ValidationResult>();

            if (product.Id <= 0)
            {
                validationResults.Add(new ValidationResult("Id must have a positive value", new string[] { "Id" }));
            }

            if (string.IsNullOrWhiteSpace(product.Name))
            {
                validationResults.Add(new ValidationResult("Name cannot be empty.", new string[] { "Name" }));
            }

            if (product.UnitPrice <= 0)
            {
                validationResults.Add(new ValidationResult("Unit Price must have a positive value", new string[] { "UnitPrice" }));
            }

            return validationResults;
        }
    }
}
