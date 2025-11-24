using System;
using Reserve.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class RoomShouldContext
{
    private Reserve.Core.Entities.Room? room = null;

    public RoomShouldContext WhenCreateRoomInstance(
        Guid id,
        long version,
        string name,
        DateTimeOffset start,
        DateTimeOffset end,
        RoomType roomType)
    {
        this.room = new Reserve.Core.Entities.Room(id, version, name, start, end, roomType);
        return this;
    }

    public RoomShouldContext ThenIdIs(Guid id)
    {
        Assert.IsNotNull(this.room);
        Assert.AreEqual(id, this.room.Id);
        return this;
    }

    public RoomShouldContext ThenVersionIs(long version)
    {
        Assert.IsNotNull(this.room);
        Assert.AreEqual(version, room.Version);
        return this;
    }

    public RoomShouldContext ThenNameIs(string name)
    {
        Assert.IsNotNull(this.room);
        Assert.AreEqual(name, room.Name);
        return this;
    }

    public RoomShouldContext ThenStartIs(DateTimeOffset start)
    {
        Assert.IsNotNull(this.room);
        Assert.AreEqual(start, room.Start);
        return this;
    }

    public RoomShouldContext ThenEndIs(DateTimeOffset end)
    {
        Assert.IsNotNull(this.room);
        Assert.AreEqual(end, room.End);
        return this;
    }
}
