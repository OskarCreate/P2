using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using P2.Models;

namespace P2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
     private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

     [TempData]
    public string Message { get; set; }

    public IActionResult Index()
    {
        var cookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict
        };

        Response.Cookies.Append("MiCookie", "ValorDeLaCookie", cookieOptions);

        return View();
    }

    public async Task<IActionResult> Privacy()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var phoneNumber = user.PhoneNumber ?? "No phone number provided";
                var email = user.Email;
                var userName = user.UserName;

                Message = $"Customer {userName} (Email: {email}, Phone: {phoneNumber}) added.";
            }
            else
            {
                Message = "User data could not be retrieved.";
            }
        }
        else
        {
            Message = "No customer is logged in.";
        }

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
