using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking
{
    public static class OEEntityValidator
    {
        public static bool TryValidateObject(object instance, ValidationContext validationContext, ICollection<ValidationResult> validationResults, bool validateAllProperties)
        {
            var result = Validator.TryValidateObject(instance, validationContext, validationResults, validateAllProperties);
            return result;
        }

        public static bool TryValidateObjectWithSeverityLevel(IValidatableObjectWithSeverityLevel instance, ValidationContext validationContext, ICollection<ValidationResultWithSeverityLevel> validationResults, bool validateAllProperties, ICollection<object> failSafeSeverityLevels)
        {
            bool result = true;

            var results = instance.ValidateWithSeverityLevels(validationContext);
            if (results != null)
            {
                foreach (var validationResult in results.Where(p => p != ValidationResultWithSeverityLevel.Success))
                    validationResults.Add(validationResult);
                result = !results.Any(p => p.ErrorSeverity == null || failSafeSeverityLevels == null || !failSafeSeverityLevels.Contains(p.ErrorSeverity));
            }

            return result;
        }

        public static bool TryValidateProperty(object instance, ValidationContext validationContext, ICollection<ValidationResult> validationResults)
        {
            var result = Validator.TryValidateProperty(instance, validationContext, validationResults);
            return result;
        }
    }
}
