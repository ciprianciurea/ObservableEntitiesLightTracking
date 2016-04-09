using ObservableEntitiesLightTracking.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking
{
    public abstract class OEEntitySet
    {
        public bool ValidateOnPropertyChanged { get; set; }

        public abstract bool Validate(ICollection<ValidationResultWithSeverityLevel> validationResults);
    }
}
