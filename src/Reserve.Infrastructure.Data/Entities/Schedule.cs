using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

    public class Schedule
    {
        public Guid Id { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        public DateTimeOffset Start { get; set; }

        public DateTimeOffset End { get; set; }

        public virtual ICollection<ScheduleResource> ScheduleResources { get; set; }

        public virtual ICollection<ScheduleSlot> ScheduleSlots { get; set; }
    }
