using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;
using ObservableEntitiesLightTracking.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class KeyPropertyTests
    {
        [TestMethod]
        public void Should_validate_when_unique_key_in_context_set()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithSingleKeyProperty>();
            var product1 = new ProductWithSingleKeyProperty()
            {
                Id = 1,
                Name = "First product",
                UnitPrice = 100
            };
            productSet.Add(product1);
            var product2 = new ProductWithSingleKeyProperty()
            {
                Id = 2,
                Name = "Second product",
                UnitPrice = 100
            };
            productSet.Add(product2);

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
        public void Should_fail_when_duplicate_key_in_context_set_add()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithSingleKeyProperty>();
            var product1 = new ProductWithSingleKeyProperty()
            {
                Id = 1,
                Name = "First product",
                UnitPrice = 100
            };
            productSet.Add(product1);
            var product2 = new ProductWithSingleKeyProperty()
            {
                Id = 1,
                Name = "Second product",
                UnitPrice = 100
            };
            productSet.Add(product2);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreSame(product1, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product2, validationResults[1].Entity);
            Assert.AreEqual("Id", validationResults[1].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(2, validationResults.Count());
            Assert.AreSame(product1, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
            Assert.AreSame(product2, validationResults[1].Entity);
            Assert.AreEqual("Id", validationResults[1].MemberNames.ElementAt(0));
        }

        [TestMethod]
        public void Should_fail_when_duplicate_key_in_context_set_attach()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithSingleKeyProperty>();
            var product1 = new ProductWithSingleKeyProperty()
            {
                Id = 1,
                Name = "First product",
                UnitPrice = 100
            };
            productSet.Attach(product1);
            var product2 = new ProductWithSingleKeyProperty()
            {
                Id = 1,
                Name = "Second product",
                UnitPrice = 100
            };
            productSet.Add(product2);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(1, validationResults.Count());
            Assert.AreSame(product2, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));

            validationResults = new List<ValidationResultWithSeverityLevel>();
            result = context.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(1, validationResults.Count());
            Assert.AreSame(product2, validationResults[0].Entity);
            Assert.AreEqual("Id", validationResults[0].MemberNames.ElementAt(0));
        }
    }
}
