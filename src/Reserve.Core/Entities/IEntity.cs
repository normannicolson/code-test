using System;

namespace Reserve.Core.Entities;

public interface IEntity
{
    Guid Id { get; set; }

    long Version { get; set; }
}