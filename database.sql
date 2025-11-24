IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [Bookings] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Start] datetimeoffset NOT NULL,
        [End] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Bookings] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [Hotels] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        CONSTRAINT [PK_Hotels] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [RoomTypes] (
        [Id] nvarchar(32) NOT NULL,
        [DisplayName] nvarchar(128) NOT NULL,
        [MaxOccupancy] int NOT NULL,
        CONSTRAINT [PK_RoomTypes] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [Schedules] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(256) NOT NULL,
        [Start] datetimeoffset NOT NULL,
        [End] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Schedules] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [Slots] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Start] datetimeoffset NOT NULL,
        [End] datetimeoffset NOT NULL,
        CONSTRAINT [PK_Slots] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [Rooms] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Start] datetimeoffset NOT NULL,
        [End] datetimeoffset NOT NULL,
        [HotelId] uniqueidentifier NOT NULL,
        [RoomTypeId] nvarchar(32) NOT NULL,
        CONSTRAINT [PK_Rooms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Rooms_Hotels_HotelId] FOREIGN KEY ([HotelId]) REFERENCES [Hotels] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Rooms_RoomTypes_RoomTypeId] FOREIGN KEY ([RoomTypeId]) REFERENCES [RoomTypes] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [BookingSlots] (
        [Id] uniqueidentifier NOT NULL,
        [BookingId] uniqueidentifier NOT NULL,
        [SlotId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_BookingSlots] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BookingSlots_Bookings_BookingId] FOREIGN KEY ([BookingId]) REFERENCES [Bookings] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BookingSlots_Slots_SlotId] FOREIGN KEY ([SlotId]) REFERENCES [Slots] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [ScheduleSlots] (
        [Id] uniqueidentifier NOT NULL,
        [ScheduleId] uniqueidentifier NOT NULL,
        [SlotId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ScheduleSlots] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ScheduleSlots_Schedules_ScheduleId] FOREIGN KEY ([ScheduleId]) REFERENCES [Schedules] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ScheduleSlots_Slots_SlotId] FOREIGN KEY ([SlotId]) REFERENCES [Slots] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [BookingRooms] (
        [Id] uniqueidentifier NOT NULL,
        [BookingId] uniqueidentifier NOT NULL,
        [RoomId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_BookingRooms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BookingRooms_Bookings_BookingId] FOREIGN KEY ([BookingId]) REFERENCES [Bookings] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BookingRooms_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Rooms] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE TABLE [ScheduleRooms] (
        [Id] uniqueidentifier NOT NULL,
        [ScheduleId] uniqueidentifier NOT NULL,
        [RoomId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_ScheduleRooms] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ScheduleRooms_Rooms_RoomId] FOREIGN KEY ([RoomId]) REFERENCES [Rooms] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ScheduleRooms_Schedules_ScheduleId] FOREIGN KEY ([ScheduleId]) REFERENCES [Schedules] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE UNIQUE INDEX [IX_BookingRooms_BookingId] ON [BookingRooms] ([BookingId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_BookingRooms_RoomId] ON [BookingRooms] ([RoomId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_BookingSlots_BookingId] ON [BookingSlots] ([BookingId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_BookingSlots_SlotId] ON [BookingSlots] ([SlotId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_Rooms_HotelId] ON [Rooms] ([HotelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_Rooms_RoomTypeId] ON [Rooms] ([RoomTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_ScheduleRooms_RoomId] ON [ScheduleRooms] ([RoomId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_ScheduleRooms_ScheduleId] ON [ScheduleRooms] ([ScheduleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_ScheduleSlots_ScheduleId] ON [ScheduleSlots] ([ScheduleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    CREATE INDEX [IX_ScheduleSlots_SlotId] ON [ScheduleSlots] ([SlotId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20251124101120_CreateDatabase'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251124101120_CreateDatabase', N'10.0.0');
END;

COMMIT;
GO