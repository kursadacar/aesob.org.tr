using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using aesob.org.tr.Utilities;
using Aesob.Web.Library.Email;
using Microsoft.AspNetCore.Hosting;

namespace aesob.org.tr.Services
{
	public static class EmailService
	{
		private const string _invalidTokenError = "Invalid authorization token";

		private static bool _isSendingEmail;

		private static Queue<AuthorizedMailData> _pendingEmails = new Queue<AuthorizedMailData>();

		#region Internal Emails (No Auth)

		public static ServiceActionResult SendContactEmail(EMailData mailData)
		{
			var authorizedMailData = new AuthorizedMailData("admin@aesob.org.tr", "yF7_y1n917Bjx320?d", mailData);

			return SendMail(authorizedMailData);
		}

		public static ServiceActionResult SendBulletinEmail(EMailData mailData)
		{
            var authorizedMailData = new AuthorizedMailData("bulletin@aesob.org.tr", "!Asperox123.", mailData);

            return SendMail(authorizedMailData);
		}

		#endregion

		#region External Emails (Auth)

		public static ServiceActionResult SendReportEmail(ExternalEMailData mailData)
		{
            if (!mailData.IsAuthTokenValid())
            {
                return ServiceActionResult.CreateFail(_invalidTokenError);
            }

            var authorizedMailData = new AuthorizedMailData("reports@aesob.org.tr", "ioVmxQe2Jm3G7ArY", mailData);

            return SendMail(authorizedMailData);
		}

        public static ServiceActionResult SendRedirectEmail(ExternalEMailData mailData)
        {
            if (!mailData.IsAuthTokenValid())
            {
                return ServiceActionResult.CreateFail(_invalidTokenError);
            }

            var authorizedMailData = new AuthorizedMailData("kepredir@aesob.org.tr", "AesobKep123456", mailData);

            return SendMail(authorizedMailData);
        }

        #endregion

		private static ServiceActionResult SendMail(AuthorizedMailData authorizedMailData)
		{
			if (_isSendingEmail)
			{
				_pendingEmails.Enqueue(authorizedMailData);
                return ServiceActionResult.CreateSuccess("Mail Service is currently busy, mail request is queued");
            }

			var senderUser = authorizedMailData.AccountEmail;
			var senderPassword = authorizedMailData.AccountPassword;
			var mailData = authorizedMailData.MailData;

			string[] targetAddresses = mailData.TargetAddresses;

#if DEBUG
			targetAddresses = new string[] { "kursad.fb.96@hotmail.com" };
#endif

			if (targetAddresses == null || targetAddresses.Length == 0)
			{
				return ServiceActionResult.CreateFail("Invalid target addresses for mail.");
			}

			_isSendingEmail = true;

			try
			{
				MailMessage mailMessage = new MailMessage();
				mailMessage.From = new MailAddress(senderUser, mailData.SenderAlias);
				mailMessage.Subject = mailData.Subject;
				mailMessage.IsBodyHtml = true;
				mailMessage.Body = mailData.Content;
				foreach (var attachment in mailData.Attachments)
				{
					MemoryStream ms = new MemoryStream(attachment.Value);
					mailMessage.Attachments.Add(new Attachment(ms, attachment.Name));
				}
				SmtpClient smtpClient = new SmtpClient("srvm11.trwww.com");
				smtpClient.Port = 587;
				smtpClient.EnableSsl = true;
				smtpClient.Credentials = new NetworkCredential
				{
					UserName = senderUser,
					Password = senderPassword
				};

				var mailTask = SendMailToAddresses(smtpClient, mailMessage, targetAddresses);

				mailTask.Wait();
				return mailTask.Result;
			}
			catch (Exception e)
			{
				try
				{
					IWebHostEnvironment webHostEnv = Common.GetWebHostEnviroment();
					if (webHostEnv != null)
					{
						using (StreamWriter streamWriter = new StreamWriter(webHostEnv.WebRootPath + "\\Logs\\erros"))
						{
							StringBuilder sb = new StringBuilder();
							sb.Append(DateTime.Now.ToString("[ss:mm:HH - dd/MM/yyyy]"));
							sb.Append(" ERROR: ");
							sb.AppendLine(e.Message);
							sb.AppendLine();
							sb.AppendLine(e.InnerException.Message);
							sb.AppendLine();
							sb.AppendLine(e.StackTrace);
							streamWriter.Write(sb.ToString());
						}
					}
				}
				catch
				{
					Debugger.Break();
				}

				return ServiceActionResult.CreateFail("An error occured while sending emails: " + e.Message);
			}
			finally
			{
				_isSendingEmail = false;
                if (_pendingEmails.Count > 0)
                {
                    var mail = _pendingEmails.Dequeue();
                    SendMail(mail);
                }
            }
		}

		private static async Task<ServiceActionResult> SendMailToAddresses(SmtpClient client, MailMessage message, string[] addresses)
		{
			if(client == null || message== null || addresses == null)
			{
				return ServiceActionResult.CreateFail("Mail gönderilirken bir hata oluştu. Geçersiz mail konfigurasyonu.");
			}

			List<string> faultyAddresses = new List<string>();

            for (int i = 0; i < addresses.Length; i++)
            {
                if (!string.IsNullOrEmpty(addresses[i]))
                {
					try
					{
                        message.To.Add(addresses[i].Trim());
                        client.Send(message);
                        message.To.Clear();
                        await Task.Delay(10);
                    }
					catch
					{
						faultyAddresses.Add(addresses[i]);
					}
                }
            }

			if(faultyAddresses.Count == 0)
			{
				return ServiceActionResult.CreateSuccess("Tüm e-posta'lar başarı ile gönderildi");
			}

			StringBuilder faultyAddressSb = new StringBuilder();
			
			faultyAddressSb.AppendLine($"Aşağıdaki adreslere mail gönderimi başarısız oldu");
			for(int i = 0; i < faultyAddresses.Count; i++)
			{
				faultyAddressSb.AppendLine($"- {faultyAddresses[i]}");
			}

			return ServiceActionResult.CreateFail(faultyAddressSb.ToString());
        }

		public static bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}
			if (email.Any((char c) => c > '\u007f'))
			{
				return false;
			}
			try
			{
				MailAddress addr = new MailAddress(email);
				return ((addr != null) ? addr.Address : null) == email;
			}
			catch
			{
				return false;
			}
		}

		private struct AuthorizedMailData
		{
			public string AccountEmail { get; }

			public string AccountPassword { get; }

			public EMailData MailData { get; }

			public AuthorizedMailData(string accountEmail, string accountPassword, EMailData mailData)
			{
				AccountEmail = accountEmail;
				AccountPassword = accountPassword;
				MailData = mailData;
			}
		}
    }
}
