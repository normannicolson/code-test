using Microsoft.EntityFrameworkCore;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data;

public interface IContext
{
    DbSet<Hotel> Hotels { get; set; }

    DbSet<Room> Rooms { get; set; }

    DbSet<Schedule> Schedules { get; set; }

    DbSet<Slot> Slots { get; set; }

    DbSet<ScheduleRoom> ScheduleRooms { get; set; }

    DbSet<ScheduleSlot> ScheduleSlots { get; set; }

    DbSet<Booking> Bookings { get; set; }

    DbSet<BookingRoom> BookingRooms { get; set; }
    
    DbSet<BookingSlot> BookingSlots { get; set; }
}
