using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class ResourceShould
{
    [TestMethod]
    public void CreateResourceInstance()
    {
        var id = Guid.NewGuid();
        var version = 1L;
        var name = "Test Resource";
        var start = DateTimeOffset.UtcNow;
        var end = DateTimeOffset.UtcNow.AddHours(1);

        new ResourceShouldContext()
            .WhenCreateResourceInstance(id, version, name, start, end)
            .ThenIdIs(id)
            .ThenVersionIs(1L)
            .ThenNameIs("Test Resource")
            .ThenStartIs(start)
            .ThenEndIs(end);
    }
}