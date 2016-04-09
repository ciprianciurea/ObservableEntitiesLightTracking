using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class OEContextTests
    {
        #region Set<TEntity> tests
        [TestMethod]
        public void Set_returns_the_same_entity_set_for_the_same_entity_type()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>(null);
            var productSetAgain = context.Set<Product>(null);
            Assert.AreSame(productSet, productSetAgain);
        }
        #endregion Set<TEntity> tests

        #region  GetChanges tests
        [TestMethod]
        public void GetChanges_should_not_return_attached_unchanged()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            var changes = context.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }

        [TestMethod]
        public void GetChanges_should_return_added()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            var changes = context.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_attached_modified()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            product.UnitPrice++;

            var changes = context.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_added_modified()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            product.UnitPrice++;

            var changes = context.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_attached_deleted()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            context.Set<Product>(null).Delete(product);

            var changes = context.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_not_return_attached_modified_deleted()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            product.UnitPrice++;
            context.Set<Product>(null).Delete(product);

            var changes = context.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_not_return_added_deleted()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            context.Set<Product>(null).Delete(product);

            var changes = context.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }

        [TestMethod]
        public void GetChanges_should_not_return_added_modified_deleted()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            product.UnitPrice++;
            context.Set<Product>(null).Delete(product);

            var changes = context.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }
        #endregion  GetChanges tests
    }
}
