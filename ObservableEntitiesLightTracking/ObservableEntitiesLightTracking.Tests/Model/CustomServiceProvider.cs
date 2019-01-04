using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests.Model
{
    public class CustomValidationServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IProductValidationService))
                return new ProductValidationService();
            else
                return null;
        }
    }
}
