using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Hosting;

namespace aesob.org.tr.Services
{
	public static class EmailService
	{
		public struct MailData
		{
			public string SenderAlias { get; set; }

			public string Subject { get; set; }

            public string Content { get; set; }

            public string[] TargetAddresses { get; set; }

            public Dictionary<string, byte[]> Attachments { get; set; }

            public MailData(string senderAlias, string subject, string content, params string[] targetAddresses)
			{
				SenderAlias = senderAlias;
				Subject = subject;
				Content = content;
				TargetAddresses = targetAddresses;
				Attachments = new Dictionary<string, byte[]>();
			}
		}

		private static bool _isSendingEmail;

		private static Queue<MailData> _pendingRedirectes = new Queue<MailData>();

		private static Queue<MailData> _pendingContactEmails = new Queue<MailData>();

		private static Queue<MailData> _pendingBulletinEmails = new Queue<MailData>();

		public static ServiceActionResult SendRedirectEmail(MailData mailData)
		{
			if (_isSendingEmail)
			{
				_pendingRedirectes.Enqueue(mailData);
				return ServiceActionResult.CreateSuccess("Mail Service is currently busy, mail request is queued");
			}
			return SendMail("kepredir@aesob.org.tr", "AesobKep123456", mailData);
		}

		public static ServiceActionResult SendContactEmail(MailData mailData)
		{
			if (_isSendingEmail)
			{
				_pendingContactEmails.Enqueue(mailData);
				return ServiceActionResult.CreateSuccess("Mail Service is currently busy, mail request is queued");
			}

			return SendMail("admin@aesob.org.tr", "yF7_y1n917Bjx320?d", mailData);
		}

		public static ServiceActionResult SendBulletinEmail(MailData mailData)
		{
			if (_isSendingEmail)
			{
				_pendingBulletinEmails.Enqueue(mailData);
				return ServiceActionResult.CreateSuccess("Mail Service is currently busy, mail request is queued");
			}

			return SendMail("bulletin@aesob.org.tr", "!Asperox123.", mailData);
		}

		private static ServiceActionResult SendMail(string senderUser, string senderPassword, MailData mailData)
		{
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
				foreach (KeyValuePair<string, byte[]> attachment in mailData.Attachments)
				{
					MemoryStream ms = new MemoryStream(attachment.Value);
					mailMessage.Attachments.Add(new Attachment(ms, attachment.Key));
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
				SendPendingEmails();
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

		private static void SendPendingEmails()
		{
			if (_pendingBulletinEmails.Count > 0)
			{
				MailData mail3 = _pendingBulletinEmails.Dequeue();
				SendBulletinEmail(mail3);
			}
			else if (_pendingContactEmails.Count > 0)
			{
				MailData mail2 = _pendingContactEmails.Dequeue();
				SendContactEmail(mail2);
			}
			else if (_pendingRedirectes.Count > 0)
			{
				MailData mail = _pendingRedirectes.Dequeue();
				SendRedirectEmail(mail);
			}
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

		private static string GetFormattedAddresses(List<string> addresses)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < addresses.Count; i++)
			{
				sb.AppendLine(addresses[i]);
			}
			return sb.ToString();
		}
	}
}
