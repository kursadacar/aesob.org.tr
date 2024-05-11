using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using aesob.org.tr.Models;
using aesob.org.tr.Utilities;

namespace aesob.org.tr.Services.Sms
{
	public static class SMSService
	{
		public static async Task<ServiceActionResult> SendMassSms(string message, DateTime sendDate, int genderFilter = -1)
		{
			List<string> targetReceivers = null;
#if DEBUG
			targetReceivers = new List<string>()
			{
				"905534968861",
				"905061795286",
			};
#else
			targetReceivers = SMSHelper.GetPhoneAddressesForAESOB((SMSHelper.MemberGender)genderFilter);
#endif

			if (targetReceivers == null || targetReceivers.Count == 0)
			{
				return ServiceActionResult.CreateFail("No phone numbers added to SMS");
			}
			try
			{
				//string messageBody = CompatibilityHelper.ReplaceTurkishChars(message);
				SMSObject sms = new SMSObject();
				sms.SetBody(message);
				sms.SetBeginDate(sendDate);
				sms.SetEndDate(sendDate.AddMinutes(10.0));
				sms.AddNumbers(targetReceivers);
				return await SendSmsAux(sms);
			}
			catch (Exception e)
			{
				return ServiceActionResult.CreateFail(e.Message);
			}
		}

		public static async Task<ServiceActionResult> SendAnnouncementMessage(Genelgeler circular, params Phone[] phones)
		{
			if (phones.Length == 0)
			{
				return ServiceActionResult.CreateFail("No phone numbers added to SMS");
			}

			List<string> receiverNumbers;

#if DEBUG
			receiverNumbers = new List<string>()
			{
				"905534968861",
				"905061795286",
			};
#else
			receiverNumbers = phones.Select(p => p.PhoneNumber).ToList();
#endif

			string announcementTitle = circular.Konu;
			string announcementLink = NavigationHelper.GetCircularLink(circular, true);
			string messageBody = "Yeni Genelge:\"" + announcementTitle + "\": " + announcementLink + "\n -Antalya Esnaf ve Sanatkarlar Birliği Odası";
			try
			{
				SMSObject sms2 = new SMSObject();
				sms2.SetBody(messageBody);
				sms2.SetBeginDate(DateTime.Now);
				sms2.SetEndDate(DateTime.Now.AddMinutes(10.0));
				sms2.AddNumbers(receiverNumbers);
				return await SendSmsAux(sms2);
			}
			catch
			{
				messageBody = CompatibilityHelper.ReplaceTurkishChars(messageBody);
				SMSObject sms = new SMSObject();
				sms.SetBody(messageBody);
				sms.SetBeginDate(DateTime.Now);
				sms.SetEndDate(DateTime.Now.AddMinutes(10.0));
				sms.AddNumbers(receiverNumbers);
				try
				{
					return await SendSmsAux(sms);
				}
				catch (Exception e)
				{
					return ServiceActionResult.CreateFail(e.Message);
				}
			}
		}

		private static async Task<ServiceActionResult> SendSmsAux(SMSObject sms)
		{
			return await TTMesajService.SendSms(sms);

			//string url = SMSHelper.GetURLForSMS(sms);
			//string smsContent = GetSmsXml(sms);
			//WebClient client = new WebClient();
			//MemoryStream requestStream = new MemoryStream();
			//StreamWriter streamWriter = new StreamWriter(requestStream);
			//streamWriter.WriteLine(smsContent);
			//streamWriter.Close();

			//try
			//{
			//	byte[] response = client.UploadData(url, "POST", requestStream.ToArray());
			//	MemoryStream responseStream = new MemoryStream(response);
			//	StreamReader streamReader = new StreamReader(responseStream);
			//	string responseMessage = streamReader.ReadLine();
			//	streamReader.Close();
			//	if (responseMessage == null)
			//	{
			//		return ServiceActionResult.CreateFail("No response from SMS Server");
			//	}
			//	if (responseMessage.StartsWith("ERR"))
			//	{
			//		return ServiceActionResult.CreateFail("SMS Server returned error code: " + responseMessage);
			//	}
			//	return ServiceActionResult.CreateSuccess("SMS sent successfully: " + responseMessage);
			//}
			//catch (Exception e)
			//{
			//	return ServiceActionResult.CreateFail("Error during sending bulk SMS: " + e.Message);
			//}
		}

		private static string GetSmsXml(SMSObject sms)
		{
            StringBuilder sb = new StringBuilder();

            string beginDateString = SMSHelper.GetFormattedDateForSMS(sms.BeginDate);
            string endDateString = SMSHelper.GetFormattedDateForSMS(sms.EndDate);
            List<string> formattedNumbers = SMSHelper.FormatAllNumbersForSMS(sms.Numbers);
			var numbersString = string.Join(',', formattedNumbers);

            sb.Append("<MainmsgBody>\n");
            sb.Append("<UserName>" + "---USERNAME---" + "</UserName>\n");
            sb.Append("<PassWord>" + "---PASSWORD---" + "</PassWord>\n");
            sb.Append("<Version>V.2</Version>\n");
            sb.Append("<Origin>" + "---ORIGIN---" + "</Origin>\n");
            sb.Append("<Mesgbody>" + sms.Body + "</Mesgbody>\n");
#if DEBUG
            sb.Append("<Numbers>905534968861</Numbers>\n");
#else
			sb.Append("<Numbers>" + numbersString + "</Numbers>\n");
#endif
            sb.Append("<SDate>" + beginDateString + "</SDate>\n");
            sb.Append("<EDate>" + endDateString + "</EDate>\n");
            sb.Append("</MainmsgBody>");

            return sb.ToString();
        }
	}
}
