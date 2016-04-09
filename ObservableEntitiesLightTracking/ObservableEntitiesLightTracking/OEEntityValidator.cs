using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.ComponentModel.DataAnnotations;
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

            //if (validationContext != null)
            //{
            //    bool contextValidationResult = true;
            //    foreach (var contextItem in validationContext)
            //    {
            //        if (contextItem != instance)
            //        {
            //            //TODO: primary keys validation
            //            contextValidationResult = true;
            //        }
            //    }
            //    result = result && contextValidationResult;
            //}

            return result;
        }

        public static bool TryValidateObjectWithSeverityLevel(IValidatableObjectWithSeverityLevel instance, ValidationContext validationContext, ICollection<ValidationResultWithSeverityLevel> validationResults, bool validateAllProperties)
        {
            bool result = true;

            var results = instance.Validate(validationContext);
            if (results != null)
            {
                foreach (var validationResult in results)
                    validationResults.Add(validationResult);
                result = results.Any();
            }

            return result;
        }
    }
}
