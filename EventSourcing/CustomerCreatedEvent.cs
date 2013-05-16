using System;

namespace EventSourcing
{
    internal class CustomerCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public CustomerCreatedEvent(Guid id, string name, DateTime dateOfBirth)
        {
            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}