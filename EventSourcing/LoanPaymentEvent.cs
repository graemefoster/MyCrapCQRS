using System;

namespace EventSourcing
{
    internal class LoanPaymentEvent : IEvent
    {

        public LoanPaymentEvent(Guid id, decimal interestChange, decimal principalChange)
        {
            InterestChange = interestChange;
            PrincipalChange = principalChange;
            Id = id;
        }

        public decimal InterestChange { get; set; }
        public decimal PrincipalChange { get; set; }
        public Guid Id { get; set; }
    }
}