using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class NewsModel : AesobModelBase
    {
        public Haberler Haber { get; private set; }
        public IEnumerable<Haberler> SonHaberler { get; private set; }

        public NewsModel(AesobDbContext context) : base(context)
        {

        }

        public IActionResult OnGet(int haber, string baslik)
        {
            SonHaberler = _context.Haberlers.ToListAsync().Result.TakeLast(8);
            Haber = _context.Haberlers.Where(x => x.Id == haber).ToListAsync().Result.FirstOrDefault();
            if(Haber == null)
            {
                return Redirect(NavigationHelper.GetNotFoundPageURL());
            }
            return Page();
        }

        //public void OnGet()
        //{
        //}
    }
}
