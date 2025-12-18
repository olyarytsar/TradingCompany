using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _mockManager = new Mock<IAuthManager>();
            _mockAuthService = new Mock<IAuthenticationService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            var services = new ServiceCollection();
            services.AddSingleton(_mockAuthService.Object);

            _controller = new AccountController(_mockManager.Object, _mockLogger.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        RequestServices = services.BuildServiceProvider()
                    }
                }
            };
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

            var error = _controller.ModelState[string.Empty].Errors[0].ErrorMessage;
            Assert.AreEqual("Invalid login attempt.", error);

            _mockAuthService.Verify(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Never);
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

            _mockAuthService.Setup(x => x.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Login(model, null) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);

            _mockAuthService.Verify(x => x.SignInAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme, It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()), Times.Once);
        }
        [Test]
        public async Task Logout_Post_SignOutAndRedirects()
        {
            // Arrange
            _mockAuthService.Setup(x => x.SignOutAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
            _mockAuthService.Verify(x => x.SignOutAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme, It.IsAny<AuthenticationProperties>()), Times.Once);
        }

    }
}