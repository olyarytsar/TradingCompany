﻿using AutoMapper;
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
        private OrderDALEF _dal;

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
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new OrderDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllOrders_ReturnsOrders()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
        }

        [Test]
        public void GetOrderById_ReturnsOrder()
        {
            var all = _dal.GetAll();
            if (!all.Any()) Assert.Ignore("No orders to test GetById");

            var order = _dal.GetById(all[0].OrderId);
            Assert.IsNotNull(order);
            Assert.AreEqual(all[0].OrderId, order.OrderId);
        }

        [Test]
        public void InsertUpdateDeleteOrder_WorksCorrectly()
        {
            var order = new Order
            {
                EmployeeId = 1,
                OrderDate = DateTime.Now,
                TotalAmount = 100,
                IsActive = true
            };

            var created = _dal.Create(order);
            Assert.IsNotNull(created);

            created.TotalAmount = 200;
            var updated = _dal.Update(created);
            Assert.AreEqual(200, updated.TotalAmount);

            var deleted = _dal.Delete(updated.OrderId);
            Assert.IsTrue(deleted);
            Assert.IsNull(_dal.GetById(updated.OrderId));
        }
    }
}
