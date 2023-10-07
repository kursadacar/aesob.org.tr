using System.Collections.Generic;
using System.Xml;
using aesob.org.tr.Services;
using Aesob.Web.Library.Email;
using Aesob.Web.Library.Encyrption;
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
			var decryptedEmail = EncryptionHelper.DecryptText(emailXml);
			var mailData = new ExternalEMailData(decryptedEmail);

			return EmailService.SendRedirectEmail(mailData);
        }

		[HttpPost("Report")]
		public IActionResult SendReportEmails([FromBody] string emailXml)
		{
			var decryptedEmail = EncryptionHelper.DecryptText(emailXml);
			var mailData = new ExternalEMailData(decryptedEmail);

            return EmailService.SendReportEmail(mailData);
        }
    }
}
