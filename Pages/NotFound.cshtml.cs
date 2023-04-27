using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Shared
{
    public class NotFoundModel : AesobModelBase
    {
        public NotFoundModel(AesobDbContext context) : base(context)
        {

        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
