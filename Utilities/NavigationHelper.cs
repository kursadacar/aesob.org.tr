using System.Collections.Generic;
using aesob.org.tr.Models;

namespace aesob.org.tr.Utilities
{
	public static class NavigationHelper
	{
		public struct NavBarCategory
		{
			public bool DoNotAutoList { get; }

			public int NavID { get; private set; }

            public string Header { get; private set; }

            public List<NavBarPage> Items { get; private set; }

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
			public NavBarCategory Parent { get; private set; }

			public int PageID { get; private set; }

			public string Title { get; private set; }

            public string URL { get; private set; }

            public bool HiddenInLayout { get; private set; }

            public bool IsQuickLink { get; private set; }

            public static NavBarPage CreateForContent(NavBarCategory parent, int contentID, int pageID, string title, bool hiddenInLayout = false, bool isQuickLink = false)
			{
				string url = string.Format("Content/Info?title={0}&contentID={1}", CompatibilityHelper.ConvertStringForSEO(title), contentID);
				return new NavBarPage(parent, title, url, pageID, hiddenInLayout, isQuickLink);
			}

			public static NavBarPage CreateForUrl(NavBarCategory parent, string url, int pageID, string title, bool hiddenInLayout = false, bool isQuickLink = false)
			{
				if (string.IsNullOrEmpty(url))
				{
					url = "";
				}
				while (url != null && url.Length > 0 && (url[0] == '~' || url[0] == '/' || url[0] == '\\'))
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

		public const string ContentLayoutPath = "~/Pages/Content/_ContentLayout.cshtml";

		public const string SiteURL = "https://aesob.org.tr";

		private static NavBarCategory _adminCategory;

		private static NavBarCategory _loginCategory;

		public static NavBarCategory AdminCategory
		{
			get
			{
				if (_adminCategory.NavID != 100)
				{
					List<NavBarPage> adminBarElements = new List<NavBarPage>();
					int categoryIndex = 100;
					_adminCategory = new NavBarCategory("Admin", categoryIndex, adminBarElements, true);
					int pageIndex = 0;
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/SMSServiceInterface", pageIndex++, "Toplu SMS"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/AesobTVEditor", pageIndex++, "Aesob TV Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/AnlasmaEditor", pageIndex++, "Anlaşma Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/DuyuruEditor", pageIndex++, "Duyuru Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/DergiEditor", pageIndex++, "Dergi Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/GenelgeEditor", pageIndex++, "Genelge Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/HaberEditor", pageIndex++, "Haber Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/IcerikEditor", pageIndex++, "Icerik Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/KurulEditor", pageIndex++, "Kurul Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/OdaEditor", pageIndex++, "Oda Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/PopupEditor", pageIndex++, "Popup Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrl(_adminCategory, "~/Admin/PhoneEditor", pageIndex++, "Telefon Numarası Editor"));
					adminBarElements.Add(NavBarPage.CreateForUrlRaw(_adminCategory, "Login?handler=Logout", pageIndex++, "Çıkış", true));
				}
				return _adminCategory;
			}
		}

		public static NavBarCategory LoginCategory
		{
			get
			{
				if (_loginCategory.NavID != 101)
				{
					int categoryIndex = 101;
					List<NavBarPage> loginElements = new List<NavBarPage>();
					_loginCategory = new NavBarCategory("Giriş", categoryIndex++, loginElements, true);
					int pageIndex = 0;
					loginElements.Add(NavBarPage.CreateForUrl(_loginCategory, "~/Login", pageIndex++, "Giriş yap"));
				}
				return _loginCategory;
			}
		}

		public static IEnumerable<NavBarCategory> NavBarElements
		{
			get
			{
				int categoryIndex = 0;
				List<NavBarPage> navBarElements6 = new List<NavBarPage>();
				NavBarCategory navBar6 = new NavBarCategory("Başkan", categoryIndex++, navBarElements6);
				int pageIndex4 = 0;
				navBarElements6.Add(NavBarPage.CreateForContent(navBar6, 1, pageIndex4++, "Özgeçmişi"));
				navBarElements6.Add(NavBarPage.CreateForContent(navBar6, 2, pageIndex4++, "Çalışma Ofisi"));
				navBarElements6.Add(NavBarPage.CreateForContent(navBar6, 3, pageIndex4++, "Görevleri"));
				navBarElements6.Add(NavBarPage.CreateForContent(navBar6, 4, pageIndex4, "Projeleri"));
				yield return navBar6;
				List<NavBarPage> navBarElements = new List<NavBarPage>();
				NavBarCategory navBar = new NavBarCategory("Kurumsal", categoryIndex++, navBarElements);
				int pageIndex = 0;
				navBarElements.Add(NavBarPage.CreateForContent(navBar, 5, pageIndex++, "Amacı"));
				navBarElements.Add(NavBarPage.CreateForContent(navBar, 6, pageIndex++, "Tarihçe"));
				navBarElements.Add(NavBarPage.CreateForContent(navBar, 7, pageIndex++, "Ahi Evran"));
				navBarElements.Add(NavBarPage.CreateForContent(navBar, 8, pageIndex++, "Ahilik"));
				navBarElements.Add(NavBarPage.CreateForContent(navBar, 9, pageIndex, "Gedik Teşkilatı"));
				yield return navBar;
				List<NavBarPage> navBarElements2 = new List<NavBarPage>();
				NavBarCategory navBar2 = new NavBarCategory("Organizasyon", categoryIndex++, navBarElements2);
				int pageIndex3 = 0;
				navBarElements2.Add(NavBarPage.CreateForUrl(navBar2, GetUnionPageLink(true, 2), pageIndex3++, "Merkez Meslek Odaları"));
				navBarElements2.Add(NavBarPage.CreateForUrl(navBar2, GetUnionPageLink(false, 39), pageIndex3++, "İlçe Meslek Odaları"));
				navBarElements2.Add(NavBarPage.CreateForContent(navBar2, 11, pageIndex3++, "Yönetim Kurulu"));
				navBarElements2.Add(NavBarPage.CreateForContent(navBar2, 12, pageIndex3++, "Denetim Kurulu"));
				navBarElements2.Add(NavBarPage.CreateForContent(navBar2, 13, pageIndex3++, "Disiplin Kurulu"));
				navBarElements2.Add(NavBarPage.CreateForContent(navBar2, 10, pageIndex3++, "AESOB Adres ve Tel."));
				navBarElements2.Add(NavBarPage.CreateForContent(navBar2, 14, pageIndex3, "Organizasyon Şeması"));
				yield return navBar2;
				List<NavBarPage> navBarElements3 = new List<NavBarPage>();
				NavBarCategory navBar4 = new NavBarCategory("Çalışma Alanları", categoryIndex++, navBarElements3);
				int pageIndex5 = 0;
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 28, pageIndex5++, "Sicil Müdürlüğü İşlemleri"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 29, pageIndex5++, "Hukuk ve Den. İşlemleri"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 30, pageIndex5++, "Mesleki Eğitim"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 31, pageIndex5++, "Kapasite Raporları"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 32, pageIndex5++, "Finansman"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 33, pageIndex5++, "Sosyal Güvenlik"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 34, pageIndex5++, "Vergi"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 35, pageIndex5++, "Ahilik Kültürü Haftası"));
				navBarElements3.Add(NavBarPage.CreateForContent(navBar4, 36, pageIndex5, "İstihdam"));
				yield return navBar4;
				List<NavBarPage> navBarElements4 = new List<NavBarPage>();
				NavBarCategory navBar5 = new NavBarCategory("Mevzuat", categoryIndex++, navBarElements4);
				int pageIndex6 = 0;
				navBarElements4.Add(NavBarPage.CreateForContent(navBar5, 23, pageIndex6++, "Kanun ve Yönetmelikler"));
				navBarElements4.Add(NavBarPage.CreateForUrl(navBar5, "~/Content/Circulars", pageIndex6++, "Genelgeler"));
				navBarElements4.Add(NavBarPage.CreateForContent(navBar5, 24, pageIndex6++, "Yönerge ve Ana Sözleş."));
				navBarElements4.Add(NavBarPage.CreateForUrl(navBar5, "~/Content/Deals", pageIndex6++, "Anlaşma ve Protokoller"));
				navBarElements4.Add(NavBarPage.CreateForContent(navBar5, 25, pageIndex6++, "İlgili Mevzuatlar"));
				navBarElements4.Add(NavBarPage.CreateForContent(navBar5, 15, pageIndex6++, "İşyeri Açarken", false, true));
				navBarElements4.Add(NavBarPage.CreateForContent(navBar5, 16, pageIndex6, "Esnaf Olma Şartları", false, true));
				yield return navBar5;
				List<NavBarPage> navBarElements7 = new List<NavBarPage>();
				NavBarCategory navBar7 = new NavBarCategory("İletişim", categoryIndex++, navBarElements7);
				int pageIndex7 = 0;
				navBarElements7.Add(NavBarPage.CreateForUrl(navBar7, "~/Content/Contact", pageIndex7++, "İletişim"));
				navBarElements7.Add(NavBarPage.CreateForUrl(navBar7, "~/Content/Location", pageIndex7, "Biz Neredeyiz"));
				yield return navBar7;
				List<NavBarPage> navBarElements5 = new List<NavBarPage>();
				NavBarCategory navBar3 = new NavBarCategory("Yayınlarımız", categoryIndex, navBarElements5);
				int pageIndex2 = 0;
				navBarElements5.Add(NavBarPage.CreateForUrl(navBar3, "~/Content/Magazines", pageIndex2++, "BİRLİK Dergisi"));
				navBarElements5.Add(NavBarPage.CreateForUrl(navBar3, "~/Content/AesobTV", pageIndex2++, "AESOB TV"));
				navBarElements5.Add(NavBarPage.CreateForUrl(navBar3, "~/Content/NewsFeed", pageIndex2, "Haberler"));
				yield return navBar3;
				yield return AdminCategory;
				yield return LoginCategory;
			}
		}

