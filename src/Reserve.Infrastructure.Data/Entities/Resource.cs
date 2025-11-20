using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

public class Resource
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    [StringLength(256)]
    public DateTimeOffset Start { get; set; }

    public DateTimeOffset End { get; set; }

    public virtual ICollection<ScheduleResource> ScheduleResources { get; set; }

    public virtual ICollection<BookingResource> BookingResources { get; set; }
}
