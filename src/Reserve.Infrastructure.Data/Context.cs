using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options), IContext
{
    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<ScheduleRoom> ScheduleRooms { get; set; }

    public virtual DbSet<ScheduleSlot> ScheduleSlots { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingRoom> BookingRooms { get; set; }

    public virtual DbSet<BookingSlot> BookingSlots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .LogTo(
                Console.WriteLine,
                LogLevel.Information);
    }
}
