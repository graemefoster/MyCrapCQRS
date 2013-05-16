using System;

namespace EventSourcing
{
    internal interface IEvent
    {
        Guid Id { get; }
    }
}