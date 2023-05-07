using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public class BoardModel : AesobModelBase
    {
        public IEnumerable<KurulUyeleri> BoardMembers { get; private set; }

        public int BoardID { get; private set; }

        public BoardModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int boardID)
        {
            BoardMembers = from x in _context.KurulUyeleris
                           where x.Kurul == boardID
                           select x into b
                           orderby b.Derece
                           select b;
            BoardID = boardID;
            return Page();
        }
    }
}
