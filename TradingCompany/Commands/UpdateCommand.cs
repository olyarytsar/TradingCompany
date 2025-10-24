using System;
using System.Reflection;
using TradingCompany.ConsoleApp.Commands.Interfaces;
using TradingCompany.DAL.Interfaces;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class UpdateCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;

        public UpdateCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Update {typeof(T).Name}";

        public void Execute()
        {
            Console.Write($"Enter ID of the {typeof(T).Name} to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Invalid ID format.");
                return;
            }

            var item = _dal.GetById(id);
            if (item == null)
            {
                Console.WriteLine($"{typeof(T).Name} with ID {id} not found.");
                return;
            }

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanWrite)
                    continue;

                if (prop.Name.Equals($"{typeof(T).Name}Id", StringComparison.OrdinalIgnoreCase) ||
                    prop.PropertyType == typeof(DateTime) ||
                    prop.PropertyType == typeof(DateTime?) ||
                    (!prop.PropertyType.IsPrimitive &&
                     prop.PropertyType != typeof(string) &&
                     prop.PropertyType != typeof(decimal) &&
                     prop.PropertyType != typeof(double) &&
                     prop.PropertyType != typeof(float) &&
                     prop.PropertyType != typeof(bool) &&
                     Nullable.GetUnderlyingType(prop.PropertyType) == null))
                {
                    continue;
                }

                while (true)
                {
                    var currentValue = prop.GetValue(item);
                    Console.Write($"Enter new value for {prop.Name} (current: {currentValue}): ");
                    var input = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(input))
                        break; 

                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                        object convertedValue = targetType switch
                        {
                            Type t when t == typeof(int) => int.Parse(input),
                            Type t when t == typeof(decimal) => decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                            Type t when t == typeof(double) => double.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                            Type t when t == typeof(float) => float.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                            Type t when t == typeof(bool) => input.Trim().ToLower() == "true" || input.Trim() == "1",
                            Type t when t == typeof(string) => input,
                            _ => throw new InvalidOperationException($"Unsupported property type: {prop.PropertyType}")
                        };

                        prop.SetValue(item, convertedValue);
                        break; 
                    }
                    catch
                    {
                        Console.WriteLine($"Invalid value for {prop.Name}. Try again.");
                    }
                }
            }

            try
            {
                _dal.Update(item);
                Console.WriteLine($"{typeof(T).Name} with ID {id} updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating {typeof(T).Name}: {ex.Message}");
                if (ex.InnerException != null)
                    Console.WriteLine($"Inner: {ex.InnerException.Message}");
            }
        }
    }
}
