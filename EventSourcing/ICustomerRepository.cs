using System;

namespace EventSourcing
{
    internal interface ICustomerRepository
    {
        Customer Get(Guid id);
    }
}