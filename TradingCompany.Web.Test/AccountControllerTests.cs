using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using System.Threading.Tasks;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;
using TradingCompany.MVC.Controllers;
using TradingCompany.MVC.Models;

namespace TradingCompany.MVC.Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAuthManager> _mockManager;
        private Mock<IAuthenticationService> _mockAuthService;
        private Mock<ILogger<AccountController>> _mockLogger;
        private Mock<IUrlHelper> _mockUrlHelper; 
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _mockManager = new Mock<IAuthManager>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _mockUrlHelper = new Mock<IUrlHelper>(); 

            var services = new ServiceCollection();

            services.AddSingleton(_mockAuthService.Object);

            services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
            var mockTempDataProvider = new Mock<ITempDataProvider>();
            services.AddSingleton<ITempDataProvider>(mockTempDataProvider.Object);

            services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();

            var serviceProvider = services.BuildServiceProvider();

            _controller = new AccountController(_mockManager.Object, _mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = serviceProvider
                    }
                }
            };

            _controller.Url = _mockUrlHelper.Object;

            _controller.TempData = new TempDataDictionary(
                _controller.HttpContext,
                serviceProvider.GetRequiredService<ITempDataProvider>()
            );
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public void Login_Get_ReturnsViewWithModel()
        {
            // Act
            var result = _controller.Login("/Home/Index") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<LoginModel>(result.Model);
            Assert.AreEqual("/Home/Index", _controller.ViewData["ReturnUrl"]);
        }

        [Test]
        public async Task Login_Post_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            var model = new LoginModel { Username = "ghost", Password = "123" };
            _mockManager.Setup(m => m.Login(model.Username, model.Password)).Returns(false);

            // Act
            var result = await _controller.Login(model, null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
            Assert.IsFalse(_controller.ModelState.IsValid);
            Assert.AreEqual("Invalid login attempt.", _controller.ModelState[string.Empty].Errors[0].ErrorMessage);
        }

        [Test]
        public async Task Login_Post_ValidCredentials_RedirectsToHome()
        {
            // Arrange
            var model = new LoginModel { Username = "manager", Password = "ok" };
            var employee = new Employee
            {
                EmployeeId = 1,
                Login = "manager",
                Role = new Role { RoleId = (int)RoleType.Manager, RoleName = "Manager" }
            };

            _mockManager.Setup(m => m.Login(model.Username, model.Password)).Returns(true);
            _mockManager.Setup(m => m.GetEmployeeByLogin(model.Username)).Returns(employee);
            _mockAuthService
                .Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            _mockUrlHelper.Setup(x => x.IsLocalUrl(It.IsAny<string>())).Returns(false);

            // Act
            var result = await _controller.Login(model, null) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public async Task Logout_Post_SignOutAndRedirects()
        {
            _mockAuthService
                .Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
    }
}