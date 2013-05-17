using System;

namespace EventSourcing
{
    internal interface ILoanRepository
    {
        Loan Get(Guid id);
    }
}