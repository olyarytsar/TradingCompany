using System;

namespace TradingCompany.WPF.Interfaces
{
    public interface ICloseable
    {
        Action Close { get; set; }
    }
}