using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
using System.Linq;
using TradingCompany.DALEF.AutoMapper;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;

namespace TradingCompany.Test.DALEF
{
    [TestFixture]
    public class OrderDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private OrderDALEF _orderDal;
        private EmployeeDALEF _employeeDal; // Додали це, щоб створювати працівника

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<OrderMap>();
            configExpression.AddProfile<EmployeeMap>(); // Додали мапінг для працівника
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _orderDal = new OrderDALEF(_testConnectionString, _mapper);
            _employeeDal = new EmployeeDALEF(_testConnectionString, _mapper);
        }

        // Допоміжний метод: створює працівника і повертає його ID
        private int CreateTempEmployee()
        {
            var emp = new Employee
            {
                FirstName = "TestUser",
                // Генеруємо унікальний логін, щоб не було помилок дублікатів
                Login = "User_" + Guid.NewGuid().ToString().Substring(0, 8),
                Password = "123",
                Phone = "123456789",
                RoleId = 1 // Ти сказала, що роль 1 точно є в базі
            };

            var created = _employeeDal.Create(emp);
            if (created == null) throw new Exception("Не вдалося створити працівника для тесту.");

            return created.EmployeeId;
        }

        [Test]
        public void GetAllOrders_ReturnsOrders()
        {
            var list = _orderDal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        [Test]
        public void GetOrderById_ReturnsOrder()
        {
            var all = _orderDal.GetAll();
            if (!all.Any())
            {
                // Якщо база пуста, створюємо дані для тесту
                int empId = CreateTempEmployee();
                var newOrder = _orderDal.Create(new Order
                {
                    EmployeeId = empId,
                    OrderDate = DateTime.Now,
                    TotalAmount = 50,
                    IsActive = true
                });

                var order = _orderDal.GetById(newOrder.OrderId);
                Assert.IsNotNull(order);
                Assert.AreEqual(newOrder.OrderId, order.OrderId);

                // Чистимо за собою
                _orderDal.Delete(newOrder.OrderId);
                _employeeDal.Delete(empId);
            }
            else
            {
                var order = _orderDal.GetById(all[0].OrderId);
                Assert.IsNotNull(order);
                Assert.AreEqual(all[0].OrderId, order.OrderId);
            }
        }

        [Test]
        public void InsertOrder_WorksCorrectly()
        {
            // 1. Створюємо працівника
            int empId = CreateTempEmployee();

            // 2. Створюємо замовлення з реальним ID
            var order = new Order
            {
                EmployeeId = empId,
                OrderDate = DateTime.Now,
                TotalAmount = 150,
                IsActive = true
            };

            var created = _orderDal.Create(order);
            Assert.IsNotNull(created);
            Assert.AreEqual(150, created.TotalAmount);

            // 3. Видаляємо (спочатку замовлення, потім працівника)
            _orderDal.Delete(created.OrderId);
            _employeeDal.Delete(empId);
        }

        [Test]
        public void UpdateOrder_WorksCorrectly()
        {
            int empId = CreateTempEmployee();

            var order = new Order
            {
                EmployeeId = empId,
                OrderDate = DateTime.Now,
                TotalAmount = 100,
                IsActive = true
            };

            var created = _orderDal.Create(order);
            Assert.IsNotNull(created);

            created.TotalAmount = 250;
            var updated = _orderDal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual(250, updated.TotalAmount);

            _orderDal.Delete(updated.OrderId);
            _employeeDal.Delete(empId);
        }

        [Test]
        public void DeleteOrder_WorksCorrectly()
        {
            int empId = CreateTempEmployee();

            var order = new Order
            {
                EmployeeId = empId,
                OrderDate = DateTime.Now,
                TotalAmount = 300,
                IsActive = true
            };

            var created = _orderDal.Create(order);
            Assert.IsNotNull(created);

            var deleted = _orderDal.Delete(created.OrderId);
            Assert.IsTrue(deleted);

            var fromDb = _orderDal.GetById(created.OrderId);
            Assert.IsNull(fromDb);

            _employeeDal.Delete(empId);
        }
    }
}