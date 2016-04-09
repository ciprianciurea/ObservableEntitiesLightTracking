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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

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
            var productSet = new OEEntitySet<Product>(changeTracker);

            var product = new Product();
            productSet.Attach(product);
            productSet.Detach(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_added_entities_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);

            var product = new Product();
            productSet.Add(product);
            productSet.Detach(product);

            Assert.AreEqual(0, changeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_modified_attached_entities_should_be_no_longer_tracked()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);

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

        #region HasChanges tests
        [TestMethod]
        public void Attach_means_no_changes()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            Assert.AreEqual(false, productSet.HasChanges());
        }

        [TestMethod]
        public void Add_means_changes()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            Assert.AreEqual(true, productSet.HasChanges());
        }

        [TestMethod]
        public void Attach_detach_means_no_changes()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            Assert.AreEqual(false, productSet.HasChanges());
            productSet.Detach(product);
            Assert.AreEqual(false, productSet.HasChanges());
        }

        [TestMethod]
        public void Attach_delete_means_changes()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            Assert.AreEqual(false, productSet.HasChanges());
            productSet.Delete(product);
            Assert.AreEqual(true, productSet.HasChanges());
        }

        [TestMethod]
        public void Add_delete_means_no_changes()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            Assert.AreEqual(true, productSet.HasChanges());
            productSet.Delete(product);
            Assert.AreEqual(false, productSet.HasChanges());
        }

        [TestMethod]
        public void Attach_modify_means_changes_at_the_end()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            Assert.AreEqual(false, productSet.HasChanges());
            product.UnitPrice++;
            Assert.AreEqual(true, productSet.HasChanges());
        }

        [TestMethod]
        public void Add_modify_means_changes_on_each_op()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            Assert.AreEqual(true, productSet.HasChanges());
            product.UnitPrice++;
            Assert.AreEqual(true, productSet.HasChanges());
        }
        #endregion HasChanges tests

        #region  GetChanges tests
        [TestMethod]
        public void GetChanges_should_not_return_attached_unchanged()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            var changes = productSet.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }

        [TestMethod]
        public void GetChanges_should_return_added()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_attached_modified()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_added_modified()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            product.UnitPrice++;

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_return_attached_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            productSet.Delete(product);

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_not_return_attached_modified_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            productSet.Delete(product);

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault());
        }

        [TestMethod]
        public void GetChanges_should_not_return_added_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            productSet.Delete(product);

            var changes = productSet.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }

        [TestMethod]
        public void GetChanges_should_not_return_added_modified_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            product.UnitPrice++;
            productSet.Delete(product);

            var changes = productSet.GetChanges();
            Assert.AreEqual(0, changes.Count());
        }
        #endregion  GetChanges tests

        #region GetAll tests
        [TestMethod]
        public void GetAll_should_return_attached()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            Assert.AreSame(product, productSet.GetAll().FirstOrDefault());
        }

        [TestMethod]
        public void GetAll_should_return_added()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            Assert.AreSame(product, productSet.GetAll().FirstOrDefault());
        }

        [TestMethod]
        public void GetAll_should_return_attached_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            productSet.Delete(product);
            Assert.AreSame(product, productSet.GetAll().FirstOrDefault());
        }

        [TestMethod]
        public void GetAll_should_not_return_added_deleted()
        {
            var changeTracker = new OEChangeTracker();
            var productSet = new OEEntitySet<Product>(changeTracker);
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            productSet.Delete(product);
            Assert.AreEqual(0, productSet.GetAll().Count());
        }
        #endregion GetAll tests
    }
}
