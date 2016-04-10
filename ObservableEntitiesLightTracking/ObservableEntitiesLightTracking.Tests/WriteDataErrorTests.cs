using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;
using ObservableEntitiesLightTracking.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class WriteDataErrorTests
    {
        [TestMethod]
        public void Should_write_validation_errors_to_the_entity_implementing_IWriteDataErrorInfo()
        {
            var context = new OEContext();
            var productSet = context.Set<ProductWithIWriteDataError>();
            var product = new ProductWithIWriteDataError();
            productSet.Add(product);

            var validationResults = new List<ValidationResultWithSeverityLevel>();
            var result = productSet.Validate(validationResults);
            Assert.AreEqual(false, result);
            Assert.AreEqual(1, validationResults.Count());
            Assert.AreSame(product, validationResults[0].Entity);
            Assert.AreEqual("Name", validationResults[0].MemberNames.ElementAt(0));

            var objectValidationResults = product.ValidationResults.ToList();
            Assert.AreEqual("Name", objectValidationResults[0].MemberNames.ElementAt(0));
        }
    }
}
