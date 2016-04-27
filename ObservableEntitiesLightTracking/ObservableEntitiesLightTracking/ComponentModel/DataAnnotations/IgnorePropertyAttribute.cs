using System;

namespace ObservableEntitiesLightTracking.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IgnorePropertyAttribute : Attribute
    {
    }
}
