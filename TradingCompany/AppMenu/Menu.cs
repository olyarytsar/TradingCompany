using System;
using TradingCompany.ConsoleApp.Interfaces;
using TradingCompany.ConsoleApp.Interfaces;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.ConsoleApp.AppMenu
{
    public class Menu<T> where T : class, new()
    {
        private readonly GetAllCommand<T> _getAllCommand;
        private readonly InsertCommand<T> _insertCommand;
        private readonly GetByIdCommand<T> _getByIdCommand;
        private readonly DeleteByIdCommand<T> _deleteByIdCommand;
        private readonly UpdateCommand<T> _updateCommand;

        public Menu(IGenericDAL<T> dal)
        {
            _getAllCommand = new GetAllCommand<T>(dal);
            _insertCommand = new InsertCommand<T>(dal);
            _getByIdCommand = new GetByIdCommand<T>(dal);
            _deleteByIdCommand = new DeleteByIdCommand<T>(dal);
            _updateCommand = new UpdateCommand<T>(dal);
        }

        public void Show()
        {
            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
                Console.WriteLine($"\n{typeof(T).Name} Menu:");
                Console.WriteLine("1 - " + _getAllCommand.Description);
                Console.WriteLine("2 - " + _getByIdCommand.Description);
                Console.WriteLine("3 - " + _insertCommand.Description);
                Console.WriteLine("4 - " + _deleteByIdCommand.Description);
                Console.WriteLine("5 - " + _updateCommand.Description);
                Console.WriteLine("q - Quit");

                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                choice = input[0];

                switch (choice)
                {
                    case '1':
                        _getAllCommand.Execute();
                        break;
                    case '2':
                        _getByIdCommand.Execute();
                        break;
                    case '3':
                        _insertCommand.Execute();
                        break;
                    case '4':
                        _deleteByIdCommand.Execute();
                        break;
                    case '5':
                        _updateCommand.Execute();
                        break;
                    case 'q':
                    case 'Q':
                        Console.WriteLine("Returning to main menu...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
