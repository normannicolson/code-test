using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class ResourceShouldContext
{
    private Reserve.Core.Entities.Resource? resource = null;

    public ResourceShouldContext WhenCreateResourceInstance(
        Guid id,
        long version,
        string name, 
        DateTimeOffset start, 
        DateTimeOffset end)
    {
        this.resource = new Reserve.Core.Entities.Resource(id, version, name, start, end);
        return this;
    }

    public ResourceShouldContext ThenIdIs(Guid id)
    {
        Assert.IsNotNull(this.resource);
        Assert.AreEqual(id, this.resource.Id);
        return this;
    }

    public ResourceShouldContext ThenVersionIs(long version)
    {
        Assert.IsNotNull(this.resource);
        Assert.AreEqual(version, resource.Version);
        return this;
    }

    public ResourceShouldContext ThenNameIs(string name)
    {
        Assert.IsNotNull(this.resource);
        Assert.AreEqual(name, resource.Name);
        return this;
    }

    public ResourceShouldContext ThenStartIs(DateTimeOffset start)
    {
        Assert.IsNotNull(this.resource);
        Assert.AreEqual(start, resource.Start);
        return this;
    }

    public ResourceShouldContext ThenEndIs(DateTimeOffset end)
    {
        Assert.IsNotNull(this.resource);
        Assert.AreEqual(end, resource.End);
        return this;
    }
}