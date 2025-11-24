using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reserve.Application.CommandHandlers.Data;
using Reserve.Application.Commands.Data;
using Reserve.Application.Results;

namespace Reserve.Infrastructure.Data.CommandHandlers.Data;

public class DatabaseResetDataCommandHandler : IDatabaseResetDataCommandHandler
{
    private readonly ILogger<DatabaseResetDataCommandHandler> logger;

    private readonly Context context;

    public DatabaseResetDataCommandHandler(Context context, ILogger<DatabaseResetDataCommandHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task<Result<bool>> Handle(DatabaseResetDataCommand command, CancellationToken token)
    {
        var sql = @"
            DELETE FROM BookingSlots;
            DELETE FROM BookingRooms;
            DELETE FROM Bookings;
            DELETE FROM ScheduleSlots;
            DELETE FROM ScheduleRooms;
            DELETE FROM Schedules;
            DELETE FROM Slots;
            DELETE FROM Rooms;
            DELETE FROM Hotels;
            DELETE FROM RoomTypes;
            ";

        var rowsModified = context.Database.ExecuteSqlRaw(sql);

        return Task.FromResult<Result<bool>>(new SuccessResult<bool>(true));
    }
}
