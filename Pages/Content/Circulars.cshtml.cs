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
        public List<Genelgeler> Circulars { get; private set; }
        public List<int> AvailableYears { get; private set; }

        public int CurYear { get; private set; }

        public CircularsModel(AesobDbContext context) : base(context)
        {
        }

        public IActionResult OnGet(int year)
        {
            int minYear = _context.Genelgelers.Min(x => x.Tarih).Value.Year;
            int maxYear = _context.Genelgelers.Max(x => x.Tarih).Value.Year;

            if (year == 0 || year > maxYear || year < minYear)
            {
                year = maxYear;
            }

            CurYear = year;

            Circulars = _context.Genelgelers.Where(x => x.Tarih.Value.Year == CurYear).ToListAsync().Result;
            Circulars.Sort(new CircularComparer());

            AvailableYears = new List<int>();
            for(int i = minYear; i <= maxYear; i++)
            {
                if(_context.Genelgelers.Any(x => x.Tarih.Value.Year == i))
                {
                    AvailableYears.Add(i);
                }
            }

            return Page();
        }

        class CircularComparer : IComparer<Genelgeler>
        {
            public int Compare([AllowNull] Genelgeler x, [AllowNull] Genelgeler y)
            {
                return -DateTime.Compare(x?.Eklemetarihi ?? DateTime.MinValue, y?.Eklemetarihi ?? DateTime.MinValue);
            }
        }
    }
}
