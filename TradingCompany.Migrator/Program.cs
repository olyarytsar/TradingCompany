using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TradingCompany.DALEF.Concrete.ctx; // Переконайся, що є посилання на DALEF

namespace TradingCompany.Migrator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ПОЧАТОК МІГРАЦІЇ ПАРОЛІВ ===");

            // 1. Встав сюди свій рядок підключення з App.xaml.cs
            string connectionString = "Data Source=localhost,1433;Database=Trading Company;Persist Security Info=True;User ID=sa;Password=MyStr0ng!Pass123;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;";

            try
            {
                using (var ctx = new TradingCompContext(connectionString))
                {
                    // 2. Шукаємо користувачів, у яких Salt ще NULL (значить пароль старий)
                    var usersToUpdate = ctx.Employees.Where(u => u.Salt == null).ToList();

                    if (usersToUpdate.Count == 0)
                    {
                        Console.WriteLine("Всі користувачі вже мають нові паролі. Міграція не потрібна.");
                    }
                    else
                    {
                        Console.WriteLine($"Знайдено {usersToUpdate.Count} користувачів для оновлення.");

                        foreach (var user in usersToUpdate)
                        {
                            Console.Write($"Обробка користувача '{user.Login}'... ");

                            // А. Генеруємо сіль
                            Guid salt = Guid.NewGuid();

                            // Б. Беремо старий пароль
                            string plainPassword = user.Password;

                            // В. Хешуємо (SHA512)
                            string hashedPassword = HashPassword(plainPassword, salt.ToString());

                            // Г. Записуємо нові дані
                            user.Salt = salt;
                            user.Password = hashedPassword;

                            Console.WriteLine("OK");
                        }

                        // 3. Зберігаємо зміни в базу
                        ctx.SaveChanges();
                        Console.WriteLine("УСПІХ! Всі дані збережено в базу даних.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ПОМИЛКА: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Деталі: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("Натисни Enter, щоб вийти...");
            Console.ReadLine();
        }

        // Той самий метод хешування, що і в EmployeeDALEF
        private static string HashPassword(string password, string salt)
        {
            using (var alg = SHA512.Create())
            {
                var bytes = alg.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}