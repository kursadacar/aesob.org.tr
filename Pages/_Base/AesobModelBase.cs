using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
            string requestUrl = HttpContext.Request.Path.Value + HttpContext.Request.QueryString.ToString();
            IEnumerable<NavigationHelper.NavBarCategory> elements = NavigationHelper.NavBarElements;
            foreach (NavigationHelper.NavBarCategory nav in elements)
            {
                foreach (NavigationHelper.NavBarPage page2 in nav.Items)
                {
                    if (requestUrl == "/" + page2.URL)
                    {
                        categoryID = nav.NavID;
                        pageID = page2.PageID;
                    }
                }
            }
            if (categoryID == -1 && pageID == -1)
            {
                List<ValueTuple<NavigationHelper.NavBarCategory, NavigationHelper.NavBarPage>> found = new List<ValueTuple<NavigationHelper.NavBarCategory, NavigationHelper.NavBarPage>>();
                foreach (NavigationHelper.NavBarCategory nav2 in elements)
                {
                    foreach (NavigationHelper.NavBarPage page in nav2.Items)
                    {
                        if (requestUrl.Contains(page.URL.Split('?')[0]))
                        {
                            found.Add(new ValueTuple<NavigationHelper.NavBarCategory, NavigationHelper.NavBarPage>(nav2, page));
                            categoryID = nav2.NavID;
                            pageID = page.PageID;
                        }
                    }
                }
                if (found.Count > 1)
                {
                    int maxSimilarities = 0;
                    NavigationHelper.NavBarPage? mostSimilarPage = null;
                    NavigationHelper.NavBarCategory? mostSimilarCategory = null;
                    string text = requestUrl.Split('?').Last();
                    List<string> splitUrlRes = text != null ? text.Split('&').ToList() : null;
                    if (splitUrlRes != null)
                    {
                        foreach (var foundItem in found)
                        {
                            string text2 = foundItem.Item2.URL.Split('?').Last();
                            List<string> split = text2 != null ? text2.Split("&").ToList() : null;
                            if (split == null)
                            {
                                continue;
                            }
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
                    if (mostSimilarCategory.HasValue && mostSimilarPage.HasValue)
                    {
                        categoryID = mostSimilarCategory.Value.NavID;
                        pageID = mostSimilarPage.Value.PageID;
                    }
                }
            }

            bool isLoggedIn = User?.Identity?.IsAuthenticated ?? false;
            ViewData["HasLoggedIn"] = isLoggedIn;
            ViewData["UserPermission"] = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "none";
            ViewData["IsDevelopment"] = false;
#if DEBUG
            ViewData["IsDevelopment"] = true;
#endif

            if (categoryID >= 0)
            {
                try
                {
                    CurrentCategory = NavigationHelper.NavBarElements.ElementAt(categoryID);
                }
                catch
                {
                    if (categoryID == NavigationHelper.AdminCategory.NavID)
                    {
                        CurrentCategory = NavigationHelper.AdminCategory;
                    }
                    else if (categoryID == NavigationHelper.LoginCategory.NavID)
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

        public IActionResult OnGetLatestPopup()
        {
            Popup latestFoundPopup = _context.Popups.ToList().LastOrDefault((x) => x.IsActive);
            if (latestFoundPopup == null)
            {
                return new EmptyResult();
            }
            return new ObjectResult(JsonSerializer.Serialize(latestFoundPopup));
        }

        public IActionResult OnGetFileExists(string directory)
        {
            IWebHostEnvironment webHostEnv = (IWebHostEnvironment)HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            bool result = false;
            string actualPath = webHostEnv.WebRootPath + directory;
            if (System.IO.File.Exists(actualPath))
            {
                result = true;
            }
            return new ObjectResult(result);
        }
    }
}
