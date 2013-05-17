using System;

namespace EventSourcing
{
    internal class LoanRepository : ILoanRepository
    {
        public Loan Get(Guid id)
        {
            return EventStore.Get<Loan>(id);
        }
    }
}