using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

    public class Schedule
    {
        public required Guid Id { get; set; }

        [StringLength(256)]
        public required string Name { get; set; }

        public required DateTimeOffset Start { get; set; }

        public required DateTimeOffset End { get; set; }

        public virtual ICollection<ScheduleResource> ScheduleResources { get; set; } = new List<ScheduleResource>();

        public virtual ICollection<ScheduleSlot> ScheduleSlots { get; set; } = new List<ScheduleSlot>();
    }
