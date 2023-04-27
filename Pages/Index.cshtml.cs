using aesob.org.tr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace aesob.org.tr.Pages
{
    public class IndexModel : AesobModelBase
    {
        public IEnumerable<Genelgeler> CircularsFeed { get; private set; }
        public IEnumerable<Duyurular> AnnouncementsFeed { get; private set; }
        public IEnumerable<Haberler> NewsFeed { get; private set; }
        public IEnumerable<Baskanlar> Presidents { get; private set; }
        public Baskanlar Baskan { get; private set; }

        [BindProperty]
        public string SubscriptionName { get; set; }
        [BindProperty]
        public string SubscriptionEmail { get; set; }

        public IndexModel(AesobDbContext context) : base(context)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            NewsFeed = _context.Haberlers.OrderByDescending(x => x.Id).Take(4).ToList();
            Presidents = _context.Baskanlars.ToListAsync().Result;
            AnnouncementsFeed = _context.Duyurulars.OrderByDescending(x => x.Eklemetarihi).Take(3).ToList();
            CircularsFeed = _context.Genelgelers.OrderByDescending(x => x.Eklemetarihi).ThenByDescending(x => x.Sayi).Take(4).ToList();

            Baskan = Presidents.FirstOrDefault(x => x.Aktif == true);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostSubscribeToEmail()
        {
            if(!string.IsNullOrEmpty(SubscriptionEmail) && !string.IsNullOrEmpty(SubscriptionName))
            {
                if(_context.Epostalistesis.Any(x => x.Eposta == SubscriptionEmail))
                {
                    return new ObjectResult("error-already-exists");
                }
                try
                {
                    _context.Epostalistesis.Add(new Epostalistesi() { Adsoyad = SubscriptionName, Eposta = SubscriptionEmail, Durum = true, Tarih = DateTime.Now });
                    _context.SaveChanges();
                    return new ObjectResult("success");
                }
                catch (Exception e)
                {
                    return new ObjectResult("error -> " + e.ToString());
                }
            }
            else
            {
                return new ObjectResult("error-data");
            }
        }
    }
}
