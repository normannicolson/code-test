using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Reserve.Infrastructure.Data.Entities;

    public class Schedule
    {
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public required DateTimeOffset Start { get; set; }

        public required DateTimeOffset End { get; set; }

        public virtual ICollection<ScheduleRoom> ScheduleRooms { get; set; } = new List<ScheduleRoom>();

        public virtual ICollection<ScheduleSlot> ScheduleSlots { get; set; } = new List<ScheduleSlot>();
    }
