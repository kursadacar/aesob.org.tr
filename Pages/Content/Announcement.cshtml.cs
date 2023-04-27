using System.Linq;
using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;

namespace aesob.org.tr.Pages.Content
{
    public class AnnouncementModel : AesobModelBase
    {

        public Duyurular InspectedAnnouncement { get; private set; }

        public AnnouncementModel(AesobDbContext context) : base(context)
        {

        }

        public IActionResult OnGet(int announcementID, string shortDescription)
        {
            InspectedAnnouncement = _context.Duyurulars.FirstOrDefault(x => x.Id == announcementID);
            if(InspectedAnnouncement == null)
            {
                Redirect(NavigationHelper.GetNotFoundPageURL());
            }
            return Page();
        }
    }
}
