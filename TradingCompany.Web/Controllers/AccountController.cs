using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradingCompany.BLL.Interfaces;
using TradingCompany.DALEF.Concrete;
using TradingCompany.DTO;
using TradingCompany.MVC.Models;

namespace TradingCompany.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthManager _manager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager manager, ILogger<AccountController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public IActionResult Login(string? ReturnUrl)
        {
            ViewData["ReturnUrl"] = ReturnUrl;
            var model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? ReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_manager.Login(model.Username, model.Password))
                    {
                        var employee = _manager.GetEmployeeByLogin(model.Username);
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.NameIdentifier, employee.EmployeeId.ToString()),
                        };

                        if (employee.Role != null)
                        {
                            var roleName = ((RoleType)employee.Role.RoleId).ToString();
                            claims.Add(new Claim(ClaimTypes.Role, roleName));
                        }

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                      
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true 
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        _logger.LogInformation("User {Username} logged in successfully.", model.Username);

                        return !string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)
                            ? Redirect(ReturnUrl)
                            : RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogWarning("Unauthorized login attempt for user: {Username}", model.Username);
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login for user: {Username}", model.Username);
                ModelState.AddModelError(string.Empty, $"An exception has occurred: {ex.Message}");
                return View(model);
            }
        }

        public IActionResult Forbidden()
        {
            _logger.LogWarning("Access denied for user {User} at {Time}", User.Identity?.Name ?? "Anonymous", DateTime.Now);
            return View();
        }
    }
}