using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class KeyPropertyAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessageFormatString = "Duplicate value {1} for field {0}.";

        public KeyPropertyAttribute()
            : base(DefaultErrorMessageFormatString)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationContextKey = this.GetType().Name;
            if (validationContext.Items != null && validationContext.Items.ContainsKey(validationContextKey))
            {
                var contextItems = validationContext.Items[validationContextKey] as IEnumerable;
                if (contextItems != null)
                {
                    List<KeyValuePair<string, object>> primaryKeys = new List<KeyValuePair<string, object>>();

                    foreach (var property in validationContext.ObjectInstance.GetType().GetProperties())
                    {
                        var keyPropertyAttributes = property.GetCustomAttributes(this.GetType(), true);
                        if (keyPropertyAttributes.Any())
                        {
                            var propertyValue = property.GetValue(validationContext.ObjectInstance);
                            primaryKeys.Add(new KeyValuePair<string, object>(property.Name, propertyValue));
                        }
                    }

                    foreach (var contextItem in contextItems)
                    {
                        if (contextItem != validationContext.ObjectInstance)
                        {
                            List<string> duplicateProperties = new List<string>();
                            foreach (var primaryKey in primaryKeys)
                            {
                                var contextItemKeyProperty = contextItem.GetType().GetProperty(primaryKey.Key);
                                if (contextItemKeyProperty != null)
                                {
                                    var contextItemKeyValue = contextItemKeyProperty.GetValue(contextItem);
                                    if ((contextItemKeyValue != null && contextItemKeyValue.Equals(primaryKey.Value)) || (contextItemKeyValue == null && primaryKey.Value == null))
                                        duplicateProperties.Add(primaryKey.Key);
                                }
                            }

                            if (duplicateProperties.Contains(validationContext.MemberName))
                            {
                                return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, primaryKeys.Single(p => p.Key == validationContext.MemberName).Value), new string[] { validationContext.MemberName });
                            }
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
