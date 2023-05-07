using System.Collections.Generic;
using System.Xml;
using aesob.org.tr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AesobWebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmailController : ControllerBase
	{
		[HttpPost("Redirect")]
		public IActionResult RedirectEMails([FromBody] string emailXml)
		{
			string decodedXml = Base64UrlEncoder.Decode(emailXml);
			return SendEmailFromDocument(decodedXml);
		}

		private IActionResult SendEmailFromDocument(string emailXml)
		{
			string subject = string.Empty;
			string content = string.Empty;
			List<string> targetAddresses = new List<string>();
			string senderAlias = string.Empty;
			Dictionary<string, byte[]> attachments = new Dictionary<string, byte[]>();
			XmlDocument document = new XmlDocument();
			document.LoadXml(emailXml);
			if (document.FirstChild != null)
			{
				foreach (object nodeObj in document.FirstChild.ChildNodes)
				{
					XmlElement element = nodeObj as XmlElement;
					if (element == null)
					{
						continue;
					}
					if (element.Name == "Auth")
					{
						string authToken = element.InnerText;
						if (authToken != "AesobEmailEncryiptionProtocol123456789")
						{
							return ServiceActionResult.CreateFail("Invalid Authorization Info");
						}
					}
					if (element.Name == "TargetEmails")
					{
						foreach (object targetMailNodeObj in element.ChildNodes)
						{
							XmlElement mailNodeObj = targetMailNodeObj as XmlElement;
							if (mailNodeObj != null)
							{
								targetAddresses.Add(mailNodeObj.InnerText);
							}
						}
					}
					if (element.Name == "Attachments")
					{
						foreach (object attachmentNode in element.ChildNodes)
						{
							XmlElement attachmentElem = attachmentNode as XmlElement;
							if (attachmentElem != null)
							{
								string attachmentName = attachmentElem.GetAttribute("Name");
								byte[] bytes = Base64UrlEncoder.DecodeBytes(attachmentElem.InnerText);
								while (attachments.ContainsKey(attachmentName))
								{
									attachmentName = "_" + attachmentName;
								}
								attachments.Add(attachmentName, bytes);
							}
						}
					}
					if (element.Name == "Subject")
					{
						subject = element.InnerText;
					}
					if (element.Name == "SenderAlias")
					{
						senderAlias = element.InnerText;
					}
					if (element.Name == "Content")
					{
						content = element.InnerText;
					}
				}
			}
			senderAlias = senderAlias.Replace("{aesob_newline}", "<br>");
			subject = subject.Replace("{aesob_newline}", "<br>");
			content = content.Replace("{aesob_newline}", "<br>");
			EmailService.MailData mailData = new EmailService.MailData(senderAlias, subject, content, targetAddresses.ToArray());
			foreach (KeyValuePair<string, byte[]> atth in attachments)
			{
				mailData.Attachments.Add(atth.Key, atth.Value);
			}
			return EmailService.SendRedirectEmail(mailData);
		}
	}
}
