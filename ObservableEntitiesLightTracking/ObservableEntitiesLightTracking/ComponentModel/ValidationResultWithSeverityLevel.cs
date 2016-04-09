using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObservableEntitiesLightTracking.ComponentModel
{
    /// <summary>
    /// Represents a container for the results of a validation request.
    /// </summary>
    public class ValidationResultWithSeverityLevel : ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using an error message and an error severity.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public ValidationResultWithSeverityLevel(string errorMessage, object errorSeverity)
            : base(errorMessage)
        {
            this.ErrorSeverity = errorSeverity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using a <see cref="ValidationResultWithSeverityLevel" /> object.
        /// </summary>
        /// <param name="validationResult">The validation result object.</param>
        public ValidationResultWithSeverityLevel(ValidationResultWithSeverityLevel validationResult)
            : base(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames))
        {
            this.ErrorSeverity = validationResult.ErrorSeverity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using an error message, 
        /// a list of members that have validation errors and an error severity.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The list of member names that have validation errors.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public ValidationResultWithSeverityLevel(string errorMessage, IEnumerable<string> memberNames, object errorSeverity)
            : base(errorMessage, memberNames)
        {
            this.ErrorSeverity = errorSeverity;
        }

        /// <summary>
        /// Gets or sets the error severity Level
        /// </summary>
        public object ErrorSeverity { get; set; }
    }
}
