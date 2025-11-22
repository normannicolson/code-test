using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class RoomShould
{
    [TestMethod]
    public void CreateRoomInstance()
    {
        var id = Guid.NewGuid();
        var version = 1L;
        var name = "Test Room";
        var start = DateTimeOffset.UtcNow;
        var end = DateTimeOffset.UtcNow.AddHours(1);

        new RoomShouldContext()
            .WhenCreateRoomInstance(id, version, name, start, end)
            .ThenIdIs(id)
            .ThenVersionIs(1L)
            .ThenNameIs("Test Room")
            .ThenStartIs(start)
            .ThenEndIs(end);
    }
}
