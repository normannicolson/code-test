using System;

namespace Reserve.Application.Queries;

public sealed record BookingGetQuery
{
    public Guid Id { get; }

    public BookingGetQuery(
        Guid id)
    {
        this.Id = id;
    }
}