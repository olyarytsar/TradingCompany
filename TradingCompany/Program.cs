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
            // Налаштування AutoMapper
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

            // Зчитування connection string
            string connectionString = "Data Source=localhost,1433;Database=Trading Company;Persist Security Info=True;User ID=sa;Password=MyStr0ng!Pass123;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

            // Запуск меню
            new AppMenuService(connectionString, mapper).Show();
        }
    }
}

