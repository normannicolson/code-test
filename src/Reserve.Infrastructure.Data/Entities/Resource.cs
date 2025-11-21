using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

public class Resource
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }

    [StringLength(256)]
    public required DateTimeOffset Start { get; set; }

    public required DateTimeOffset End { get; set; }

    public virtual ICollection<ScheduleResource> ScheduleResources { get; set; } = new List<ScheduleResource>();

    public virtual ICollection<BookingResource> BookingResources { get; set; } = new List<BookingResource>();
}
