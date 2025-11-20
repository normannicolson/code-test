using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class ScheduleShould
{
    [TestMethod]
    public void have_one_night_available_when_in_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .ThenSlotsCount(1);
    }

    [TestMethod]
    public void have_zero_nights_available_when_out_of_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 04, 15, 0, 0), new DateTime(2026, 06, 05, 11, 0, 0))
            .ThenSlotsCount(0);
    }

    [TestMethod]
    public void have_one_night_available_when_one_night_within_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 04, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .ThenSlotsCount(1);
    }

    [TestMethod]
    public void have_one_night_available_when_within_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 05, 16, 0, 0), new DateTime(2026, 06, 06, 10, 0, 0))
            .ThenSlotsCount(1);
    }

    [TestMethod]
    public void have_two_nights_available_when_within_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleResource("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .ThenSlotsCount(2);
    }
}

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

    public ScheduleShouldContext GivenScheduleResource(string name, long version, DateTimeOffset start, DateTimeOffset end)
    {
        var resource = new Reserve.Core.Entities.Resource(Guid.NewGuid(), version, name, start, end);

        this.sut.Resources.Add(resource);

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
        Assert.AreEqual(expectedCount, this.foundSlots.Count);

        return this;
    }
}