using System.Collections.Generic;
using System.Linq;
using System.Text;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;

namespace aesob.org.tr.Services
{
	public static class BulletinService
	{
		private class BulletinComparer : IComparer<EBulten>
		{
			private const string _adminGroup = "Admin";

			public int Compare(EBulten x, EBulten y)
			{
				if (x.Group == "Admin" && y.Group != "Admin")
				{
					return -99;
				}
				if (x.Group != "Admin" && y.Group == "Admin")
				{
					return 99;
				}
				if (x.Group == null && y.Group != null)
				{
					return 49;
				}
				if (x.Group != null && y.Group == null)
				{
					return -49;
				}
				if (x.Group == null && y.Group == null)
				{
					return 0;
				}
				return x.Group.CompareTo(y.Group);
			}
		}

		public static ServiceActionResult OnAddNews(Haberler news, params EBulten[] targetAddresses)
		{
			List<string> targetEmails = GetTargetEmails(targetAddresses);
			string emailContent = GetFormattedEmailContent(news.Baslik, news.Icerik, "https://aesob.org.tr/images/Haber/Large/" + news.Resimyolu, NavigationHelper.GetNewsLink(news, true));
			return SendEmail("Antalya Esnaf ve Sanatkarlar Odası Birliği", "Yeni Haber: " + news.Baslik, emailContent, targetEmails.ToArray());
		}

		public static ServiceActionResult OnAddAnnouncements(Duyurular announcement, params EBulten[] targetAddresses)
		{
			List<string> targetEmails = GetTargetEmails(targetAddresses);
			string emailContent = GetFormattedEmailContent(announcement.Baslik, announcement.Icerik, null, NavigationHelper.GetAnnouncementLink(announcement, true));
			return SendEmail("Antalya Esnaf ve Sanatkarlar Odaları Birliği", "Yeni Duyuru: " + announcement.Baslik, emailContent, targetEmails.ToArray());
		}

		private static string GetFormattedEmailContent(string header, string content, string imageUrl, string url)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<div style=\"font-family: Helvetica; padding: 15px 0px; background-color:#eaeaea; width:100%; border: 1px solid #252525;\">");
			if (!string.IsNullOrEmpty(header))
			{
				sb.AppendLine("<div style=\"text-align:center; font-size:20px; font-weight:200; \">");
				sb.AppendLine(header);
				sb.AppendLine("</div>");
			}
			if (!string.IsNullOrEmpty(imageUrl))
			{
				sb.AppendLine();
				sb.AppendLine("<div style=\"text-align:center; font-size:18px; \">");
				sb.AppendLine("<img style=\"max-width:80%; max-height:500px; object-fit:contain;\" src=\"" + imageUrl + "\" />");
				sb.AppendLine("</div>");
			}
			if (!string.IsNullOrEmpty(content))
			{
				sb.AppendLine();
				sb.AppendLine("<div style=\"text-align:center; font-size:20px; font-weight:100;\" >");
				sb.AppendLine(content);
				sb.AppendLine("</div>");
			}
			if (!string.IsNullOrEmpty(url))
			{
				sb.AppendLine();
				sb.AppendLine("<div style=\"text-align:center; font-size:18px; \">");
				sb.AppendLine("<a href=" + url + ">Daha fazla bilgi için lütfen tıklayın.</a>");
				sb.AppendLine("</div>");
			}
			sb.AppendLine("</div>");
			return sb.ToString();
		}

		private static ServiceActionResult SendEmail(string senderAlias, string subject, string content, params string[] targetAddresses)
		{
			return EmailService.SendBulletinEmail(new EmailService.MailData(senderAlias, subject, content, targetAddresses));
		}

		private static List<string> GetTargetEmails(EBulten[] bulletins)
		{
			List<EBulten> bulletinList = bulletins.ToList();
			bulletinList.Sort(new BulletinComparer());
			List<string> list = new List<string>();
			foreach (EBulten bulletin in bulletinList)
			{
				string email = bulletin.EMail.Replace(" ", "");
				list.Add(email);
			}
			return list;
		}
	}
}
