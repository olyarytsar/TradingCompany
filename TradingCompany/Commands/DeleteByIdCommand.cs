using System;
using TradingCompany.DAL.Interfaces;
using TradingCompany.ConsoleApp.Commands.Interfaces;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class DeleteByIdCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;

        public DeleteByIdCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Delete {typeof(T).Name} by ID";

        public void Execute()
        {
            Console.Write("Enter ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var item = _dal.GetById(id);
            if (item == null)
            {
                Console.WriteLine($"{typeof(T).Name} with ID {id} not found.");
                return;
            }

            Console.Write($"Are you sure you want to delete {typeof(T).Name} with ID {id}? (y/n): ");
            var confirm = Console.ReadLine();
            if (confirm?.ToLower() != "y")
            {
                Console.WriteLine("Delete cancelled.");
                return;
            }

            var success = _dal.Delete(id);
            if (success)
            {
                Console.WriteLine($"{typeof(T).Name} with ID {id} deleted successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to delete {typeof(T).Name} with ID {id}.");
            }
        }
    }
}
