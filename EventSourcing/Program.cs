using System;
using System.Collections.Generic;
using System.Transactions;

namespace EventSourcing
{
    internal class Program
    {
        private static readonly IDictionary<Type, dynamic> CommandHandlers = new Dictionary<Type, dynamic>();

        private static void Main()
        {
            //what no ioc...
            RegisterCommand<CreateCustomerCommand>(c => CreateCustomerCommandHandler(new SomeOtherService(), c));
            RegisterCommand<ChangeCustomerNameCommand>(c => ChangeCustomerNameCommandHandler(new LoanRepository(), c));
            RegisterCommand<PayLoanCommand>(c => PayLoanCommandHandler(new LoanRepository(), c));

            var customerId = Guid.NewGuid();
            Handle(new CreateCustomerCommand(customerId, "Graeme Foster", new DateTime(1975, 7, 2)));

            //will learn how to get the id tomorrow. Sorry for the hack :)
            Handle(new ChangeCustomerNameCommand(customerId, "Fred Fibnar"));

            Handle(new PayLoanCommand(customerId, 10m));

            //Loads the Loan at its latest state purely by replaying events.
            Loan loan = new LoanRepository().Get(customerId);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            Console.WriteLine("Loading by replaying events");
            Console.WriteLine();
            Console.WriteLine(loan.ToString());
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        //Could be classes - just playing with a simple way of doing them.
        private static void ChangeCustomerNameCommandHandler(
            ILoanRepository loanRepository,
            ChangeCustomerNameCommand changeCustomerNameCommand)
        {
            Loan loan = loanRepository.Get(changeCustomerNameCommand.Id);
            loan.ChangeName(changeCustomerNameCommand.Name);
        }

        private static void PayLoanCommandHandler(
    ILoanRepository loanRepository,
    PayLoanCommand payLoanCommand)
        {
            Loan loan = loanRepository.Get(payLoanCommand.Id);
            loan.Pay(payLoanCommand.Payment);
        }

        private static void Handle(object command)
        {
            //Being lazy - using dynamic!!
            CommandHandlers[command.GetType()]((dynamic) command);
        }

        private static void CreateCustomerCommandHandler(ISomeOtherService otherService, CreateCustomerCommand command)
        {
            //Parameter injection via functional composition
            var customer = new Loan(command.Id, command.Name, command.DateOfBirth);
        }

        private static Action<T> WrapInTransaction<T>(Action<T> handler)
        {
            return obj =>
            {
                //Just an example of how to compose a more complex command which might use a transaction
                //boundary, or log... basically aop without a framework!
                using (var t = new TransactionScope())
                {
                    handler(obj);
                    t.Complete();
                }
            };
        }

        private static void RegisterCommand<T>(Action<T> commandHandler)
        {
            CommandHandlers.Add(typeof (T), WrapInTransaction(commandHandler));
        }
    }
}