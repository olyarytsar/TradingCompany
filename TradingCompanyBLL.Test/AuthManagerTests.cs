using Moq;
using NUnit.Framework;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
using TradingCompany.DALEF.Concrete; 
using TradingCompany.DTO;

namespace TradingCompany.BLL.Tests
{
    [TestFixture]
    public class AuthManagerTests
    {
        private Mock<IEmployeeDAL> _employeeDalMock;
        private AuthManager _sut;

        [SetUp]
        public void SetUp()
        {
            _employeeDalMock = new Mock<IEmployeeDAL>(MockBehavior.Strict);
            _sut = new AuthManager(_employeeDalMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _employeeDalMock.VerifyAll();
        }


        [Test]
        public void Login_ShouldReturnTrue_WhenCredentialsAreValid()
        {
            // Arrange
            string login = "admin";
            string password = "123";

            _employeeDalMock.Setup(d => d.Login(login, password)).Returns(true);

            // Act
            bool result = _sut.Login(login, password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Login_ShouldReturnFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            string login = "user";
            string password = "wrong";

            _employeeDalMock.Setup(d => d.Login(login, password)).Returns(false);

            // Act
            bool result = _sut.Login(login, password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasRole_ShouldReturnTrue_WhenRoleMatches()
        {
            // Arrange
            var employee = new Employee
            {
                Role = new Role { RoleName = "Manager" }
            };

            // Act
            var result = _sut.HasRole(employee, RoleType.Manager);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void HasRole_ShouldReturnFalse_WhenRoleIsDifferent()
        {
            // Arrange
            var employee = new Employee
            {
                Role = new Role { RoleName = "Admin" }
            };

            // Act
            var result = _sut.HasRole(employee, RoleType.Manager);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void HasRole_ShouldReturnFalse_WhenEmployeeOrRoleIsNull()
        {
            // Act
            var result1 = _sut.HasRole(null, RoleType.Manager);
            var result2 = _sut.HasRole(new Employee { Role = null }, RoleType.Manager);

            // Assert
            Assert.IsFalse(result1);
            Assert.IsFalse(result2);
        }
    }
}