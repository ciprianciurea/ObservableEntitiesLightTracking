using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ObservableEntitiesLightTracking.ComponentModel.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class IndexPropertyAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessageFormatString = "Duplicate value {1} for field {0}.";

        public IndexPropertyAttribute()
            : base(DefaultErrorMessageFormatString)
        {
        }

        public IndexPropertyAttribute(string name)
            : base(DefaultErrorMessageFormatString)
        {
            Name = name;
        }

        /// <summary>
        /// The index name.
        /// </summary>
		public string Name { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var validationContextKey = this.GetType().Name;
            if (validationContext.Items != null && validationContext.Items.ContainsKey(validationContextKey))
            {
                var contextItems = validationContext.Items[validationContextKey] as IEnumerable;
                if (contextItems != null)
                {
                    List<KeyValuePair<string, List<KeyValuePair<string, object>>>> indexProperties = new List<KeyValuePair<string, List<KeyValuePair<string, object>>>>();

                    foreach (var property in validationContext.ObjectInstance.GetType().GetProperties())
                    {
                        var indexName = this.Name ?? "IDX";
                        var indexPropertyAttributes = property.GetCustomAttributes(this.GetType(), true).Cast<IndexPropertyAttribute>();
                        if (indexPropertyAttributes.Any(p => p.Name.Equals(indexName)))
                        {
                            var propertyValue = property.GetValue(validationContext.ObjectInstance);

                            List<KeyValuePair<string, object>> currentIndexProperties = indexProperties.FirstOrDefault(p => p.Key == indexName).Value;
                            if (currentIndexProperties == null)
                                currentIndexProperties = new List<KeyValuePair<string, object>>();

                            var kvp = new KeyValuePair<string, object>(property.Name, propertyValue);
                            currentIndexProperties.Add(kvp);
                        }
                    }

                    foreach (var contextItem in contextItems)
                    {
                        if (contextItem != validationContext.ObjectInstance)
                        {
                            List<string> duplicateProperties = new List<string>();
                            bool isDuplicate = true;

                            foreach (var indexProperty in indexProperties)
                            {
                                var contextItemKeyProperty = contextItem.GetType().GetProperty(indexProperty.Key);
                                if (contextItemKeyProperty != null)
                                {
                                    var contextItemKeyValue = contextItemKeyProperty.GetValue(contextItem);
                                    if ((contextItemKeyValue != null && contextItemKeyValue.Equals(indexProperty.Value)) || (contextItemKeyValue == null && indexProperty.Value == null))
                                        isDuplicate = isDuplicate && true;
                                    else
                                        isDuplicate = isDuplicate && false;
                                }
                            }

                            if (isDuplicate)
                            {
                                foreach (var indexProperty in indexProperties)
                                    duplicateProperties.Add(indexProperty.Key);
                            }

                            if (duplicateProperties.Contains(validationContext.MemberName))
                            {
                                return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName, indexProperties.Single(p => p.Key == validationContext.MemberName).Value), new string[] { validationContext.MemberName });
                            }
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
