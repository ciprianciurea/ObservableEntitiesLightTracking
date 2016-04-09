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

        #region HasChanges tests
        [TestMethod]
        public void Attach_means_no_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(false, context.HasChanges());
        }

        [TestMethod]
        public void Add_means_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(true, context.HasChanges());
        }

        [TestMethod]
        public void Attach_detach_means_no_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(false, context.HasChanges());
            context.Set<Product>(null).Detach(product);
            Assert.AreEqual(false, context.HasChanges());
        }

        [TestMethod]
        public void Attach_delete_means_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(false, context.HasChanges());
            context.Set<Product>(null).Delete(product);
            Assert.AreEqual(true, context.HasChanges());
        }

        [TestMethod]
        public void Add_delete_means_no_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(true, context.HasChanges());
            context.Set<Product>(null).Delete(product);
            Assert.AreEqual(false, context.HasChanges());
        }

        [TestMethod]
        public void Attach_modify_means_changes_at_the_end()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(false, context.HasChanges());
            product.UnitPrice++;
            Assert.AreEqual(true, context.HasChanges());
        }

        [TestMethod]
        public void Add_modify_means_changes_on_each_op()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(true, context.HasChanges());
            product.UnitPrice++;
            Assert.AreEqual(true, context.HasChanges());
        }
        #endregion HasChanges tests

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

        #region EntityChanged event tests
        [TestMethod]
        public void EntityChanged_should_not_be_triggered_on_attached()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(0, changesCount);
        }

        public void EntityChanged_should_not_be_triggered_on_detached()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            context.Set<Product>(null).Detach(product);
            Assert.AreEqual(0, changesCount);
        }

        [TestMethod]
        public void EntityChanged_should_be_triggered_once_on_attached_deleted()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            context.Set<Product>(null).Delete(product);
            Assert.AreEqual(1, changesCount);
        }

        [TestMethod]
        public void EntityChanged_should_be_triggered_on_added()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(1, changesCount);
        }

        [TestMethod]
        public void EntityChanged_should_be_triggered_on_modified_deleted_for_attached()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            Assert.AreEqual(0, changesCount);
            product.UnitPrice++;
            context.Set<Product>(null).Delete(product);
            Assert.AreEqual(2, changesCount);
        }

        [TestMethod]
        public void EntityChanged_should_be_triggered_on_added_modified_but_not_on_deleted()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(1, changesCount);
            product.UnitPrice++;
            Assert.AreEqual(2, changesCount);
            context.Set<Product>(null).Delete(product);
            Assert.AreEqual(2, changesCount);
        }

        [TestMethod]
        public void EntityChanged_should_be_triggered_on_modified()
        {
            int changesCount = 0;
            var context = new OEContext();
            context.EntityChanged += (s, e) => changesCount++;

            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Attach(product);
            product.UnitPrice++;
            Assert.AreEqual(1, changesCount);
        }
        #endregion EntityChanged event tests

        #region CancelChanges tests
        [TestMethod]
        public void CancelChanges_cancels_added_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.CancelChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
        }

        public void CancelChanges_cancels_attached_modified_changes()
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
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.CancelChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
        }

        public void CancelChanges_cancels_attached_deleted_changes()
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
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.CancelChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
        }
        #endregion CancelChanges tests

        #region ApplyChanges tests
        [TestMethod]
        public void ApplyChanges_applies_added_changes()
        {
            var context = new OEContext();
            var product = new Product()
            {
                Id = 1,
                Name = "Test product",
                UnitPrice = 100
            };
            context.Set<Product>(null).Add(product);
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.ApplyChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
            Assert.AreEqual(1, context.Set<Product>(null).GetAll().Count());
        }

        [TestMethod]
        public void ApplyChanges_applies_attached_modified_changes()
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
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.ApplyChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
            Assert.AreEqual(1, context.Set<Product>(null).GetAll().Count());
        }

        [TestMethod]
        public void ApplyChanges_applies_attached_deleted_changes()
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
            Assert.AreEqual(true, context.HasChanges());
            Assert.AreEqual(1, context.GetChanges().Count());
            context.ApplyChanges();
            Assert.AreEqual(false, context.HasChanges());
            Assert.AreEqual(0, context.GetChanges().Count());
            Assert.AreEqual(0, context.Set<Product>(null).GetAll().Count());
        }
        #endregion ApplyChanges tests
    }
}
