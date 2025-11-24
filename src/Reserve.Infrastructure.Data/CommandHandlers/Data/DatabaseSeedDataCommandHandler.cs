using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;
using Reserve.Application.Results;

namespace Reserve.Infrastructure.Data.CommandHandlers.Data;

public class DatabaseSeedDataCommandHandler : IDatabaseSeedDataCommandHandler
{
    private readonly Context context;
    
    private readonly ILogger<DatabaseSeedDataCommandHandler> logger;

    public DatabaseSeedDataCommandHandler(Context context, ILogger<DatabaseSeedDataCommandHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task<Result<bool>> Handle(DatabaseSeedDataCommand command, CancellationToken token)
    {
        var sql = @"
            DECLARE @hotel1 UNIQUEIDENTIFIER = 'a1b2c3d4-e5f6-7890-abcd-ef1234567890';
            DECLARE @hotel2 UNIQUEIDENTIFIER = 'b2c3d4e5-f6a7-8901-bcde-f23456789012';

            DECLARE @room1 UNIQUEIDENTIFIER = 'd4ed1b54-6c29-4a94-9c8f-7f5a36f3d601';
            DECLARE @room2 UNIQUEIDENTIFIER = '94a6bf17-65e3-4c78-9bf0-96c8cba5b2b4';
            DECLARE @room3 UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000003';
            DECLARE @room4 UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000004';
            DECLARE @room5 UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000005';
            DECLARE @room6 UNIQUEIDENTIFIER = '00000000-0000-0000-0000-000000000006';

            DECLARE @slotFri UNIQUEIDENTIFIER = '60e8155a-2b1d-4e8b-875a-1b3f7de79c9c';
            DECLARE @slotSat UNIQUEIDENTIFIER = 'eac6a7e6-0fc7-4aa9-b987-0ed1d1b6c6fd';
            DECLARE @slotSun UNIQUEIDENTIFIER = 'f8b0fdd5-6bf7-4b1f-a6cf-123364c3f3c1';

            DECLARE @schedule UNIQUEIDENTIFIER = '5cbdf1ef-0d5a-4bb0-91a6-957c39fd9c2c';

            DECLARE @booking UNIQUEIDENTIFIER = '2b4161ec-c329-4898-8c7e-3f7234ab9ec9';

            ------------------------------------------------------------
            -- Cleanup to keep script repeatable
            ------------------------------------------------------------
            DELETE FROM BookingSlots
            DELETE FROM BookingRooms
            DELETE FROM Bookings
            DELETE FROM ScheduleSlots
            DELETE FROM ScheduleRooms
            DELETE FROM Schedules
            DELETE FROM Slots
            DELETE FROM Rooms
            DELETE FROM Hotels
            DELETE FROM RoomTypes

            ------------------------------------------------------------
            -- Room Types
            ------------------------------------------------------------
            INSERT INTO RoomTypes (Id, DisplayName, MaxOccupancy)
            VALUES
                ('Single', 'Single Room', 1),
                ('Double', 'Double Room', 2),
                ('Deluxe', 'Deluxe Room', 4)

            ------------------------------------------------------------
            -- Hotels
            ------------------------------------------------------------
            INSERT INTO Hotels (Id, Name)
            VALUES
                (@hotel1, 'Grand Reserve Hotel'),
                (@hotel2, 'Hotel Bristol');

            ------------------------------------------------------------
            -- Rooms (Rooms linked to Hotel and RoomType)
            ------------------------------------------------------------
            INSERT INTO Rooms (Id, Name, [Start], [End], HotelId, RoomTypeId)
            VALUES
                (@room1, 'Room 101', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Deluxe'),
                (@room2, 'Room 102', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Deluxe'),
                (@room3, 'Room 103', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Double'),
                (@room4, 'Room 104', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Double'),
                (@room5, 'Room 105', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Single'),
                (@room6, 'Room 106', '2026-03-27T00:00:00', '2027-03-27T00:00:00', @hotel1, 'Single');

            ------------------------------------------------------------
            -- Slots
            ------------------------------------------------------------
            INSERT INTO Slots (Id, Name, [Start], [End])
            VALUES
                (@slotFri, 'Fri', '2026-06-05T15:00:00', '2026-06-06T11:00:00'),
                (@slotSat, 'Sat', '2026-06-06T15:00:00', '2026-06-07T11:00:00'),
                (@slotSun, 'Sun', '2026-06-07T15:00:00', '2026-06-08T11:00:00');

            ------------------------------------------------------------
            -- Schedule + links
            ------------------------------------------------------------
            INSERT INTO Schedules (Id, Name, [Start], [End])
            VALUES (@schedule, 'Default', '2026-03-27T00:00:00', '2027-03-27T00:00:00');

            INSERT INTO ScheduleRooms (Id, ScheduleId, RoomId)
            VALUES
                ('1f4cc4da-3d5f-4d37-9120-f0ddc1e9a8f2', @schedule, @room1),
                ('7db2f5f6-d5c8-4b80-ae9e-0af7a1a7b4e6', @schedule, @room2),
                ('c3e5f4b1-8f4e-4d2a-9f3e-2b6d5e4c9a7b', @schedule, @room3),
                ('d2a1b6c3-5e4f-4a9b-8c7d-1e2f3a4b5c6d', @schedule, @room4),
                ('e4c9a7b6-1b2c-3d4e-5f6a-7b8c9d0e1f2a', @schedule, @room5),
                ('f6a7b8c9-d0e1-f2a3-b4c5-d6e7f8a9b0c1', @schedule, @room6);

            INSERT INTO ScheduleSlots (Id, ScheduleId, SlotId)
            VALUES
                ('82f4c6e0-33f4-4a44-ac98-90f8828e3ac5', @schedule, @slotFri),
                ('3f6a3a18-8787-4c1e-a6ca-6f7a197e2be1', @schedule, @slotSat),
                ('a3ed0e26-1924-4b8d-b8a3-7b3325c4f6d1', @schedule, @slotSun);

            ------------------------------------------------------------
            -- Booking occupying Room 1 on the Friday slot
            ------------------------------------------------------------
            INSERT INTO Bookings (Id, Name, [Start], [End])
            VALUES
                (@booking, 'Booking 1', '2026-06-05T15:00:00', '2026-06-06T11:00:00');

            INSERT INTO BookingRooms (Id, BookingId, RoomId)
            VALUES ('16a8a3f4-6d03-4c5a-9f0a-a7a5cd3cb8ea', @booking, @room1);

            INSERT INTO BookingSlots (Id, BookingId, SlotId)
            VALUES ('dd1f4561-4af7-44ae-9cd4-6db0b456f73d', @booking, @slotFri);
            ";

        var rowsModified = context.Database.ExecuteSqlRaw(sql);

        return Task.FromResult<Result<bool>>(new SuccessResult<bool>(true));
    }
}
