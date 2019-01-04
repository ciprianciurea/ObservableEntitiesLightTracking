using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.ComponentModel;
using ObservableEntitiesLightTracking.Tests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class ValidationServiceProviderTests
    {
        [TestMethod]
        public void Should_validate_and_no_errors_when_no_conditions()
        {
            var context = new OEContext();

            var productSet = context.Set<ProductWithCustomValidationProviderNoSeverity>();
            productSet.SetValidationProvider(new CustomValidationServiceProvider());

            var product = new ProductWithCustomValidationProviderNoSeverity()
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
    }
}
