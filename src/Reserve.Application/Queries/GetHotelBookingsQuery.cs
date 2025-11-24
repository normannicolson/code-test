using System;

namespace Reserve.Application.Queries;

public sealed record GetHotelBookingsQuery
{
    public Guid HotelId { get; }

    public GetHotelBookingsQuery(Guid hotelId)
    {
        this.HotelId = hotelId;
    }
}
