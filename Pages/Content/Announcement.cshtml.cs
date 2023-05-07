using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public class AnnouncementModel : AesobModelBase
    {
        public Duyurular InspectedAnnouncement { get; private set; }

        public AnnouncementModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int announcementID, string shortDescription)
        {
            InspectedAnnouncement = _context.Duyurulars.FirstOrDefault((Duyurular x) => x.Id == announcementID);
            if (InspectedAnnouncement == null)
            {
                Redirect(NavigationHelper.GetNotFoundPageURL());
            }
            return Page();
        }
    }
}
