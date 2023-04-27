using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace aesob.org.tr.Pages
{
    public class LoginModel : AesobModelBase
    {
        public LoginModel(AesobDbContext context) : base(context)
        {
        }

        class LoginResult : IActionResult
        {
            string _username;
            string _password;
            AesobDbContext _dbContext;
            HttpContext _httpContext;

            public LoginResult(string username, string password, AesobDbContext dbContext, HttpContext httpContext)
            {
                _username = username;
                _password = password;
                _dbContext = dbContext;
                _httpContext = httpContext;
            }

            public Task ExecuteResultAsync(ActionContext context)
            {
                string hashedPassword = EncryptionHelper.Encrypt(_password);

                var user = _dbContext.Users.FirstOrDefault(x => x.Username == _username && x.Password == hashedPassword);

                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.ID)),
                        new Claim(ClaimTypes.Name, user.Username),
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    _httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        IssuedUtc = DateTimeOffset.UtcNow,
                        ExpiresUtc = (DateTimeOffset.Now.AddDays(1))
                    }).Wait();

                    var result = new ObjectResult("success");
                    return result.ExecuteResultAsync(context);
                }
                else
                {
                    var result = new ObjectResult("error");
                    return result.ExecuteResultAsync(context);
                }
            }
        }

        public IActionResult OnPostLogin(IFormCollection data)
        {
            //_signInManager.IsSignedIn()

            return new LoginResult(data["username"], data["password"], _context, HttpContext);
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToPage("/Index");
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
