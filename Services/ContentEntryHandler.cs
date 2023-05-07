using System.Linq;
using aesob.org.tr.Models;
using aesob.org.tr.Services.Sms;

namespace aesob.org.tr.Services
{
	public static class ContentEntryHandler
	{
		public static ServiceActionResult OnContentAdded(IAesobEntity entity, AesobDbContext dbContext)
		{
			Haberler news = entity as Haberler;
			if (news != null)
			{
				EBulten[] targetAddresses = dbContext.EBultens.Where((EBulten x) => x.EMail != null && x.IsEmailActive).ToArray();
				return BulletinService.OnAddNews(news, targetAddresses);
			}
			Genelgeler circular = entity as Genelgeler;
			if (circular != null)
			{
				Phone[] targetNumbers = dbContext.Phones.Where((Phone x) => x.IsActive && x.Name != null && x.PhoneNumber != null).ToArray();
				return SMSService.SendAnnouncementMessage(circular, targetNumbers);
			}
			return ServiceActionResult.CreateSuccess("Content Added");
		}
	}
}
