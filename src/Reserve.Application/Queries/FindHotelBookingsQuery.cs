using System;

namespace Reserve.Application.Queries;

public sealed record FindHotelBookingsQuery
{
    public Guid HotelId { get; }

    public FindHotelBookingsQuery(Guid hotelId)
    {
        this.HotelId = hotelId;
    }
}
