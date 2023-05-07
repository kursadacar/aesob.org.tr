using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class AesobTVModel : AesobModelBase
    {
        public List<Youtubevideo> Videos { get; private set; }

        public AesobTVModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet()
        {
            Videos = _context.Youtubevideos.OrderByDescending((Youtubevideo x) => x.Tarih.Value).ToListAsync().Result;
            return Page();
        }
    }
}
