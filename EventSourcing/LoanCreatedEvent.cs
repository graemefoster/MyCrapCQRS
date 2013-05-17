using System;

namespace EventSourcing
{
    internal class LoanCreatedEvent : IEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }

        public LoanCreatedEvent(Guid id, string name, DateTime dateOfBirth, decimal principal, decimal interest)
        {
            Id = id;
            Name = name;
            DateOfBirth = dateOfBirth;
            Principal = principal;
            Interest = interest;
        }
    }
}