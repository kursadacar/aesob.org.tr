using System;
using System.Collections.Generic;
using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr
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

        public IndexModel(AesobDbContext context)
            : base(context)
        {
            InitializeData();
        }

        private void InitializeData()
        {
            NewsFeed = _context.Haberlers.OrderByDescending((Haberler x) => x.Id).Take(4).ToList();
            Presidents = _context.Baskanlars.ToListAsync().Result;
            AnnouncementsFeed = _context.Duyurulars.OrderByDescending((Duyurular x) => x.Eklemetarihi).Take(3).ToList();
            CircularsFeed = (from x in _context.Genelgelers
                             orderby x.Eklemetarihi descending, x.Sayi descending
                             select x).Take(4).ToList();
            Baskan = Presidents.FirstOrDefault((Baskanlar x) => x.Aktif);
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPostSubscribeToEmail()
        {
            if (!EmailService.IsValidEmail(SubscriptionEmail))
            {
                return ServiceActionResult.CreateFail("Lütfen geçerli bir e-mail giriniz.");
            }
            if (string.IsNullOrEmpty(SubscriptionName))
            {
                return ServiceActionResult.CreateFail("İsim bölümü boş olamaz");
            }
            if (_context.EBultens.Any((EBulten x) => x.EMail == SubscriptionEmail))
            {
                return ServiceActionResult.CreateFail("E-Mail zaten kayıtlı");
            }
            try
            {
                _context.EBultens.Add(new EBulten
                {
                    Name = SubscriptionName,
                    EMail = SubscriptionEmail,
                    IsEmailActive = true
                });
                _context.SaveChanges();
                return ServiceActionResult.CreateSuccess("Başarılı bir şekilde eklendi.");
            }
            catch (Exception e)
            {
                return ServiceActionResult.CreateFail("E-Mail kaydedilirken hata oluştu: " + e.Message);
            }
        }
    }
}
