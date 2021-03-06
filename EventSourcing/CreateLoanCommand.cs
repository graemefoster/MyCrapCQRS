﻿using System;

namespace EventSourcing
{
    internal class CreateLoanCommand
    {
        public CreateLoanCommand(Guid customerId, string name, DateTime dateOfBirth)
        {
            Id = customerId;
            Name = name;
            DateOfBirth = dateOfBirth;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    internal class ChangeCustomerNameOnLoanCommand
    {
        public ChangeCustomerNameOnLoanCommand(Guid id, string name)
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