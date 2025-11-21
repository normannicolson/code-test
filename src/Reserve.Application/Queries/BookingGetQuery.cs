using System;

namespace Reserve.Application.Queries;

public sealed class BookingGetQuery
{
    public Guid Id { get; }

    public BookingGetQuery(
        Guid id)
    {
        this.Id = id;
    }
}