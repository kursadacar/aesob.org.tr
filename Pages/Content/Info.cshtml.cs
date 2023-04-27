using System.Linq;
using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;

namespace aesob.org.tr.Pages.Content
{
    public class InfoModel : AesobModelBase
    {
        public string PageContent { get; private set; }

        public InfoModel(AesobDbContext context) : base(context)
        {

        }

        public IActionResult OnGet(int nav, int page, int contentID)
        {
            PageContent = _context.Iceriklers.FirstOrDefault(x => x.Id == contentID).Icerik;

            return Page();
        }
    }
}
