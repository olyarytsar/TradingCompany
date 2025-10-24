using System;
using AutoMapper;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.ConsoleApp.AppMenu
{

    internal class AppMenuService
    {
        private readonly string _connectionString;
        private readonly IMapper _mapper;

        public AppMenuService(string connectionString, IMapper mapper)
        {
            _connectionString = connectionString;
            _mapper = mapper;
        }

        public void Show()
        {
            Console.WriteLine("Welcome to TradingCompany!\n");

            char choice = ' ';
            while (choice != 'q' && choice != 'Q')
            {
                Console.WriteLine("Select a menu:");
                Console.WriteLine("1 - Category Menu");
                Console.WriteLine("2 - Employee Menu");
                Console.WriteLine("3 - Role Menu");
                Console.WriteLine("4 - Product Menu");
                Console.WriteLine("5 - Order Menu");
                Console.WriteLine("6 - Order Item Menu");
                Console.WriteLine("7 - Supplier Menu");
                Console.WriteLine("q - Quit\n");

                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                choice = input[0];

                switch (choice)
                {
                    case '1':
                        new Menu<Category>(new CategoryDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '2':
                        new Menu<Employee>(new EmployeeDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '3':
                        new Menu<Role>(new RoleDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '4':
                        new Menu<Product>(new ProductDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '5':
                        new Menu<Order>(new OrderDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '6':
                        new Menu<OrderItem>(new OrderItemDALEF(_connectionString, _mapper)).Show();
                        break;
                    case '7':
                        new Menu<Supplier>(new SupplierDALEF(_connectionString, _mapper)).Show();
                        break;
                    case 'q':
                    case 'Q':
                        Console.WriteLine("Exiting");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }
    }
}
