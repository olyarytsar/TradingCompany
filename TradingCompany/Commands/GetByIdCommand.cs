using System;
using System.Reflection;
using TradingCompany.ConsoleApp.Commands.Interfaces;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class GetByIdCommand<T> : ICommand where T : class
    {
        private readonly IGenericDAL<T> _dal;

        public GetByIdCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Get {typeof(T).Name} by ID";

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

            
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(item);
                Console.WriteLine($"{prop.Name}: {value}");
            }
        }
    }
}

