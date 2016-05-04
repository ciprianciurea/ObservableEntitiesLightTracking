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
        /// Gets a <see cref="ValidationResultWithSeverityLevel"/> that indicates Success. 
        /// </summary>
        /// <remarks> 
        /// The <c>null</c> value is used to indicate success.  Consumers of <see cref="ValidationResultWithSeverityLevel"/>s 
        /// should compare the values to <see cref="ValidationResultWithSeverityLevel.Success"/> rather than checking for null. 
        /// </remarks> 
        public static readonly new ValidationResultWithSeverityLevel Success;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using an error message and an error severity.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public ValidationResultWithSeverityLevel(string errorMessage, object errorSeverity, object entity)
            : base(errorMessage)
        {
            this.ErrorSeverity = errorSeverity;
            this.Entity = entity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using a <see cref="ValidationResultWithSeverityLevel" /> object.
        /// </summary>
        /// <param name="validationResult">The validation result object.</param>
        public ValidationResultWithSeverityLevel(ValidationResultWithSeverityLevel validationResult)
            : base(new ValidationResult(validationResult.ErrorMessage, validationResult.MemberNames))
        {
            this.ErrorSeverity = validationResult.ErrorSeverity;
            this.Entity = validationResult.Entity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResultWithSeverityLevel" /> class by using an error message, 
        /// a list of members that have validation errors and an error severity.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The list of member names that have validation errors.</param>
        /// <param name="errorSeverity">The error severity.</param>
        public ValidationResultWithSeverityLevel(string errorMessage, IEnumerable<string> memberNames, object errorSeverity, object entity)
            : base(errorMessage, memberNames)
        {
            this.ErrorSeverity = errorSeverity;
            this.Entity = entity;
        }

        /// <summary>
        /// Gets or sets the error identifier value.
        /// </summary>
        public int ErrorId { get; set; }

        /// <summary>
        /// Gets or sets the error severity Level.
        /// </summary>
        public object ErrorSeverity { get; set; }

        /// <summary>
        /// Gets the validated entity.
        /// </summary>
        public object Entity { get; private set; }
    }
}
