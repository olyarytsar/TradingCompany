using System;
using TradingCompany.ConsoleApp.Commands.Interfaces;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class UpdateCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;

        public UpdateCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Update {typeof(T).Name}";

        public void Execute()
        {
            Console.Write("Enter ID of the item to update: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var item = _dal.GetById(id);
                if (item != null)
                {
                    Console.WriteLine($"Update properties for {typeof(T).Name} manually in code (implement later).");
                    
                    _dal.Update(item);
                    Console.WriteLine($"{typeof(T).Name} with ID {id} updated.");
                }
                else
                {
                    Console.WriteLine($"{typeof(T).Name} with ID {id} not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
    }
}

