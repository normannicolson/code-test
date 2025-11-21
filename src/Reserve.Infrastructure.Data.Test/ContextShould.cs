namespace Reserve.Infrastructure.Data.Test;

[TestClass]
public class ContextShould
{
    [TestMethod]
    public async Task FindAvailableResource_Day_All_Available()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenResource("Room 1")
            .GivenResource("Room 2")
            .GivenResource("Room 3")
            .GivenResource("Room 4")
            .GivenResource("Room 5")
            .GivenResource("Room 6")
            .GivenResource("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleResource("Default", "Room 1")
            .GivenScheduleResource("Default", "Room 2")
            .GivenScheduleResource("Default", "Room 3")
            .GivenScheduleResource("Default", "Room 4")
            .GivenScheduleResource("Default", "Room 5")
            .GivenScheduleResource("Default", "Room 6")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .WhenFindAvailableResourceIsCalled(
                friStart,
                friEnd))
            .ThenAvailableResourceCount(6)
            .ThenAvailableResourcesContains("Room 1")
            .ThenAvailableResourcesContains("Room 2")
            .ThenAvailableResourcesContains("Room 3")
            .ThenAvailableResourcesContains("Room 4")
            .ThenAvailableResourcesContains("Room 5")
            .ThenAvailableResourcesContains("Room 6");
    }

    [TestMethod]
    public async Task FindAvailableResource_One_Available()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenResource("Room 1")
            .GivenResource("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleResource("Default", "Room 1")
            .GivenScheduleResource("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingResource("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableResourceIsCalled(
                friStart,
                friEnd))
            .ThenAvailableResourceCount(1)
            .ThenAvailableResourcesContains("Room 2");
    }

    [TestMethod]
    public async Task find_available_resource_for_two_nights()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenResource("Room 1")
            .GivenResource("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleResource("Default", "Room 1")
            .GivenScheduleResource("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingResource("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableResourceIsCalled(
                friStart,
                satEnd))
            .ThenAvailableResourceCount(1)
            .ThenAvailableResourcesContains("Room 2");
    }

    [TestMethod]
    public async Task find_available_resource_for_sat_night()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenResource("Room 1")
            .GivenResource("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleResource("Default", "Room 1")
            .GivenScheduleResource("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingResource("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableResourceIsCalled(
                satStart,
                satEnd))
            .ThenAvailableResourceCount(2)
            .ThenAvailableResourcesContains("Room 1")
            .ThenAvailableResourcesContains("Room 2");
    }

    [TestMethod]
    public async Task outof_range()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);
        var monStart = new DateTime(2026, 06, 08, 15, 0, 0);
        var monEnd = new DateTime(2026, 06, 09, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenResource("Room 1")
            .GivenResource("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleResource("Default", "Room 1")
            .GivenScheduleResource("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingResource("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableResourceIsCalled(
                monStart,
                monEnd))
            .ThenAvailableResourceCount(0);
    }
}