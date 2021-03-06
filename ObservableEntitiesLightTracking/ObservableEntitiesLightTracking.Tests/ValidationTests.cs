﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.Tests.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class ValidationTests
    {
        #region Validation tests
        [TestMethod]
        public void Should_validate_and_no_errors_when_no_conditions()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product();
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(0, validationResults.Count());

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(0, validationResults.Count());
        }

        [TestMethod]
        public void Should_validate_and_no_errors_when_attributes_conditions_are_met()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributesNoSeverity>();
            var product = new ProductWithValidationAttributesNoSeverity()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(0, validationResults.Count());

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(0, validationResults.Count());
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_when_attributes_conditions_are_not_met_no_severity_support()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributesNoSeverity>();
            var product = new ProductWithValidationAttributesNoSeverity()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_when_custom_validation_is_not_met_no_severity_support()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithCustomValidationNoSeverity>();
            var product = new ProductWithCustomValidationNoSeverity()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
        }

        [TestMethod]
        public void Should_validate_when_custom_validation_is_met_severity_support()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithCustomValidationSeveritySupport>();
            var product = new ProductWithCustomValidationSeveritySupport()
            {
                Id = 1,
                Name = "Valid Product",
                UnitPrice = 1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_when_custom_validation_is_not_met_severity_support()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithCustomValidationSeveritySupport>();
            var product = new ProductWithCustomValidationSeveritySupport()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);
        }

        [TestMethod]
        public void Should_validate_and_return_errors_when_all_severity_levels_are_excluded()
        {
            var context = new OEContext();

            context.Configuration.ValidationSafeSeverityLevels = new Collection<object>() { ValidationSeverityLevel.Error, ValidationSeverityLevel.Warning };

            var productSet = context.Set<ProductWithCustomValidationSeveritySupport>();
            var product = new ProductWithCustomValidationSeveritySupport()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(true, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_when_some_severity_levels_are_not_excluded()
        {
            var context = new OEContext();

            context.Configuration.ValidationSafeSeverityLevels = new Collection<object>() { ValidationSeverityLevel.Warning };
            // ValidationSeverityLevel.Error should fail validation

            var productSet = context.Set<ProductWithCustomValidationSeveritySupport>();
            var product = new ProductWithCustomValidationSeveritySupport()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Error, validationResults[1].ErrorSeverity);
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("UnitPrice", validationResults[2].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[2].ErrorSeverity);
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_when_mixed_validation_is_not_met()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithMixedValidation>();
            var product = new ProductWithMixedValidation()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Name", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(null, validationResults[0].ErrorSeverity);
            // when attributes validation fails, IValidatableObject is not executed
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("UnitPrice", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[1].ErrorSeverity);

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            // when attributes validation fails, IValidtableObject is not executed
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Name", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreEqual(null, validationResults[0].ErrorSeverity);
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("UnitPrice", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreEqual(ValidationSeverityLevel.Warning, validationResults[1].ErrorSeverity);
        }

        [TestMethod]
        public void Should_not_validate_inherited_classes_with_errors_when_the_entity_set_is_registered_against_the_base_type()
        {
            var context = new OEContext();
            var productSet = context.Set<BaseItem>();
            var product = new InheritedItem()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1,
                Description = null
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Name", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Description", validationResults[1].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Name", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("Description", validationResults[1].MemberNames.ElementAt(0));
        }

        [TestMethod]
        public void Should_not_validate_and_return_errors_with_lower_camel_case_fiel_names_when_attributes_conditions_are_not_met_no_severity_support()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributesNoSeverity>();
            context.Configuration.EnableLowerCamelCaseOnMemberNames = true;
            var product = new ProductWithValidationAttributesNoSeverity()
            {
                Id = -1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("unitPrice", validationResults[2].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(3, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[1].Entity);
            Assert.AreEqual("name", validationResults[1].MemberNames.ElementAt(0));
            Assert.AreSame(product, validationResults[2].Entity);
            Assert.AreEqual("unitPrice", validationResults[2].MemberNames.ElementAt(0));
        }

        #endregion Validation tests
    }
}
