using System.Collections.Generic;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class DealsModel : AesobModelBase
    {
        public List<Anlasma> Deals { get; private set; }

        public DealsModel(AesobDbContext context) : base(context)
        {
        }

        public IActionResult OnGet()
        {
            Deals = _context.Anlasmas.ToListAsync().Result;
            return Page();
        }
    }
}
