using System;
using TradingCompany.ConsoleApp.Commands.Interfaces;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class InsertCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;

        public InsertCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Insert new {typeof(T).Name}";

        public void Execute()
        {
            var item = new T();
            Console.WriteLine($"Enter properties for new {typeof(T).Name} manually in code (implement later).");
           
            _dal.Create(item);
            Console.WriteLine($"{typeof(T).Name} created.");
        }
    }
}
