using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Pages.Content
{
    public class DealsModel : AesobModelBase
    {
        public List<Anlasma> Deals { get; private set; }

        public DealsModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet()
        {
            Deals = _context.Anlasmas.OrderByDescending((Anlasma x) => x.Eklemetarihi).ToList();
            return Page();
        }
    }
}
