using System;
using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Pages.Content.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class MagazinesModel : AesobModelBase, INavigablePage
    {
        public List<Dergi> Magazines { get; private set; }

        public int MaxPages { get; private set; }

        public int CurrentPageIndex { get; private set; }

        public int MaxPerPage => 16;

        public string ContentPageName => "Content/Magazines";

        public string ContentParameterName => "pageIndex";

        public MagazinesModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int pageIndex)
        {
            List<Dergi> collection = _context.Dergis.OrderByDescending((Dergi x) => x.Dergisayisi).ToListAsync().Result;
            MaxPages = (int)MathF.Ceiling((float)collection.Count / (float)MaxPerPage);
            CurrentPageIndex = Math.Clamp(pageIndex, 0, MaxPages - 1);
            Magazines = new List<Dergi>();
            for (int i = CurrentPageIndex * MaxPerPage; i < Math.Min((CurrentPageIndex + 1) * MaxPerPage, collection.Count); i++)
            {
                Magazines.Add(collection[i]);
            }
            return Page();
        }
    }
}
