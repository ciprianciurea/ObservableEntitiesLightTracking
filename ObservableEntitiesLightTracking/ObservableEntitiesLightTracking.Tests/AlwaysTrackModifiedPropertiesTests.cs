using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObservableEntitiesLightTracking.Tests.Model;
using System.Linq;

namespace ObservableEntitiesLightTracking.Tests
{
  [TestClass]
  public class AlwaysTrackModifiedPropertiesTests
  {
    [TestMethod]
    public void Modified_added_should_track_modified_properties_if_AlwaysTrackModifiedProperties()
    {
      var context = new OEContext();
      context.Set<Product>().AlwaysTrackModifiedProperties = true;
      var product = new Product()
      {
        Id = 1,
        Name = "Test product",
        UnitPrice = 100
      };
      context.Set<Product>().Add(product);
      product.UnitPrice = 150;
      var changes = context.GetChanges();
      var entityEntry = changes.FirstOrDefault();
      Assert.AreSame(product, entityEntry.Entity);
      var modifiedProperties = entityEntry.ModifiedProperties.ToArray();
      Assert.AreEqual("UnitPrice", modifiedProperties[0].Name);
      Assert.AreEqual(100M, modifiedProperties[0].OriginalValue);
    }

    [TestMethod]
    public void Modified_added_should_track_modified_properties_if_not_AlwaysTrackModifiedProperties()
    {
      var context = new OEContext();
      var product = new Product()
      {
        Id = 1,
        Name = "Test product",
        UnitPrice = 100
      };
      context.Set<Product>().Add(product);
      product.UnitPrice = 150;
      var changes = context.GetChanges();
      var entityEntry = changes.FirstOrDefault();
      Assert.AreSame(product, entityEntry.Entity);
      var modifiedProperties = entityEntry.ModifiedProperties.ToArray();
      Assert.AreEqual(0, modifiedProperties.Count());
    }
  }
}
