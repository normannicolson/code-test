using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Reserve.Core.Test.Entities;

[TestClass]
public class ScheduleShould
{
    [TestMethod]
    public void have_one_night_available_when_in_range()
    {
        new ScheduleShouldContext()
            .GivenSchedule("Default", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
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
            .GivenScheduleRoom("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
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
            .GivenScheduleRoom("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
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
            .GivenScheduleRoom("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
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
            .GivenScheduleRoom("Standard Room 1", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)
            .GivenScheduleRoom("Superior Room 2", 0, DateTimeOffset.MinValue, DateTimeOffset.MaxValue)          
            .GivenScheduleSlot("fri", 0, new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 06, 11, 0, 0))
            .GivenScheduleSlot("sat", 0, new DateTime(2026, 06, 06, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .WhenSearchByDateRange(new DateTime(2026, 06, 05, 15, 0, 0), new DateTime(2026, 06, 07, 11, 0, 0))
            .ThenSlotsCount(2);
    }
}
