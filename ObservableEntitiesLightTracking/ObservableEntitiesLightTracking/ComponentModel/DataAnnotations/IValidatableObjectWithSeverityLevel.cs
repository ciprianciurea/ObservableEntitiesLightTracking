using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Provides a way for an object to be invalidated against a severity level.
    /// </summary>
    public interface IValidatableObjectWithSeverityLevel
    {
        /// <summary>
        /// Determines whether the specified object is valid against a severity level.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information, along with the severity level.</returns>
        IEnumerable<ValidationResultWithSeverityLevel> ValidateWithSeverityLevels(ValidationContext validationContext);
    }
}
