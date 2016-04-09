using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.ComponentModel
{
    /// <summary>
    /// Defines members that data entity classes can implement to provide support for receiving validation errors with severity levels from an external validator.
    /// </summary>
    public interface IWriteDataErrorInfo
    {
        /// <summary>
        /// Passes a collection of <see cref="ValidationResultWithSeverityLevel" /> to the specified object.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        void AddValidationErrors(ICollection<ValidationResultWithSeverityLevel> validationResults);
    }
}
