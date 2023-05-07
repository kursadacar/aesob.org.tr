using System;
using System.Collections.Generic;
using aesob.org.tr.Models;
using aesob.org.tr.Pages.Content.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aesob.org.tr.Pages.Content
{
    public class NewsFeedModel : AesobModelBase, INavigablePage
    {
        public List<Haberler> News { get; private set; }

        public int MaxPages { get; private set; }

        public int CurrentPageIndex { get; private set; }

        public int MaxPerPage => 16;

        public string ContentPageName =>"Content/NewsFeed";//Synced with page name

        public string ContentParameterName => "pageIndex";//Synced with onget parameter name below

        public NewsFeedModel(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(int pageIndex = 0)
        {
            News = new List<Haberler>();
            List<Haberler> tmpNews = _context.Haberlers.ToListAsync().Result;
            tmpNews.Sort(new NewsComparer());
            tmpNews.Reverse();
            MaxPages = (int)Math.Floor((float)tmpNews.Count / (float)MaxPerPage);
            CurrentPageIndex = Math.Clamp(pageIndex, 0, MaxPages);
            for (int i = MaxPerPage * CurrentPageIndex; i < Math.Min(tmpNews.Count, MaxPerPage * (CurrentPageIndex + 1)); i++)
            {
                News.Add(tmpNews[i]);
            }
            return Page();
        }

        internal class NewsComparer : IComparer<Haberler>
        {
            public int Compare(Haberler x, Haberler y)
            {
                DateTime dateX;
                DateTime dateY;

                DateTime.TryParse(x.Tarih, out dateX);
                DateTime.TryParse(y.Tarih, out dateY);

                if (dateX.Year != dateY.Year)
                {
                    return dateX.Year - dateY.Year;
                }

                if (dateX.Month != dateY.Month)
                {
                    return dateX.Month - dateY.Month;
                }

                if (dateX.Day != dateY.Day)
                {
                    return dateX.Day - dateY.Day;
                }

                if (dateX.Hour != dateY.Hour)
                {
                    return dateX.Hour - dateY.Hour;
                }

                if (dateX.Minute != dateY.Minute)
                {
                    return dateX.Minute - dateY.Minute;
                }

                return dateX.Second - dateY.Second;
            }
        }
    }
}
