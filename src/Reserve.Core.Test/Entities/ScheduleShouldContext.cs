using System.Text.Json;
using Reserve.Core.Entities;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class ScheduleShouldContext
{
    private Reserve.Core.Entities.Schedule sut;

    private List<Reserve.Core.Entities.Slot>? foundSlots = null;

    public ScheduleShouldContext()
    {
        this.sut = new Reserve.Core.Entities.Schedule();
    }

    public ScheduleShouldContext GivenSchedule(string name, long version, DateTimeOffset start, DateTimeOffset end)
    {
        this.sut = new Reserve.Core.Entities.Schedule(Guid.NewGuid(), version, name, start, end);

        return this;
    }

    public ScheduleShouldContext GivenScheduleSlot(string name, long version, DateTimeOffset start, DateTimeOffset end)
    {
        var slot = new Reserve.Core.Entities.Slot(Guid.NewGuid(), version, name, start, end);

        this.sut.Slots.Add(slot);

        return this;
    }

    public ScheduleShouldContext GivenScheduleRoom(string name, long version, DateTimeOffset start, DateTimeOffset end)
    {
        var room = new Reserve.Core.Entities.Room(Guid.NewGuid(), version, name, start, end, RoomType.Double);

        this.sut.Rooms.Add(room);

        return this;
    }

        public ScheduleShouldContext GivenBooking(string name, long version, DateTimeOffset start, DateTimeOffset end, string roomName)
    {
        var room = this.sut.Rooms
            .First(s => s.Name.Equals(roomName, StringComparison.InvariantCulture));

        var slot = this.sut.Slots
            .First(s => s.Start == start && s.End == end);

        var booking = new Reserve.Core.Entities.Booking(Guid.NewGuid(), version, name, start, end)
        {
            Slots = new List<Slot> { slot },
            Rooms = new List<Room> { room }
        };

        return this;
    }

    public ScheduleShouldContext WhenSearchByDateRange(DateTimeOffset start, DateTimeOffset end)
    {
        this.foundSlots = this.sut.Slots
            .Where(i =>
                (i.Start <= start && i.End >= end) //Spans
                || (i.Start >= start && i.Start < end) //Starts in range
                || (i.End < end && i.End > start) //Ends in range 
            )
            .ToList();

        var value = JsonSerializer.Serialize<List<Reserve.Core.Entities.Slot>?>(this.foundSlots);
        Console.WriteLine(value);

        return this;
    }

    public ScheduleShouldContext ThenSlotsCount(int expectedCount)
    {
        Assert.IsNotNull(this.foundSlots);
        Assert.AreEqual(expectedCount, this.foundSlots.Count);

        return this;
    }
}