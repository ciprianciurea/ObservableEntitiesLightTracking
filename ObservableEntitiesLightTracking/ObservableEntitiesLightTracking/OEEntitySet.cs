using ObservableEntitiesLightTracking.ComponentModel;
using System.Collections.Generic;

namespace ObservableEntitiesLightTracking
{
    public abstract class OEEntitySet
    {
        public bool ValidateOnPropertyChanged { get; set; }

        public abstract bool Validate(ICollection<ValidationResultWithSeverityLevel> validationResults);
    }
}
