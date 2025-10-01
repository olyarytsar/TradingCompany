using System;
using TradingCompany.DAL.Concrete;
using TradingCompany.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TradingCompany.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Рядок підключення до бази даних (з пробілом у назві)
            string connStr = "Data Source = localhost,1433;Database=Trading Company;Persist Security Info = True; User ID = sa; Password = MyStr0ng!Pass123; Pooling = False; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = True;";


            // 2. Створення DAL (без мапера можна передати null або прибрати зовсім у конструкторі GenericDAL)
            var orderDAL = new OrderDAL(connStr, null);

            try
            {
                // 3. Виклик GetAll() для отримання всіх замовлень
                var orders = orderDAL.GetAll();

                // 4. Вивід результатів на консоль
                foreach (var order in orders)
                {
                    Console.WriteLine($"OrderId: {order.OrderId}, EmployeeId: {order.EmployeeId}, Date: {order.OrderDate}, Total: {order.TotalAmount}");
                }

                if (orders.Count == 0)
                {
                    Console.WriteLine("Замовлень у таблиці dbo.Orders немає.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Сталася помилка:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}

