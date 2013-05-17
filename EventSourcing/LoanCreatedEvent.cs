using System;

namespace EventSourcing
{
    internal class LoanCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public LoanCreatedEvent(Guid id, string name, DateTime dateOfBirth)
        {
            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}