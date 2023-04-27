using aesob.org.tr.Models;
using System.Collections.Generic;

namespace aesob.org.tr.Utilities
{
    public static class NavigationHelper
    {
        public static string ContentLayoutPath => "~/Pages/Content/_ContentLayout.cshtml";

        public static string GetNewsLink(Haberler news)
        {
            return $"~/Content/News?haber={news.Id}&baslik={CompatibilityHelper.ConvertStringForSEO(news.Baslik.ToLower())}";
        }

        public static string GetAnnouncementLink(Duyurular duyuru)
        {
            return $"~/Content/Announcement?announcementID={duyuru.Id}&shortDescription={CompatibilityHelper.ConvertStringForSEO(duyuru.Baslik?.ToLower())}";
        }

        public static string GetBoardLink(int kurulID)
        {
            return $"~/Content/Board?boardID={kurulID}";
        }

        public static string GetUnionPageLink(bool isCentral, int union)
        {
            return $"~/Content/Unions?type={(isCentral ? 1 : 0)}&union={union}";
        }

        public static string GetNotFoundPageURL()
        {
            return @"/NotFound";
        }

        public struct NavBarCategory
        {
            public bool DoNotAutoList { get; }
            public int NavID { get; }
            public string Header { get; }
            public List<NavBarPage> Items { get; }

            public NavBarCategory(string header, int navID, List<NavBarPage> items, bool doNotAutoList = false)
            {
                Header = header;
                Items = items;
                NavID = navID;
                DoNotAutoList = doNotAutoList;
            }
        }

        public struct NavBarPage
        {
            public NavBarCategory Parent { get; }
            public int PageID { get; }
            public string Title { get; }
            public string URL { get; }
            public bool HiddenInLayout { get; }
            public bool IsQuickLink { get; set; }

            public static NavBarPage CreateForContent(NavBarCategory parent, int contentID, int pageID, string title, bool hiddenInLayout = false, bool isQuickLink = false)
            {
                string url = $"Content/Info?title={CompatibilityHelper.ConvertStringForSEO(title)}&contentID={contentID}";
                return new NavBarPage(parent, title, url, pageID, hiddenInLayout, isQuickLink);
            }

            public static NavBarPage CreateForUrl(NavBarCategory parent, string url, int pageID, string title, bool hiddenInLayout = false, bool isQuickLink = false)
            {
                if (string.IsNullOrEmpty(url))
                {
                    url = "";
                }

                while(url != null && url.Length > 0 && (url[0] == '~' || url[0] == '/' || url[0] == '\\'))
                {
                    url = url.Remove(0, 1);
                }

                return new NavBarPage(parent, title, url, pageID, hiddenInLayout, isQuickLink);
            }

            public static NavBarPage CreateForUrlRaw(NavBarCategory parent, string url, int pageID, string title, bool hiddenInLayout = false, bool isQuickLink = false)
            {
                return new NavBarPage(parent, title, url, pageID, hiddenInLayout, isQuickLink);
            }

            private NavBarPage(NavBarCategory parent, string title, string url, int pageID, bool hiddenInLayout, bool isQuickLink)
            {
                Parent = parent;
                Title = title;
                URL = url;
                PageID = pageID;
                HiddenInLayout = hiddenInLayout;
                IsQuickLink = isQuickLink;
            }
        }

        #region Special Categories
        private static NavBarCategory _adminCategory;
        private static NavBarCategory _loginCategory;

        public static NavBarCategory AdminCategory
        {
            get
            {
                if(_adminCategory.NavID != 100)
                {
                    var adminBarElements = new List<NavBarPage>();
                    var categoryIndex = 100;
                    _adminCategory = new NavBarCategory("Admin", categoryIndex, adminBarElements, true);
                    int pageIndex = 0;

                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/AesobTVEditor", pageIndex++, "Aesob TV Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/AnlasmaEditor", pageIndex++, "Anlaşma Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/DuyuruEditor", pageIndex++, "Duyuru Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/DergiEditor", pageIndex++, "Dergi Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/GenelgeEditor", pageIndex++, "Genelge Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/HaberEditor", pageIndex++, "Haber Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/IcerikEditor", pageIndex++, "Icerik Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/KurulEditor", pageIndex++, "Kurul Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/OdaEditor", pageIndex++, "Oda Editor"));
                    adminBarElements.Add(NavBarPage.CreateForUrlRaw(_adminCategory, "Login?handler=Logout", pageIndex++, "Çıkış", true));
                }
                return _adminCategory;
            }
        }

