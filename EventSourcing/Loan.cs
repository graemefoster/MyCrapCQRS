using System;
using System.Collections.Generic;

namespace EventSourcing
{
    internal class Loan : IReplayEvents
    {
        internal static IDictionary<Type, Delegate> EventHandlers = new Dictionary<Type, Delegate>
            {
                {
                    typeof (LoanCreatedEvent),
                    new Action<Loan, LoanCreatedEvent>((c, t) => c.ApplyCustomerCreatedEvent(t))
                },
                {
                    typeof (LoanNameChangedEvent),
                    new Action<Loan, LoanNameChangedEvent>((c, t) => c.ApplyCustomerNameChangedEvent(t))
                },
                {
                    typeof (LoanPaymentEvent),
                    new Action<Loan, LoanPaymentEvent>((c, t) => c.ApplyLoanPaymentEvent(t))
                }            };

        private void ApplyLoanPaymentEvent(LoanPaymentEvent loanPaymentEvent)
        {
            _principal += loanPaymentEvent.PrincipalChange;
            _interest += loanPaymentEvent.InterestChange;
        }

        private DateTime _dateOfBirth;
        private Guid _id;
        private string _name;
        private decimal _principal;
        private decimal _interest;

        /// <summary>
        ///     could be private and reflected on
        /// </summary>
        public Loan()
        {
        }

        public Loan(Guid id, string name, DateTime dateOfBirth)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            ApplyChanges(EventStore.Create(new LoanCreatedEvent(id, name, dateOfBirth)));
        }

        public void ApplyChanges(IEvent @event)
        {
            EventHandlers[@event.GetType()].DynamicInvoke(this, @event);
        }

        private void ApplyCustomerCreatedEvent(LoanCreatedEvent @event)
        {
            _id = @event.Id;
            _dateOfBirth = @event.DateOfBirth;
            _name = @event.Name;
            _principal = 10;
            _interest = 3;
            //can ignore properties if you want to
        }

        private void ApplyCustomerNameChangedEvent(LoanNameChangedEvent @event)
        {
            _name = @event.Name;
        }

        public void ChangeName(string name)
        {
            ApplyChanges(EventStore.Create(new LoanNameChangedEvent(_id, name)));
        }

        public override string ToString()
        {
            return string.Format(@"Loan {2}
    Name: {0} 
    Date of Birth: {1}
    Principal: {3}
    Interest: {4}", _name, _dateOfBirth, _id, _principal, _interest);
        }

        public void Pay(decimal payment)
        {
            ApplyChanges(EventStore.Create(new LoanPaymentEvent(this._id, -2, -(payment - 2))));
        }
    }
}