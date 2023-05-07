using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages
{
    public class LoginModel : AesobModelBase
    {
        private class LoginResult : IActionResult
        {
            private string _username;

            private string _password;

            private AesobDbContext _dbContext;

            private HttpContext _httpContext;

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
                User user = _dbContext.Users.FirstOrDefault((User x) => x.Username == _username && x.Password == hashedPassword);
                if (user != null)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", Convert.ToString(user.ID)),
                        new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", user.Username)
                    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Cookies");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    _httpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true,
                        IssuedUtc = DateTimeOffset.UtcNow,
                        ExpiresUtc = DateTimeOffset.Now.AddDays(1.0)
                    }).Wait();
                    ObjectResult result2 = new ObjectResult("success");
                    return result2.ExecuteResultAsync(context);
                }
                ObjectResult result = new ObjectResult("error");
                return result.ExecuteResultAsync(context);
            }
        }

        public LoginModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnPostLogin(IFormCollection data)
        {
            return new LoginResult(data["username"], data["password"], _context, base.HttpContext);
        }

        public IActionResult OnGetLogout()
        {
            base.HttpContext.SignOutAsync("Cookies").Wait();
            return RedirectToPage("/Index");
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
