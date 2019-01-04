using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public interface IProductValidationService
    {
        IEnumerable<ValidationResult> Validate(ProductWithCustomValidationProviderNoSeverity product);
    }
}