		public static string GetNewsLink(Haberler news, bool isExternal = false)
		{
			return GetFormattedURL(isExternal, string.Format("Content/News?haber={0}&baslik={1}", news.Id, CompatibilityHelper.ConvertStringForSEO(news.Baslik.ToLower())));
		}

		public static string GetAnnouncementLink(Duyurular duyuru, bool isExternal = false)
		{
			object arg = duyuru.Id;
			string baslik = duyuru.Baslik;
			return GetFormattedURL(isExternal, string.Format("Content/Announcement?announcementID={0}&shortDescription={1}", arg, CompatibilityHelper.ConvertStringForSEO((baslik != null) ? baslik.ToLower() : null)));
		}

		public static string GetCircularLink(Genelgeler circular, bool isExternal = false)
		{
			return GetFormattedURL(isExternal, "images/Genelgeler/" + circular.Yolu);
		}

		public static string GetBoardLink(int kurulID, bool isExternal = false)
		{
			return GetFormattedURL(isExternal, string.Format("Content/Board?boardID={0}", kurulID));
		}

		public static string GetUnionPageLink(bool isCentral, int union, bool isExternal = false)
		{
			return GetFormattedURL(isExternal, string.Format("Content/Unions?type={0}&union={1}", isCentral ? 1 : 0, union));
		}

		public static string GetNotFoundPageURL()
		{
			return "/NotFound";
		}

		private static string GetFormattedURL(bool isExternal, string subPageLink)
		{
			if (isExternal)
			{
				return "https://aesob.org.tr/" + subPageLink;
			}
			return "~/" + subPageLink;
		}
	}
}
