namespace Reserve.Infrastructure.Data.Test;

[TestClass]
public class ContextShould
{
    [TestMethod]
    public async Task FindAvailableRoom_Day_All_Available()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenRoom("Room 1")
            .GivenRoom("Room 2")
            .GivenRoom("Room 3")
            .GivenRoom("Room 4")
            .GivenRoom("Room 5")
            .GivenRoom("Room 6")
            .GivenRoom("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1")
            .GivenScheduleRoom("Default", "Room 2")
            .GivenScheduleRoom("Default", "Room 3")
            .GivenScheduleRoom("Default", "Room 4")
            .GivenScheduleRoom("Default", "Room 5")
            .GivenScheduleRoom("Default", "Room 6")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .WhenFindAvailableRoomIsCalled(
                friStart,
                friEnd))
            .ThenAvailableRoomCount(6)
            .ThenAvailableRoomsContains("Room 1")
            .ThenAvailableRoomsContains("Room 2")
            .ThenAvailableRoomsContains("Room 3")
            .ThenAvailableRoomsContains("Room 4")
            .ThenAvailableRoomsContains("Room 5")
            .ThenAvailableRoomsContains("Room 6");
    }

    [TestMethod]
    public async Task FindAvailableRoom_One_Available()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenRoom("Room 1")
            .GivenRoom("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1")
            .GivenScheduleRoom("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingRoom("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableRoomIsCalled(
                friStart,
                friEnd))
            .ThenAvailableRoomCount(1)
            .ThenAvailableRoomsContains("Room 2");
    }

    [TestMethod]
    public async Task find_available_room_for_two_nights()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenRoom("Room 1")
            .GivenRoom("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1")
            .GivenScheduleRoom("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingRoom("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableRoomIsCalled(
                friStart,
                satEnd))
            .ThenAvailableRoomCount(1)
            .ThenAvailableRoomsContains("Room 2");
    }

    [TestMethod]
    public async Task find_available_room_for_sat_night()
    {
        var friStart = new DateTime(2026, 06, 05, 15, 0, 0);
        var friEnd = new DateTime(2026, 06, 06, 11, 0, 0);
        var satStart = new DateTime(2026, 06, 06, 15, 0, 0);
        var satEnd = new DateTime(2026, 06, 07, 11, 0, 0);
        var sunStart = new DateTime(2026, 06, 07, 15, 0, 0);
        var sunEnd = new DateTime(2026, 06, 08, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenRoom("Room 1")
            .GivenRoom("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1")
            .GivenScheduleRoom("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingRoom("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableRoomIsCalled(
                satStart,
                satEnd))
            .ThenAvailableRoomCount(2)
            .ThenAvailableRoomsContains("Room 1")
            .ThenAvailableRoomsContains("Room 2");
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
        var monStart = new DateTime(2028, 06, 08, 15, 0, 0);
        var monEnd = new DateTime(2028, 06, 09, 11, 0, 0);

        (await new ContextShouldContext()
            .GivenRoom("Room 1")
            .GivenRoom("Room 2")
            .GivenSlot("Fri", friStart, friEnd)
            .GivenSlot("Sat", satStart, satEnd)
            .GivenSlot("Sun", sunStart, sunEnd)
            .GivenSchedule("Default")
            .GivenScheduleRoom("Default", "Room 1")
            .GivenScheduleRoom("Default", "Room 2")
            .GivenScheduleSlot("Default", "Fri")
            .GivenScheduleSlot("Default", "Sat")
            .GivenScheduleSlot("Default", "Sun")
            .GivenBooking("Booking 1", friStart, friEnd)
            .GivenBookingRoom("Booking 1", "Room 1")
            .GivenBookingSlot("Booking 1", "Fri")
            .WhenFindAvailableRoomIsCalled(
                monStart,
                monEnd))
            .ThenAvailableRoomCount(0);
    }
}