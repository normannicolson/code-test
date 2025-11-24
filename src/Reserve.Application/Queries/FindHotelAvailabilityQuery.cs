using System;

namespace Reserve.Application.Queries;

public sealed record FindHotelAvailabilityQuery
{
    public Guid HotelId { get; }

    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public int NumberOfPeople { get; }

    public FindHotelAvailabilityQuery(
        Guid hotelId,
        DateTimeOffset start,
        DateTimeOffset end,
        int numberOfPeople = 2)
    {
        this.HotelId = hotelId;
        this.Start = start;
        this.End = end;
        this.NumberOfPeople = numberOfPeople;
    }
}