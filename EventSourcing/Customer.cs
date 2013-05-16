using System;
using System.Collections.Generic;

namespace EventSourcing
{
    internal class Customer : IReplayEvents
    {
        internal static IDictionary<Type, Delegate> EventHandlers = new Dictionary<Type, Delegate>
            {
                {
                    typeof (CustomerCreatedEvent),
                    new Action<Customer, CustomerCreatedEvent>((c, t) => c.ApplyCustomerCreatedEvent(t))
                },
                {
                    typeof (CustomerNameChangedEvent),
                    new Action<Customer, CustomerNameChangedEvent>((c, t) => c.ApplyCustomerNameChangedEvent(t))
                },
            };

        private DateTime _dateOfBirth;
        private Guid _id;
        private string _name;

        /// <summary>
        ///     could be private and reflected on
        /// </summary>
        public Customer()
        {
        }

        public Customer(string name, DateTime dateOfBirth)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ApplyChanges(EventStore.Create(new CustomerCreatedEvent(Guid.NewGuid(), name, dateOfBirth)));
        }

        public void ApplyChanges(IEvent @event)
        {
            EventHandlers[@event.GetType()].DynamicInvoke(this, @event);
        }

        private void ApplyCustomerCreatedEvent(CustomerCreatedEvent @event)
        {
            _id = @event.Id;
            _dateOfBirth = @event.DateOfBirth;
            _name = @event.Name;
            //can ignore properties if you want to
        }

        private void ApplyCustomerNameChangedEvent(CustomerNameChangedEvent @event)
        {
            _name = @event.Name;
        }

        public void ChangeName(string name)
        {
            ApplyChanges(EventStore.Create(new CustomerNameChangedEvent(_id, name)));
        }

        public override string ToString()
        {
            return string.Format(@"Customer {2}
    Name: {0} 
    Date of Birth: {1}", _name, _dateOfBirth, _id);
        }
    }
}