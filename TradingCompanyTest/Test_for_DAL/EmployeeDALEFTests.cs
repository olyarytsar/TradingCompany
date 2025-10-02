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
    public class EmployeeDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private EmployeeDALEF _dal;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<EmployeeMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new EmployeeDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllEmployees_ReturnsEmployees()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
        }

        [Test]
        public void GetEmployeeById_ReturnsEmployee()
        {
            var all = _dal.GetAll();
            if (!all.Any()) Assert.Ignore("No employees to test GetById");

            var emp = _dal.GetById(all[0].EmployeeId);
            Assert.IsNotNull(emp);
            Assert.AreEqual(all[0].EmployeeId, emp.EmployeeId);
        }

        [Test]
        public void InsertUpdateDeleteEmployee_WorksCorrectly()
        {
            var emp = new Employee { FirstName = "Test", Login = "TestLogin", Password = "123", Phone = "1234567890", RoleId = 1 };
            var created = _dal.Create(emp);
            Assert.IsNotNull(created);

            created.FirstName = "UpdatedTest";
            var updated = _dal.Update(created);
            Assert.AreEqual("UpdatedTest", updated.FirstName);

            var deleted = _dal.Delete(updated.EmployeeId);
            Assert.IsTrue(deleted);
            Assert.IsNull(_dal.GetById(updated.EmployeeId));
        }
    }
}
