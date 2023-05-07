using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class CircularsModel : AesobModelBase
    {
        private class CircularComparer : IComparer<Genelgeler>
        {
            public int Compare([AllowNull] Genelgeler x, [AllowNull] Genelgeler y)
            {
                return -DateTime.Compare(((x != null) ? x.Eklemetarihi : null) ?? DateTime.MinValue, ((y != null) ? y.Eklemetarihi : null) ?? DateTime.MinValue);
            }
        }

        public List<Genelgeler> Circulars { get; private set; }

        public List<int> AvailableYears { get; private set; }

        public int CurYear { get; private set; }

        public CircularsModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int year)
        {
            int minYear = _context.Genelgelers.Min((Genelgeler x) => x.Tarih).Value.Year;
            int maxYear = _context.Genelgelers.Max((Genelgeler x) => x.Tarih).Value.Year;
            if (year == 0 || year > maxYear || year < minYear)
            {
                year = maxYear;
            }
            CurYear = year;
            Circulars = _context.Genelgelers.Where((Genelgeler x) => x.Tarih.Value.Year == CurYear).ToListAsync().Result;
            Circulars.Sort(new CircularComparer());
            AvailableYears = new List<int>();
            int i = minYear;
            while (true)
            {
                if (i > maxYear)
                {
                    break;
                }
                if (_context.Genelgelers.Any((Genelgeler x) => x.Tarih.Value.Year == i))
                {
                    AvailableYears.Add(i);
                }
                i++;
            }
            return Page();
        }
    }
}
