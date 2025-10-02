﻿using System;
using TradingCompany.ConsoleApp.Commands.Interfaces;
using TradingCompany.DAL.Interfaces;
using System.Reflection;

namespace TradingCompany.ConsoleApp.Interfaces
{
    public class InsertCommand<T> : ICommand where T : class, new()
    {
        private readonly IGenericDAL<T> _dal;

        public InsertCommand(IGenericDAL<T> dal)
        {
            _dal = dal;
        }

        public string Description => $"Insert new {typeof(T).Name}";

        public void Execute()
        {
            var instance = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanWrite) continue;

                
                if (prop.Name.EndsWith("Id") || prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?))
                    continue;

                while (true)
                {
                    Console.Write($"Enter {prop.Name}: ");
                    var input = Console.ReadLine();

                    try
                    {
                        if (string.IsNullOrWhiteSpace(input) && Nullable.GetUnderlyingType(prop.PropertyType) != null)
                        {
                            prop.SetValue(instance, null);
                        }
                        else
                        {
                            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                            object convertedValue = targetType switch
                            {
                                Type t when t == typeof(int) => int.Parse(input),
                                Type t when t == typeof(decimal) => decimal.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                                Type t when t == typeof(double) => double.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                                Type t when t == typeof(float) => float.Parse(input, System.Globalization.CultureInfo.InvariantCulture),
                                Type t when t == typeof(bool) => input.Trim().ToLower() == "true" || input.Trim() == "1",
                                _ => Convert.ChangeType(input, targetType)
                            };

                            prop.SetValue(instance, convertedValue);
                        }
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
                _dal.Create(instance);
                Console.WriteLine($"{typeof(T).Name} inserted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting {typeof(T).Name}: {ex.Message}");
            }
        }
    }
}
