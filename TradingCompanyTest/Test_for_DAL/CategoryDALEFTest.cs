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
    public class CategoryDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private CategoryDALEF _dal;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<CategoryMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new CategoryDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllCategories_ReturnsCategories()
        {
            var categories = _dal.GetAll();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Count >= 0);
        }

        [Test]
        public void GetCategoryById_ReturnsCategory()
        {
            var all = _dal.GetAll();
            if (all.Count == 0) Assert.Ignore("No categories in DB to test GetById.");

            var cat = _dal.GetById(all[0].CategoryId);
            Assert.IsNotNull(cat);
            Assert.AreEqual(all[0].CategoryId, cat.CategoryId);
        }

        [Test]
        public void InsertUpdateDeleteCategory_WorksCorrectly()
        {
            
            var category = new Category { CategoryName = "TestCategory" };
            var created = _dal.Create(category);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestCategory", created.CategoryName);

            
            created.CategoryName = "UpdatedCategory";
            var updated = _dal.Update(created);
            Assert.AreEqual("UpdatedCategory", updated.CategoryName);

            
            var deleted = _dal.Delete(updated.CategoryId);
            Assert.IsTrue(deleted);
            var fromDb = _dal.GetById(updated.CategoryId);
            Assert.IsNull(fromDb);
        }
    }
}

