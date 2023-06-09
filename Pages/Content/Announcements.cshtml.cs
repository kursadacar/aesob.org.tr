using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class AnnouncementsModel : AesobModelBase
    {
        public List<Duyurular> Announcements { get; private set; }

        public AnnouncementsModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet()
        {
            Announcements = _context.Duyurulars.ToListAsync().Result;
            Announcements = Announcements.OrderByDescending((Duyurular x) => x.Eklemetarihi).ToList();
            return Page();
        }
    }
}
