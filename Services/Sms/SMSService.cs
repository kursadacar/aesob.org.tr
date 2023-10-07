using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;

namespace aesob.org.tr.Services.Sms
{
	public static class SMSService
	{
		private static bool _isInitialized;

		private static string _smsUserName;

		private static string _smsUserPass;

		private static string _smsAlias;

		public static void Initialize(string smsUserName, string smsUserPass, string smsAlias)
		{
			if (!_isInitialized)
			{
				_smsUserName = smsUserName;
				_smsUserPass = smsUserPass;
				_smsAlias = smsAlias;
				_isInitialized = true;
			}
		}

		public static ServiceActionResult SendMassSms(string message)
		{
			if (!_isInitialized)
			{
				ServiceActionResult.CreateFail("SMS Service is not initialized");
			}

			List<string> targetReceivers = null;
#if DEBUG
			targetReceivers = new List<string>()
			{
				"905534968861",
			};
#else
			targetReceivers = SMSHelper.GetPhoneAddressesForAESOB();
#endif

            if (targetReceivers == null || targetReceivers.Count == 0)
			{
				return ServiceActionResult.CreateFail("No phone numbers added to SMS");
			}
			try
			{
				//string messageBody = CompatibilityHelper.ReplaceTurkishChars(message);
				SMSObject sms = new SMSObject(_smsUserName, _smsUserPass, _smsAlias);
				sms.SetBody(message);
				sms.SetBeginDate(DateTime.Now);
				sms.SetEndDate(DateTime.Now.AddMinutes(10.0));
				sms.AddNumbers(targetReceivers);
				return SendSms(sms);
			}
			catch (Exception e)
			{
				return ServiceActionResult.CreateFail(e.Message);
			}
		}

		public static ServiceActionResult SendAnnouncementMessage(Genelgeler circular, params Phone[] phones)
		{
			if (!_isInitialized)
			{
				return ServiceActionResult.CreateFail("SMS Service is not initialized");
			}
			if (phones.Length == 0)
			{
				return ServiceActionResult.CreateFail("No phone numbers added to SMS");
			}
			string announcementTitle = circular.Konu;
			string announcementLink = NavigationHelper.GetCircularLink(circular, true);
			string messageBody = "Yeni Genelge:\"" + announcementTitle + "\": " + announcementLink + "\n -Antalya Esnaf ve Sanatkarlar Birliği Odası";
			try
			{
				SMSObject sms2 = new SMSObject(_smsUserName, _smsUserPass, _smsAlias);
				sms2.SetBody(messageBody);
				sms2.SetBeginDate(DateTime.Now);
				sms2.SetEndDate(DateTime.Now.AddMinutes(10.0));
				sms2.AddNumbers(phones.Select((Phone p) => p.PhoneNumber));
				return SendSms(sms2);
			}
			catch
			{
				messageBody = CompatibilityHelper.ReplaceTurkishChars(messageBody);
				SMSObject sms = new SMSObject(_smsUserName, _smsUserPass, _smsAlias);
				sms.SetBody(messageBody);
				sms.SetBeginDate(DateTime.Now);
				sms.SetEndDate(DateTime.Now.AddMinutes(10.0));
				sms.AddNumbers(phones.Select((Phone p) => p.PhoneNumber));
				try
				{
					return SendSms(sms);
				}
				catch (Exception e)
				{
					return ServiceActionResult.CreateFail(e.Message);
				}
			}
		}

		private static string CreateMessageSection(string message, string numbersSection, string beginDate, string endDate)
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<Message>");
			sb.AppendLine("<Msgbody>");
			sb.Append(message);
			sb.Append("</Msgbody>");
			sb.AppendLine("</Message>");
			sb.Append("<Numbers>" + numbersSection + "</Numbers>\n");
			sb.Append("<SDate>" + beginDate + "</SDate>\n");
			sb.Append("<EDate>" + endDate + "</EDate>\n");
			return sb.ToString();
		}

		private static ServiceActionResult SendSms(SMSObject sms)
		{
			string url = SMSHelper.GetURLForSMS(sms);
			string smsContent = sms.ToString();
			WebClient client = new WebClient();
			MemoryStream requestStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(requestStream);
			streamWriter.WriteLine(smsContent);
			streamWriter.Close();

			try
			{
				byte[] response = client.UploadData(url, "POST", requestStream.ToArray());
				MemoryStream responseStream = new MemoryStream(response);
				StreamReader streamReader = new StreamReader(responseStream);
				string responseMessage = streamReader.ReadLine();
				streamReader.Close();
				if (responseMessage == null)
				{
					return ServiceActionResult.CreateFail("No response from SMS Server");
				}
				if (responseMessage.StartsWith("ERR"))
				{
					return ServiceActionResult.CreateFail("SMS Server returned error code: " + responseMessage);
				}
				return ServiceActionResult.CreateSuccess("SMS sent successfully: " + responseMessage);
			}
			catch (Exception e)
			{
				return ServiceActionResult.CreateFail("Error during sending bulk SMS: " + e.Message);
			}
		}
	}
}