        public static NavBarCategory LoginCategory
        {
            get
            {
                if(_loginCategory.NavID != 101)
                {
                    var categoryIndex = 101;
                    var loginElements = new List<NavBarPage>();
                    _loginCategory = new NavBarCategory("Giriş", categoryIndex++, loginElements, true);
                    int pageIndex = 0;

                    loginElements.Add(NavBarPage.CreateForUrl(_loginCategory, "~/Login", pageIndex++, "Giriş yap"));
                }
                return _loginCategory;
            }
        }
        #endregion

        public static IEnumerable<NavBarCategory> NavBarElements
        {
            get
            {
                int categoryIndex = 0;

                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Başkan", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 1, pageIndex++, "Özgeçmişi"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 2, pageIndex++, "Çalışma Ofisi"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 3, pageIndex++, "Görevleri"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 4, pageIndex++, "Projeleri"));
                    yield return navBar;
                }


                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Kurumsal", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 5, pageIndex++, "Amacı"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 6, pageIndex++, "Tarihçe"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 7, pageIndex++, "Ahi Evran"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 8, pageIndex++, "Ahilik"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 9, pageIndex++, "Gedik Teşkilatı"));
                    yield return navBar;
                }


                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Organizasyon", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, GetUnionPageLink(true, 2), pageIndex++, "Merkez Meslek Odaları"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, GetUnionPageLink(false, 39), pageIndex++, "İlçe Meslek Odaları"));

                    //TODO_Kursad: Readd this
                    //foreach (var boardData in BoardData.All)
                    //{
                    //    var link = GetBoardLink(boardData.ID);
                    //    navBarElements.Add(NavBarPage.CreateForUrl(navBar, link, pageIndex++, boardData.Name));
                    //}
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 11, pageIndex++, "Yönetim Kurulu"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 12, pageIndex++, "Denetim Kurulu"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 13, pageIndex++, "Disiplin Kurulu"));


                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 10, pageIndex++, "AESOB Adres ve Tel."));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 14, pageIndex++, "Organizasyon Şeması"));
                    yield return navBar;
                }

                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Çalışma Alanları", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 28, pageIndex++, "Sicil Müdürlüğü İşlemleri"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 29, pageIndex++, "Hukuk ve Den. İşlemleri"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 30, pageIndex++, "Mesleki Eğitim"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 31, pageIndex++, "Kapasite Raporları"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 32, pageIndex++, "Finansman"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 33, pageIndex++, "Sosyal Güvenlik"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 34, pageIndex++, "Vergi"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 35, pageIndex++, "Ahilik Kültürü Haftası"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 36, pageIndex++, "İstihdam"));
                    yield return navBar;
                }

                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Mevzuat", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 23, pageIndex++, "Kanun ve Yönetmelikler"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/Circulars", pageIndex++, "Genelgeler"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 24, pageIndex++, "Yönerge ve Ana Sözleş."));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/Deals", pageIndex++, "Anlaşma ve Protokoller"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 25, pageIndex++, "İlgili Mevzuatlar"));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 15, pageIndex++, "İşyeri Açarken", isQuickLink: true));
                    navBarElements.Add(NavBarPage.CreateForContent(navBar, 16, pageIndex++, "Esnaf Olma Şartları", isQuickLink: true));
                    yield return navBar;
                }

                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("İletişim", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    //navBarElements.Add(NavBarPage.CreateForUrl(navBar, "", pageIndex++, "Bilgi Edinme"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/Contact", pageIndex++, "İletişim"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/Location", pageIndex++, "Biz Neredeyiz"));
                    yield return navBar;
                }

                {
                    var navBarElements = new List<NavBarPage>();
                    var navBar = new NavBarCategory("Yayınlarımız", categoryIndex++, navBarElements);
                    int pageIndex = 0;

                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/Magazines", pageIndex++, "BİRLİK Dergisi"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/AesobTV", pageIndex++, "AESOB TV"));
                    navBarElements.Add(NavBarPage.CreateForUrl(navBar, "~/Content/NewsFeed", pageIndex++, "Haberler"));
                    //navBarElements.Add(NavBarPage.CreateForUrl(navBar, "", pageIndex++, "Duyurular"));
                    yield return navBar;
                }

                yield return AdminCategory;
                yield return LoginCategory;
            }
        }
    }
}
