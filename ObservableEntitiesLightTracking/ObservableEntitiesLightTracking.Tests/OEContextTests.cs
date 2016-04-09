using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class OEContextTests
    {
        [TestMethod]
        public void Set_returns_the_same_entity_set_for_the_same_entity_type()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>(null);
            var productSetAgain = context.Set<Product>(null);
            Assert.AreSame(productSet, productSetAgain);
        }
    }
}
