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
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Attach(product);

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Unchanged, productEntry.State);
        }

        [TestMethod]
        public void Modified_attached_entity_should_be_tracked_as_modified()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);
        }

        [TestMethod]
        public void Modified_attached_entity_should_only_add_modified_property()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);

            var modifiedProperties = productEntry.ModifiedProperties.ToArray();
            Assert.AreEqual(1, modifiedProperties.Count());

            var modifiedProperty = modifiedProperties[0];
            Assert.AreEqual("UnitPrice", modifiedProperty.Name);
            Assert.AreEqual(100M, modifiedProperty.OriginalValue);
        }

        [TestMethod]
        public void Modified_attached_entity_should_add_multiple_modified_properties()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            product.Name = "Modified Product";

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Modified, productEntry.State);

            var modifiedProperties = productEntry.ModifiedProperties.ToArray();
            Assert.AreEqual(2, modifiedProperties.Count());

            var modifiedProperty = modifiedProperties[0];
            Assert.AreEqual("UnitPrice", modifiedProperty.Name);
            Assert.AreEqual(100M, modifiedProperty.OriginalValue);

            modifiedProperty = modifiedProperties[1];
            Assert.AreEqual("Name", modifiedProperty.Name);
            Assert.AreEqual("Test product", modifiedProperty.OriginalValue);
        }
        #endregion Attach tests

        #region Add Tests
        [TestMethod]
        public void Added_entity_should_be_tracked_as_added()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Add(product);

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Added, productEntry.State);
        }

        [TestMethod]
        public void Modified_added_entity_should_be_tracked_as_added()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            product.UnitPrice++;

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreSame(product, productEntry.Entity);
            Assert.AreEqual(OEEntityState.Added, productEntry.State);
        }
        #endregion Add Tests

        #region Delete Tests
        [TestMethod]
        public void Deleted_attached_entity_should_be_tracked_as_deleted()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Attach(product);
            productSet.Delete(product);

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Deleted, productEntry.State);
        }

        [TestMethod]
        public void Deleted_modified_attached_entity_should_be_tracked_as_deleted()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            productSet.Delete(product);

            var productEntry = context.ChangeTracker.Entries().First();
            Assert.AreEqual(OEEntityState.Deleted, productEntry.State);
        }

        [TestMethod]
        public void Deleted_added_entity_should_be_no_longer_tracked()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Add(product);
            productSet.Delete(product);

            Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }
        #endregion Delete Tests

        #region Detach Tests
        [TestMethod]
        public void Detached_attached_entities_should_be_no_longer_tracked()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Attach(product);
            productSet.Detach(product);

            Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_added_entities_should_be_no_longer_tracked()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product();
            productSet.Add(product);
            productSet.Detach(product);

            Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }

        [TestMethod]
        public void Detached_modified_attached_entities_should_be_no_longer_tracked()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;
            productSet.Detach(product);

            Assert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }
        #endregion Detach Tests

        #region HasChanges tests
        [TestMethod]
        public void Attach_means_no_changes()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault().Entity);
        }

        [TestMethod]
        public void GetChanges_should_return_attached_modified()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            product.UnitPrice++;

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault().Entity);
            var modifiedProperty = changes.First().ModifiedProperties.First();
            Assert.AreEqual("UnitPrice", modifiedProperty.Name);
            Assert.AreEqual(100M, modifiedProperty.OriginalValue);
        }

        [TestMethod]
        public void GetChanges_should_return_added_modified()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Add(product);
            product.UnitPrice++;

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault().Entity);
        }

        [TestMethod]
        public void GetChanges_should_return_attached_deleted()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            productSet.Attach(product);
            productSet.Delete(product);

            var changes = productSet.GetChanges();
            Assert.AreSame(product, changes.FirstOrDefault().Entity);
        }

        [TestMethod]
        public void GetChanges_should_return_attached_modified_deleted()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            Assert.AreSame(product, changes.FirstOrDefault().Entity);
        }

        [TestMethod]
        public void GetChanges_should_not_return_added_deleted()
        {
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
            var context = new OEContext();
            var productSet = context.Set<Product>();
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
