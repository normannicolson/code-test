using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Infrastructure.Data.Entities;

namespace Reserve.Infrastructure.Data;

public class Context : DbContext, IContext
{
    public Context()
    { 
    }

    public Context(DbContextOptions<Context> options)
        : base(options)
    { 
    }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<ScheduleRoom> ScheduleRooms { get; set; }

    public virtual DbSet<ScheduleSlot> ScheduleSlots { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingRoom> BookingRooms { get; set; }

    public virtual DbSet<BookingSlot> BookingSlots { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder
            .LogTo(
                Console.WriteLine,
                LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(t => t.GetForeignKeys()))
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
        
        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.Property(r => r.Id)
                .HasConversion<string>()
                .HasMaxLength(32);

            entity.Property(r => r.DisplayName)
                .HasMaxLength(128);
        }); 

        base.OnModelCreating(modelBuilder);
    }
}
