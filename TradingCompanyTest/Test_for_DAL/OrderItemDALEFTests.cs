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
    public class OrderItemDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private OrderItemDALEF _dal;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<OrderItemMap>();
            configExpression.AddProfile<ProductMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new OrderItemDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllOrderItems_ReturnsItems()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        [Test]
        public void GetOrderItemById_ReturnsItem()
        {
            var all = _dal.GetAll();
            //if (!all.Any())
            //    Assert.Ignore("No order items to test GetById.");

            var item = _dal.GetById(all[0].OrderItemId);
            Assert.IsNotNull(item);
            Assert.AreEqual(all[0].OrderItemId, item.OrderItemId);
        }

        [Test]
        public void InsertOrderItem_WorksCorrectly()
        {
            var item = new OrderItem
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 5
            };

            var created = _dal.Create(item);
            Assert.IsNotNull(created);
            Assert.AreEqual(5, created.Quantity);

            _dal.Delete(created.OrderItemId);
        }

        [Test]
        public void UpdateOrderItem_WorksCorrectly()
        {
            var item = new OrderItem
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 10
            };

            var created = _dal.Create(item);
            Assert.IsNotNull(created);

            created.Quantity = 20;
            var updated = _dal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual(20, updated.Quantity);

            _dal.Delete(updated.OrderItemId);
        }

        [Test]
        public void DeleteOrderItem_WorksCorrectly()
        {
            var item = new OrderItem
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 15
            };

            var created = _dal.Create(item);
            Assert.IsNotNull(created);

            var deleted = _dal.Delete(created.OrderItemId);
            Assert.IsTrue(deleted);

            var fromDb = _dal.GetById(created.OrderItemId);
            Assert.IsNull(fromDb);
        }
    }
}

