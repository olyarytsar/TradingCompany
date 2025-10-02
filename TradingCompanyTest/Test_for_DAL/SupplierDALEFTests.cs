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
    public class SupplierDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private SupplierDALEF _dal;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<SupplierMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new SupplierDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllSuppliers_ReturnsSuppliers()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
        }

        [Test]
        public void GetSupplierById_ReturnsSupplier()
        {
            var all = _dal.GetAll();
            if (!all.Any())
                Assert.Ignore("No suppliers to test GetById");

            var supplier = _dal.GetById(all[0].SupplierId);
            Assert.IsNotNull(supplier);
            Assert.AreEqual(all[0].SupplierId, supplier.SupplierId);
        }

        [Test]
        public void InsertUpdateDeleteSupplier_WorksCorrectly()
        {
            var supplier = new Supplier { Brand = "TestBrand", Phone = "1234567890", Email = "test@test.com", Address = "Test Address" };
            var created = _dal.Create(supplier);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestBrand", created.Brand);

            created.Brand = "UpdatedBrand";
            var updated = _dal.Update(created);
            Assert.AreEqual("UpdatedBrand", updated.Brand);

            var deleted = _dal.Delete(updated.SupplierId);
            Assert.IsTrue(deleted);
            Assert.IsNull(_dal.GetById(updated.SupplierId));
        }
    }
}
