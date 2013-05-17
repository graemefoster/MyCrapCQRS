using System;

namespace EventSourcing
{
    internal class CreateCustomerCommand
    {
        public CreateCustomerCommand(string name, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    internal class ChangeCustomerNameCommand
    {
        public ChangeCustomerNameCommand(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    internal class PayLoanCommand
    {
        public PayLoanCommand(Guid id, decimal payment)
        {
            Id = id;
            Payment = payment;
        }

        public Guid Id { get; set; }
        public decimal Payment { get; set; }
    }
}