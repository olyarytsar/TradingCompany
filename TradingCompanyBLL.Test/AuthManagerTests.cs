using Moq;
using NUnit.Framework;
using TradingCompany.BLL.Concrete;
using TradingCompany.DAL.Interfaces;
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
        public void Login_ShouldReturnEmployee_WhenCredentialsAreValid()
        {
            // Arrange
            string login = "admin";
            string password = "123";
            var employee = new Employee { Login = login };

            _employeeDalMock.Setup(d => d.Login(login, password)).Returns(true);
            _employeeDalMock.Setup(d => d.GetByLogin(login)).Returns(employee);

            // Act
            var result = _sut.Login(login, password);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Login, Is.EqualTo(login));
        }

        [Test]
        public void Login_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            _employeeDalMock.Setup(d => d.Login("user", "wrong")).Returns(false);

            // Act
            var result = _sut.Login("user", "wrong");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IsWarehouseManager_ShouldReturnTrue_WhenRoleIsManager()
        {
            // Arrange
            var employee = new Employee
            {
                Role = new Role { RoleName = "Manager" }
            };

            // Act
            var result = _sut.IsWarehouseManager(employee);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsWarehouseManager_ShouldReturnFalse_WhenRoleIsDifferent()
        {
            // Arrange
            var employee = new Employee
            {
                Role = new Role { RoleName = "Admin" }
            };

            // Act
            var result = _sut.IsWarehouseManager(employee);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsWarehouseManager_ShouldReturnFalse_WhenEmployeeOrRoleIsNull()
        {
            // Act
            var result1 = _sut.IsWarehouseManager(null);
            var result2 = _sut.IsWarehouseManager(new Employee { Role = null });

            // Assert
            Assert.That(result1, Is.False);
            Assert.That(result2, Is.False);
        }
    }
}