using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
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
            if (all.Count == 0)
                Assert.Ignore("No categories in DB to test GetById.");

            var cat = _dal.GetById(all[0].CategoryId);
            Assert.IsNotNull(cat);
            Assert.AreEqual(all[0].CategoryId, cat.CategoryId);
        }

        [Test]
        public void InsertCategory_WorksCorrectly()
        {
            var category = new Category { CategoryName = "TestCategory_Insert" };
            var created = _dal.Create(category);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestCategory_Insert", created.CategoryName);
            _dal.Delete(created.CategoryId);
        }

        [Test]
        public void UpdateCategory_WorksCorrectly()
        {
            var category = new Category { CategoryName = "TestCategory_ForUpdate" };
            var created = _dal.Create(category);
            Assert.IsNotNull(created);
            created.CategoryName = "TestCategory_Updated";
            var updated = _dal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual("TestCategory_Updated", updated.CategoryName);
            _dal.Delete(updated.CategoryId);
        }

        [Test]
        public void DeleteCategory_WorksCorrectly()
        {
            var category = new Category { CategoryName = "TestCategory_ForDelete" };
            var created = _dal.Create(category);
            Assert.IsNotNull(created);
            var deleted = _dal.Delete(created.CategoryId);
            Assert.IsTrue(deleted);
            var fromDb = _dal.GetById(created.CategoryId);
            Assert.IsNull(fromDb);
        }
    }
}
