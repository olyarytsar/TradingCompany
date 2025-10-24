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
    public class ProductDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private ProductDALEF _dal;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<ProductMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new ProductDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllProducts_ReturnsProducts()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        [Test]
        public void GetProductById_ReturnsProduct()
        {
            var all = _dal.GetAll();
            if (!all.Any())
                Assert.Ignore("No products to test GetById.");

            var prod = _dal.GetById(all[0].ProductId);
            Assert.IsNotNull(prod);
            Assert.AreEqual(all[0].ProductId, prod.ProductId);
        }

        [Test]
        public void InsertProduct_WorksCorrectly()
        {
            var prod = new Product
            {
                Name = "TestProduct_Insert",
                CategoryId = 1,
                SupplierId = 1,
                Price = 100,
                QuantityInStock = 10
            };

            var created = _dal.Create(prod);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestProduct_Insert", created.Name);

            _dal.Delete(created.ProductId);
        }

        [Test]
        public void UpdateProduct_WorksCorrectly()
        {
            var prod = new Product
            {
                Name = "TestProduct_Update",
                CategoryId = 1,
                SupplierId = 1,
                Price = 150,
                QuantityInStock = 5
            };

            var created = _dal.Create(prod);
            Assert.IsNotNull(created);

            created.Price = 250;
            var updated = _dal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual(250, updated.Price);

            _dal.Delete(updated.ProductId);
        }

        [Test]
        public void DeleteProduct_WorksCorrectly()
        {
            var prod = new Product
            {
                Name = "TestProduct_Delete",
                CategoryId = 1,
                SupplierId = 1,
                Price = 200,
                QuantityInStock = 8
            };

            var created = _dal.Create(prod);
            Assert.IsNotNull(created);

            var deleted = _dal.Delete(created.ProductId);
            Assert.IsTrue(deleted);

            var fromDb = _dal.GetById(created.ProductId);
            Assert.IsNull(fromDb);
        }
    }
}
