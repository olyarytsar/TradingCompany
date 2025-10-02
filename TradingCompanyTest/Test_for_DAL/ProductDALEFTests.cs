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
            configExpression.AddProfile<ProductMap>(); // правильний мапінг для Product
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new ProductDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllProducts_ReturnsProducts()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
        }

        [Test]
        public void GetProductById_ReturnsProduct()
        {
            var all = _dal.GetAll();
            if (!all.Any()) Assert.Ignore("No products to test GetById");

            var prod = _dal.GetById(all[0].ProductId);
            Assert.IsNotNull(prod);
            Assert.AreEqual(all[0].ProductId, prod.ProductId);
        }

        [Test]
        public void InsertUpdateDeleteProduct_WorksCorrectly()
        {
            var prod = new Product
            {
                Name = "TestProd",
                CategoryId = 1,
                SupplierId = 1,
                Price = 100,
                QuantityInStock = 10
            };

            var created = _dal.Create(prod);
            Assert.IsNotNull(created);

            created.Price = 200;
            var updated = _dal.Update(created);
            Assert.AreEqual(200, updated.Price);

            var deleted = _dal.Delete(updated.ProductId);
            Assert.IsTrue(deleted);
            Assert.IsNull(_dal.GetById(updated.ProductId));
        }
    }
}
