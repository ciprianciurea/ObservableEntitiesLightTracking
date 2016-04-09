using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;

namespace ObservableEntitiesLightTracking.Tests
{
    [TestClass]
    public class OEEntitySetTests
    {
        #region Attach tests
        [TestMethod]
        public void Attached_entity_should_be_tracked_as_unchanged()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Attach(product);

            var productEntry = changeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Unchanged, productEntry.State);
        }

        [TestMethod]
        public void Modified_attached_entity_should_be_tracked_as_modified()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var productEntry = changeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);
        }

        [TestMethod]
        public void Modified_attached_entity_should_only_add_modified_property()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var productEntry = changeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);

            var modifiedProperties = productEntry.ModifiedProperties.ToArray();
            Assert.AreEqual(1, modifiedProperties.Count());
            Assert.AreEqual("UnitPrice", modifiedProperties[0]);
        }

        [TestMethod]
        public void Modified_attached_entity_should_add_multiple_modified_properties()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            product.Name = "Modified Product";

            var productEntry = changeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);

            var modifiedProperties = productEntry.ModifiedProperties.ToArray();
            Assert.AreEqual(2, modifiedProperties.Count());
            Assert.AreEqual("UnitPrice", modifiedProperties[0]);
            Assert.AreEqual("Name", modifiedProperties[1]);
        }
        #endregion Attach tests

        #region Add Tests
        [TestMethod]
        public void Added_entity_should_be_tracked_as_added()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Add(product);

            var productEntry = changeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Added, productEntry.State);
        }

        [TestMethod]
        public void Modified_added_entity_should_be_tracked_as_added()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            product.UnitPrice++;

            var productEntry = changeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Added, productEntry.State);
        }
        #endregion Add Tests

        #region Delete Tests
        [TestMethod]
        public void Deleted_attached_entity_should_be_tracked_as_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Attach(product);
            productSet.Delete(product);

            var productEntry = changeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Deleted, productEntry.State);
        }

        [TestMethod]
        public void Deleted_modified_attached_entity_should_be_tracked_as_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            productSet.Delete(product);

            var productEntry = changeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Deleted, productEntry.State);
        }

        [TestMethod]
        public void Deleted_added_entity_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Add(product);
            productSet.Delete(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }
        #endregion Delete Tests

        #region Detach Tests
        [TestMethod]
        public void Detached_attached_entities_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Attach(product);
            productSet.Detach(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_added_entities_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product();
            productSet.Add(product);
            productSet.Detach(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_modified_attached_entities_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker, null);

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            productSet.Detach(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }
        #endregion Detach Tests
    }
}
