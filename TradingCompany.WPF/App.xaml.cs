using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private const string ConnectionString = "Data Source=localhost,1433;Database=Trading Company;Persist Security Info=True;User ID=sa;Password=MyStr0ng!Pass123;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

        protected override void OnStartup(StartupEventArgs e)
        {
            Services = BuildServiceProvider();
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            // Просто запускаємо головне вікно
            var mainWindow = Services.GetRequiredService<MainWindow>();
            // Встановлюємо MainViewModel як DataContext
            mainWindow.DataContext = Services.GetRequiredService<MainViewModel>();

            mainWindow.Show();
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddLogging(builder => { builder.AddConsole(); });

            // AutoMapper (ваш старий код мапінгу залишається тут)
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

            // DAL & BLL (ваш старий код залишається)
            services.AddTransient<IEmployeeDAL>(sp => new EmployeeDALEF(ConnectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IProductDAL>(sp => new ProductDALEF(ConnectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<ISupplierDAL>(sp => new SupplierDALEF(ConnectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IOrderDAL>(sp => new OrderDALEF(ConnectionString, sp.GetRequiredService<IMapper>()));
            services.AddTransient<IOrderItemDAL>(sp => new OrderItemDALEF(ConnectionString, sp.GetRequiredService<IMapper>()));

            services.AddTransient<IAuthManager, AuthManager>();
            services.AddTransient<IProductManager, ProductManager>();
            services.AddTransient<ISupplyManager, SupplyManager>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>(); // Головна VM
            services.AddTransient<WarehouseManagerViewModel>();
            services.AddTransient<CreateOrderViewModel>();
            services.AddTransient<ActiveOrdersViewModel>();

            // Windows & Views (Тут реєструємо тільки MainWindow, UserControls реєструвати не обов'язково)
            services.AddTransient<MainWindow>();

            return services.BuildServiceProvider();
        }
    }
}