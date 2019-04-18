using System;
using System.Collections.Generic;

namespace ObservableEntitiesLightTracking
{
    public class OEContextConfiguration
    {
        private IServiceProvider _validationServiceProvider;
        /// <summary>
        /// A service provider to be passed to the validation context to be used by the validation methods.
        /// </summary>
        public IServiceProvider ValidationServiceProvider
        {
            get { return _validationServiceProvider; }
            set { _validationServiceProvider = value; }
        }

        private ICollection<object> _validationSafeSeverityLevels;
        /// <summary>
        /// An enumeration of severity levels that should not fail the validation. 
        /// </summary>
        public ICollection<object> ValidationSafeSeverityLevels
        {
            get { return _validationSafeSeverityLevels; }
            set { _validationSafeSeverityLevels = value; }
        }

        public bool EnableLowerCamelCaseOnMemberNames { get; set; }
    }
}
