using AutoMapper;
using TradingCompany.BLL.Concrete;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.MVC.App.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

namespace TradingCompany.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddLog4Net("log4net.config");
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { new CultureInfo("en-US") };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            builder.Services.AddSingleton<IMapper>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.ConstructServicesUsing(sp.GetService);
                    cfg.AddMaps(typeof(ProductMap).Assembly, typeof(CategoryListItemProfile).Assembly);
                }, loggerFactory);

                return config.CreateMapper();
            });

            string connStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

            builder.Services.AddTransient<IProductDAL>(sp => new ProductDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<ICategoryDAL>(sp => new CategoryDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<ISupplierDAL>(sp => new SupplierDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IEmployeeDAL>(sp => new EmployeeDALEF(connStr, sp.GetRequiredService<IMapper>()))
                            .AddTransient<IProductManager, ProductManager>()
                            .AddTransient<IAuthManager, AuthManager>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Account/Forbidden/";
                options.LoginPath = "/Account/Login/";
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            app.UseRequestLocalization();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}