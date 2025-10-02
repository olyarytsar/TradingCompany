using Microsoft.EntityFrameworkCore;
using System;
using TradingCompany.DALEF.Data;
using TradingCompany.DALEF.Models;

namespace TradingCompany.DALEF.Concrete.ctx
{
    public class TradingCompContext : TradCompCtx
    {
        private readonly string _connStr;

        public TradingCompContext(string connStr) : base()
        {
            _connStr = connStr;
        }

        public TradingCompContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(_connStr))
            {
                optionsBuilder.UseSqlServer(_connStr);
            }
        }
    }
}
