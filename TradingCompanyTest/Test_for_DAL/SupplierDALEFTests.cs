using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
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
            Assert.IsTrue(list.Count >= 0);
        }

        [Test]
        public void GetSupplierById_ReturnsSupplier()
        {
            var all = _dal.GetAll();
            if (!all.Any())
                Assert.Ignore("No suppliers in database to test GetById.");

            var supplier = _dal.GetById(all[0].SupplierId);
            Assert.IsNotNull(supplier);
            Assert.AreEqual(all[0].SupplierId, supplier.SupplierId);
        }

        [Test]
        public void InsertSupplier_WorksCorrectly()
        {
            var supplier = new Supplier
            {
                Brand = "TestBrand_Insert",
                Phone = "1111111111",
                Email = "insert@test.com",
                Address = "Insert Street"
            };

            var created = _dal.Create(supplier);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestBrand_Insert", created.Brand);

            _dal.Delete(created.SupplierId);
        }

        [Test]
        public void UpdateSupplier_WorksCorrectly()
        {
            var supplier = new Supplier
            {
                Brand = "TestBrand_Update",
                Phone = "2222222222",
                Email = "update@test.com",
                Address = "Update Street"
            };

            var created = _dal.Create(supplier);
            Assert.IsNotNull(created);

            created.Brand = "UpdatedBrand";
            var updated = _dal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual("UpdatedBrand", updated.Brand);

            _dal.Delete(updated.SupplierId);
        }

        [Test]
        public void DeleteSupplier_WorksCorrectly()
        {
            var supplier = new Supplier
            {
                Brand = "TestBrand_Delete",
                Phone = "3333333333",
                Email = "delete@test.com",
                Address = "Delete Street"
            };

            var created = _dal.Create(supplier);
            Assert.IsNotNull(created);

            var deleted = _dal.Delete(created.SupplierId);
            Assert.IsTrue(deleted);

            var fromDb = _dal.GetById(created.SupplierId);
            Assert.IsNull(fromDb);
        }
    }
}

