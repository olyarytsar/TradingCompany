using AutoMapper;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO; 
using System.Windows;
using TradingCompany.BLL.Concrete;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;
using TradingCompany.WPF.ViewModels;
using TradingCompany.WPF.Windows;

namespace TradingCompany.WPF
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }

    

        protected override void OnStartup(StartupEventArgs e)
        {
            Services = BuildServiceProvider();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.DataContext = Services.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettingss.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("DefaultConnection");


            var services = new ServiceCollection();
            services.AddLogging(builder => { builder.AddConsole(); });

            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<TradingCompany.DALEF.Models.Role, Role>().ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role1)).ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.Category, Category>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category1)).ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.Employee, Employee>().ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.Product, Product>().ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.Supplier, Supplier>().ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.Order, Order>().ReverseMap();
                cfg.CreateMap<TradingCompany.DALEF.Models.OrderItem, OrderItem>().ReverseMap();
            });

            services.AddTransient<IEmployeeDAL>(sp => new EmployeeDALEF(connectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IProductDAL>(sp => new ProductDALEF(connectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<ISupplierDAL>(sp => new SupplierDALEF(connectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IOrderDAL>(sp => new OrderDALEF(connectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IOrderItemDAL>(sp => new OrderItemDALEF(connectionString, sp.GetRequiredService<IMapper>()));

            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IProductManager, ProductManager>();
            services.AddTransient<ISupplyManager, SupplyManager>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<WarehouseManagerViewModel>();
            services.AddTransient<CreateOrderViewModel>();
            services.AddTransient<ActiveOrdersViewModel>();

            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}