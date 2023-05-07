using System.Collections.Generic;
using System.Xml;
using aesob.org.tr.Services;
using aesob.org.tr.Services.Sms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AesobWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SMSController : ControllerBase
	{
		[HttpPost("MassSMS")]
		public IActionResult SendMassSms([FromBody] string emailXml)
		{
			string decodedXml = Base64UrlEncoder.Decode(emailXml);
			return SendSMSFromXML(decodedXml);
		}

		private IActionResult SendSMSFromXML(string emailXml)
		{
			string content = string.Empty;
			List<string> targetAddresses = new List<string>();
			XmlDocument document = new XmlDocument();
			document.LoadXml(emailXml);
			if (document.DocumentElement != null && document.DocumentElement.Name == "SmsData")
			{
				foreach (XmlElement element in document.DocumentElement.ChildNodes)
				{
					if (element.Name == "Auth")
					{
						string authToken = element.InnerText;
						if (authToken != "AesobSmsEncryptionProt1593574682")
						{
							return ServiceActionResult.CreateFail("Invalid authorization info");
						}
					}
					if (element.Name == "TargetAddresses")
					{
						foreach (XmlElement mailNodeObj in element.ChildNodes)
						{
							targetAddresses.Add(mailNodeObj.InnerText);
						}
					}
					if (element.Name == "Content")
					{
						content = element.InnerText;
					}
				}
			}
			content = content.Replace("{aesob_newline}", "\n");
			return SMSService.SendMassSms(content);
		}
	}
}
