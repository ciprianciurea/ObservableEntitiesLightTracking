using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.Tests.Model;
using System.Collections.Generic;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class ValidationTests
    {
        #region OEContext validation tests
        [TestMethod]
        public void Should_validate_when_no_conditions()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product();
            productSet.Add(product);
            var result = context.Validate();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Should_validate_when_attributes_conditions_are_met()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributes>();
            var product = new ProductWithValidationAttributes()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            var result = context.Validate();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Should_not_validate_when_attributes_conditions_are_not_met()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributes>();
            var product = new ProductWithValidationAttributes()
            {
                Id = 1,
                Name = null,
                UnitPrice = -1
            };
            productSet.Add(product);
            var result = context.Validate();
            Assert.AreEqual(false, result);
        }
        #endregion OEContext validation tests

        #region OEEntitySet validation tests
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
        }

        [TestMethod]
        public void Should_validate_and_no_errors_when_attributes_conditions_are_met()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithValidationAttributes>();
            var product = new ProductWithValidationAttributes()
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
        }
        #endregion OEEntitySet validation tests
    }
}
