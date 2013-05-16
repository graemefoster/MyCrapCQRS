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
            RegisterCommand<ChangeCustomerNameCommand>(c => ChangeCustomerNameCommandHandler(new CustomerRepository(), c));

            Handle(new CreateCustomerCommand("Graeme Foster", new DateTime(1975, 7, 2)));

            //will learn how to get the id tomorrow. Sorry for the hack :)
            Handle(new ChangeCustomerNameCommand(EventStore.HackGetCustomerId(), "Fred Fibnar"));

            //Loads the customer at its latest state purely by replaying events.
            Customer customer = new CustomerRepository().Get(EventStore.HackGetCustomerId());

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine();
            Console.WriteLine("Loading by replaying events");
            Console.WriteLine();
            Console.WriteLine(customer.ToString());
            Console.WriteLine();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }

        //Could be classes - just playing with a simple way of doing them.
        private static void ChangeCustomerNameCommandHandler(
            ICustomerRepository customerRepository,
            ChangeCustomerNameCommand changeCustomerNameCommand)
        {
            Customer customer = customerRepository.Get(changeCustomerNameCommand.Id);
            customer.ChangeName(changeCustomerNameCommand.Name);
        }

        private static void Handle(object command)
        {
            //Being lazy - using dynamic!!
            CommandHandlers[command.GetType()]((dynamic) command);
        }

        private static void CreateCustomerCommandHandler(ISomeOtherService otherService, CreateCustomerCommand command)
        {
            //Parameter injection via functional composition
            var customer = new Customer(command.Name, command.DateOfBirth);
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