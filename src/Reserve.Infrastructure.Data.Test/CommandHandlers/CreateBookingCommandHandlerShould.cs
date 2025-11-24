namespace Reserve.Infrastructure.Data.Test.CommandHandlers;

[TestClass]
public class CreateBookingCommandHandlerShould
{
    [TestMethod]
    public async Task CreateBooking_Successfully()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .WhenCreateBookingIsCalled("Test Booking", "Room 1", friStart, friEnd))
            .ThenResultIsSuccess()
            .ThenBookingExists("Test Booking")
            .ThenBookingCount(1)
            .ThenBookingRoomCount(1)
            .ThenBookingSlotCount(1)
            .ThenBookingHasRoom("Test Booking", "Room 1")
            .ThenBookingHasSlot("Test Booking", "Fri");
    }

    [TestMethod]
    public async Task CreateBooking_WithMultipleSlots()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .WhenCreateBookingIsCalled("Weekend Booking", "Room 1", friStart, sunEnd))
            .ThenResultIsSuccess()
            .ThenBookingExists("Weekend Booking")
            .ThenBookingCount(1)
            .ThenBookingRoomCount(1)
            .ThenBookingSlotCount(3)
            .ThenBookingHasRoom("Weekend Booking", "Room 1")
            .ThenBookingHasSlot("Weekend Booking", "Fri")
            .ThenBookingHasSlot("Weekend Booking", "Sat")
            .ThenBookingHasSlot("Weekend Booking", "Sun");
    }

    [TestMethod]
    public async Task CreateBooking_OnlyIncludesRelevantSlots()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .WhenCreateBookingIsCalled("Friday Booking", "Room 1", friStart, friEnd))
            .ThenResultIsSuccess()
            .ThenBookingSlotCount(1)
            .ThenBookingHasSlot("Friday Booking", "Fri")
            .ThenBookingDoesNotHaveSlot("Friday Booking", "Sat")
            .ThenBookingDoesNotHaveSlot("Friday Booking", "Sun");
    }

    [TestMethod]
    public async Task FailToCreateBooking_WhenRoomHasOverlappingBooking()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenBooking("Existing Booking", friStart, friEnd)
            .GivenBookingRoom("Existing Booking", "Room 1")
            .GivenBookingSlot("Existing Booking", "Fri")
            .WhenCreateBookingIsCalled("New Booking", "Room 1", friStart, friEnd))
            .ThenResultIsFailure()
            .ThenErrorCodeIs("ROOM_UNAVAILABLE")
            .ThenBookingCount(1);
    }

    [TestMethod]
    public async Task CreateBooking_WhenDifferentRoomHasBooking()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenRoom("Grand Hotel", "Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleRoom("Default", "Room 2", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenBooking("Existing Booking", friStart, friEnd)
            .GivenBookingRoom("Existing Booking", "Room 1")
            .GivenBookingSlot("Existing Booking", "Fri")
            .WhenCreateBookingIsCalled("New Booking", "Room 2", friStart, friEnd))
            .ThenResultIsSuccess()
            .ThenBookingExists("New Booking")
            .ThenBookingCount(2)
            .ThenBookingHasRoom("New Booking", "Room 2");
    }

    [TestMethod]
    public async Task CreateBooking_WhenSameRoomHasNonOverlappingBooking()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenBooking("Friday Booking", friStart, friEnd)
            .GivenBookingRoom("Friday Booking", "Room 1")
            .GivenBookingSlot("Friday Booking", "Fri")
            .WhenCreateBookingIsCalled("Saturday Booking", "Room 1", satStart, satEnd))
            .ThenResultIsSuccess()
            .ThenBookingExists("Saturday Booking")
            .ThenBookingCount(2)
            .ThenBookingHasRoom("Saturday Booking", "Room 1")
            .ThenBookingHasSlot("Saturday Booking", "Sat");
    }

    [TestMethod]
    public async Task CreateBooking_WithNoSlots_WhenNoScheduleSlots()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .WhenCreateBookingIsCalled("Test Booking", "Room 1", friStart, friEnd))
            .ThenResultIsSuccess()
            .ThenBookingExists("Test Booking")
            .ThenBookingCount(1)
            .ThenBookingRoomCount(1)
            .ThenBookingSlotCount(0);
    }

    [TestMethod]
    public async Task FailToCreateBooking_WhenPartialOverlapWithExistingBooking()
    {
        // Friday is already booked, trying to book Friday-Saturday should fail
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);

        (await new CreateBookingCommandHandlerShouldContext()
            .GivenHotel("Grand Hotel")
            .GivenRoom("Grand Hotel", "Room 1")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1", "Grand Hotel")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenBooking("Friday Booking", friStart, friEnd)
            .GivenBookingRoom("Friday Booking", "Room 1")
            .GivenBookingSlot("Friday Booking", "Fri")
            .WhenCreateBookingIsCalled("FriSat Booking", "Room 1", friStart, satEnd))
            .ThenResultIsFailure()
            .ThenErrorCodeIs("ROOM_UNAVAILABLE")
            .ThenBookingCount(1);
    }
}
