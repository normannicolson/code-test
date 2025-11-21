# Create Database 

```
docker run --name sqlserver -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=yourStrong!Passw0rd' -p 1433:1433 -v sqlvolume:/var/opt/mssql -d mcr.microsoft.com/mssql/server:2025-latest
```

```
DECLARE @room1 UNIQUEIDENTIFIER = 'd4ed1b54-6c29-4a94-9c8f-7f5a36f3d601';
DECLARE @room2 UNIQUEIDENTIFIER = '94a6bf17-65e3-4c78-9bf0-96c8cba5b2b4';
DECLARE @slotFri UNIQUEIDENTIFIER = '60e8155a-2b1d-4e8b-875a-1b3f7de79c9c';
DECLARE @slotSat UNIQUEIDENTIFIER = 'eac6a7e6-0fc7-4aa9-b987-0ed1d1b6c6fd';
DECLARE @slotSun UNIQUEIDENTIFIER = 'f8b0fdd5-6bf7-4b1f-a6cf-123364c3f3c1';
DECLARE @schedule UNIQUEIDENTIFIER = '5cbdf1ef-0d5a-4bb0-91a6-957c39fd9c2c';
DECLARE @booking UNIQUEIDENTIFIER = '2b4161ec-c329-4898-8c7e-3f7234ab9ec9';

------------------------------------------------------------
-- Cleanup to keep script repeatable
------------------------------------------------------------
DELETE FROM BookingSlots
DELETE FROM BookingResources
DELETE FROM Bookings
DELETE FROM ScheduleSlots
DELETE FROM ScheduleResources
DELETE FROM Schedules
DELETE FROM Slots
DELETE FROM Resources

------------------------------------------------------------
-- Resources
------------------------------------------------------------
INSERT INTO Resources (Id, Name, [Start], [End])
VALUES
    (@room1, 'Room 1', '2026-03-01T00:00:00', '2027-03-01T00:00:00'),
    (@room2, 'Room 2', '2026-03-01T00:00:00', '2027-03-01T00:00:00');

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

INSERT INTO ScheduleResources (Id, ScheduleId, ResourceId)
VALUES
    ('1f4cc4da-3d5f-4d37-9120-f0ddc1e9a8f2', @schedule, @room1),
    ('7db2f5f6-d5c8-4b80-ae9e-0af7a1a7b4e6', @schedule, @room2);

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

INSERT INTO BookingResources (Id, BookingId, ResourceId)
VALUES ('16a8a3f4-6d03-4c5a-9f0a-a7a5cd3cb8ea', @booking, @room1);

INSERT INTO BookingSlots (Id, BookingId, SlotId)
VALUES ('dd1f4561-4af7-44ae-9cd4-6db0b456f73d', @booking, @slotFri);

------------------------------------------------------------
-- Query  
------------------------------------------------------------

DECLARE @start DATETIME = '2026-06-05T15:00:00';
DECLARE @end   DATETIME = '2026-06-06T11:00:00';

WITH AvailableSlots AS (
    SELECT 
        Resources.Id AS ResourceId,
        Resources.Name,
        Slots.Id AS SlotId,
        Slots.[Start],
        Slots.[End]
    FROM Resources
    INNER JOIN ScheduleResources ON ScheduleResources.ResourceId = Resources.Id
    INNER JOIN Schedules ON Schedules.Id = ScheduleResources.ScheduleId
    INNER JOIN ScheduleSlots ON ScheduleSlots.ScheduleId = Schedules.Id
    INNER JOIN Slots ON Slots.Id = ScheduleSlots.SlotId
    WHERE Slots.[Start] <= @end 
      AND Slots.[End] >= @start
      AND NOT EXISTS (
          SELECT 1
          FROM Bookings
          INNER JOIN BookingSlots ON BookingSlots.BookingId = Bookings.Id
          INNER JOIN BookingResources ON BookingResources.BookingId = Bookings.Id
          WHERE BookingResources.ResourceId = Resources.Id
            AND BookingSlots.SlotId = Slots.Id
            AND Bookings.[Start] <= @end
            AND Bookings.[End] >= @start
      )
),
ResourceCoverage AS (
    SELECT 
        ResourceId,
        Name,
        MIN([Start]) AS EarliestStart,
        MAX([End]) AS LatestEnd
    FROM AvailableSlots
    GROUP BY ResourceId, Name
)
SELECT *
FROM ResourceCoverage
WHERE EarliestStart <= @start 
  AND LatestEnd >= @end; 
```