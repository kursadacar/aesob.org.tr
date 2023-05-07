using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public class LocationModel : AesobModelBase
    {
        public LocationModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
