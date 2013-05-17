using System;

namespace EventSourcing
{
    internal class LoanNameChangedEvent : IEvent
    {
        public LoanNameChangedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}