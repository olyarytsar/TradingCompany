using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.ConsoleApp.AppMenu;


namespace TradingCompany.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<OrderMap>();
            configExpression.AddProfile<ProductMap>();
            configExpression.AddProfile<RoleMap>();
            configExpression.AddProfile<CategoryMap>();
            configExpression.AddProfile<EmployeeMap>();
            configExpression.AddProfile<OrderItemMap>();
            configExpression.AddProfile<SupplierMap>();

            var loggerFactory = NullLoggerFactory.Instance;
            var mapperConfig = new MapperConfiguration(configExpression, loggerFactory);
            IMapper mapper = mapperConfig.CreateMapper();
            
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection");

            new AppMenuService(connectionString, mapper).Show();
        }
    }
}

