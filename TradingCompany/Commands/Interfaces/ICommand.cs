using System;

namespace TradingCompany.ConsoleApp.Commands.Interfaces
{
    public interface ICommand
    {
        void Execute();
        string Description { get; }
    }
}

