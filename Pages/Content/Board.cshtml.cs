using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public class BoardModel : AesobModelBase
    {
        public BoardModel(AesobDbContext context) : base(context)
        {

        }

        public IEnumerable<KurulUyeleri> BoardMembers { get; private set; }
        public int BoardID { get; private set; }

        public IActionResult OnGet(int boardID)
        {
            BoardMembers = _context.KurulUyeleris.Where(x => x.Kurul == boardID).OrderBy(b => b.Derece);
            BoardID = boardID;
            return Page();
        }
    }
}
