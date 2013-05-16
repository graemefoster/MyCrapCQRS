using System;

namespace EventSourcing
{
    internal class CustomerRepository : ICustomerRepository
    {
        public Customer Get(Guid id)
        {
            return EventStore.Get<Customer>(id);
        }
    }
}