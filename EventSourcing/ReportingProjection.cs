using System;
using System.Collections.Generic;

namespace EventSourcing
{
    internal class ReportingProjection: IReplayEvents
    {
        private decimal _liability;

        internal static IDictionary<Type, Delegate> EventHandlers = new Dictionary<Type, Delegate>
            {
                {
                    typeof (LoanCreatedEvent),
                    new Action<ReportingProjection, LoanCreatedEvent>((c, t) => c.ApplyCustomerCreatedEvent(t))
                },
                {
                    typeof (LoanPaymentEvent),
                    new Action<ReportingProjection, LoanPaymentEvent>((c, t) => c.ApplyLoanPaymentEvent(t))
                }            };

        private void ApplyCustomerCreatedEvent(LoanCreatedEvent loanCreatedEvent)
        {
            _liability += loanCreatedEvent.Principal + loanCreatedEvent.Interest;
        }

        private void ApplyLoanPaymentEvent(LoanPaymentEvent loanPaymentEvent)
        {
            _liability += loanPaymentEvent.PrincipalChange + loanPaymentEvent.InterestChange;
        }

        public void ApplyChanges(IEvent @event)
        {
            Delegate handler;
            if (EventHandlers.TryGetValue(@event.GetType(), out handler))
            {
                handler.DynamicInvoke(this, @event);
            }
        }

        public override string ToString()
        {
            return "Loan reporting model - total liability:" + _liability;
        }
    }
}