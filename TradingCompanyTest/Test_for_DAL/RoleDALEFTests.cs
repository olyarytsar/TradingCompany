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
    public class RoleDALEFTests
    {
        private string _testConnectionString;
        private IMapper _mapper;
        private RoleDALEF _dal;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testConnectionString = config.GetConnectionString("TestConnection");

            var configExpression = new MapperConfigurationExpression();
            configExpression.AddProfile<RoleMap>();
            var mapperConfig = new MapperConfiguration(configExpression, NullLoggerFactory.Instance);
            _mapper = mapperConfig.CreateMapper();

            _dal = new RoleDALEF(_testConnectionString, _mapper);
        }

        [Test]
        public void GetAllRoles_ReturnsRoles()
        {
            var list = _dal.GetAll();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        [Test]
        public void GetRoleById_ReturnsRole()
        {
            var all = _dal.GetAll();
            if (!all.Any())
                Assert.Ignore("No roles to test GetById.");

            var role = _dal.GetById(all[0].RoleId);
            Assert.IsNotNull(role);
            Assert.AreEqual(all[0].RoleId, role.RoleId);
        }

        [Test]
        public void InsertRole_WorksCorrectly()
        {
            var role = new Role { RoleName = "TestRole_Insert" };
            var created = _dal.Create(role);
            Assert.IsNotNull(created);
            Assert.AreEqual("TestRole_Insert", created.RoleName);

            _dal.Delete(created.RoleId);
        }

        [Test]
        public void UpdateRole_WorksCorrectly()
        {
            var role = new Role { RoleName = "TestRole_Update" };
            var created = _dal.Create(role);
            Assert.IsNotNull(created);

            created.RoleName = "UpdatedRoleName";
            var updated = _dal.Update(created);
            Assert.IsNotNull(updated);
            Assert.AreEqual("UpdatedRoleName", updated.RoleName);

            _dal.Delete(updated.RoleId);
        }

        [Test]
        public void DeleteRole_WorksCorrectly()
        {
            var role = new Role { RoleName = "TestRole_Delete" };
            var created = _dal.Create(role);
            Assert.IsNotNull(created);

            var deleted = _dal.Delete(created.RoleId);
            Assert.IsTrue(deleted);

            var fromDb = _dal.GetById(created.RoleId);
            Assert.IsNull(fromDb);
        }
    }
}

