using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Authentication;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace aesob.org.tr
{
    public abstract class AesobModelBase : PageModel
    {
        public readonly AesobDbContext _context;
        public NavigationHelper.NavBarCategory CurrentCategory { get; private set; }
        public NavigationHelper.NavBarPage CurrentPage { get; private set; }
        public bool HasLoggedIn { get; private set; }

        public AesobModelBase(AesobDbContext context)
        {
            _context = context;
        }

        public new virtual IActionResult Page()
        {
            int categoryID = -1;
            int pageID = -1;

            var requestUrl = HttpContext.Request.Path.Value + HttpContext.Request.QueryString;

            var elements = NavigationHelper.NavBarElements;

            foreach(var nav in elements)
            {
                foreach(var page in nav.Items)
                {
                    if (requestUrl == "/" + page.URL)
                    {
                        categoryID = nav.NavID;
                        pageID = page.PageID;
                    }
                }
            }

            if(categoryID == -1 && pageID == -1)
            {
                List<(NavigationHelper.NavBarCategory, NavigationHelper.NavBarPage)> found = new List<(NavigationHelper.NavBarCategory, NavigationHelper.NavBarPage)>();

                foreach (var nav in elements)
                {
                    foreach (var page in nav.Items)
                    {
                        if (requestUrl.Contains(page.URL.Split('?')[0]))
                        {
                            found.Add((nav, page));
                            categoryID = nav.NavID;
                            pageID = page.PageID;
                        }
                    }
                }

                if (found.Count > 1)
                {
                    int maxSimilarities = 0;
                    NavigationHelper.NavBarPage? mostSimilarPage = null;
                    NavigationHelper.NavBarCategory? mostSimilarCategory = null;

                    var splitUrlRes = requestUrl.Split('?').Last()?.Split('&').ToList();
                    if (splitUrlRes != null)
                    {
                        foreach(var foundItem in found)
                        {
                            var split = foundItem.Item2.URL.Split('?').Last()?.Split("&").ToList();

                            if (split != null)
                            {
                                int similiarities = 0;

                                for (int i = 0; i < split.Count; i++)
                                {
                                    if (splitUrlRes.Count > i && split[i] == splitUrlRes[i])
                                    {
                                        similiarities++;
                                    }
                                }

                                if (similiarities > maxSimilarities)
                                {
                                    maxSimilarities = similiarities;
                                    mostSimilarCategory = foundItem.Item1;
                                    mostSimilarPage = foundItem.Item2;
                                }
                            }
                        }
                    }

                    if(mostSimilarCategory.HasValue && mostSimilarPage.HasValue)
                    {
                        categoryID = mostSimilarCategory.Value.NavID;
                        pageID = mostSimilarPage.Value.PageID;
                    }
                }
            }

            ViewData["HasLoggedIn"] = User?.Identity?.IsAuthenticated == true;
#if DEBUG
            ViewData["IsDevelopment"] = true;
#else
            ViewData["IsDevelopment"] = false;
#endif

            if (User?.Identity?.IsAuthenticated != true)
            {
                HttpContext.SignOutAsync().Wait();
            }

            if (categoryID >= 0)
            {
                try
                {
                    CurrentCategory = NavigationHelper.NavBarElements.ElementAt(categoryID);
                }
                catch
                {
                    if(categoryID == NavigationHelper.AdminCategory.NavID)
                    {
                        CurrentCategory = NavigationHelper.AdminCategory;
                    }
                    else if(categoryID == NavigationHelper.LoginCategory.NavID)
                    {
                        CurrentCategory = NavigationHelper.LoginCategory;
                    }
                    else
                    {
                        HttpContext.Response.Cookies.Delete("nav");
                        HttpContext.Response.Cookies.Delete("page");
                        CurrentCategory = NavigationHelper.NavBarElements.FirstOrDefault();
                    }
                }

                if (pageID >= 0)
                {
                    CurrentPage = CurrentCategory.Items.ElementAt(pageID);
                }
                else
                {
                    CurrentPage = default;
                }
            }
            else
            {
                CurrentCategory = default;
                CurrentPage = default;
            }

            ViewData["Title"] = CurrentPage.Title;
            ViewData["CurrentCategory"] = CurrentCategory;
            ViewData["CurrentPage"] = CurrentPage;

            return base.Page();
        }



    }
}
