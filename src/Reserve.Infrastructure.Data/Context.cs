using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data;

public class Context(DbContextOptions<Context> options) : DbContext(options)
{
    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<ScheduleResource> ScheduleResources { get; set; }

    public virtual DbSet<ScheduleSlot> ScheduleSlots { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingResource> BookingResources { get; set; }

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
