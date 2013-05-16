using System;

namespace EventSourcing
{
    internal class CustomerNameChangedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CustomerNameChangedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}