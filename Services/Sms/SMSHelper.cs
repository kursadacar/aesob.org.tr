using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;

namespace aesob.org.tr.Services.Sms
{
	public static class SMSHelper
	{
		public static string GetFormattedDateForSMS(DateTime date)
		{
			Func<string, string> getTwoCharacterDate = (string dateString) => (dateString.Length == 1) ? ("0" + dateString) : dateString;
			return getTwoCharacterDate(date.Year.ToString()) + getTwoCharacterDate(date.Month.ToString()) + getTwoCharacterDate(date.Day.ToString()) + getTwoCharacterDate(date.Hour.ToString()) + getTwoCharacterDate(date.Minute.ToString());
		}

		public static string GetURLForSMS(SMSObject sms)
		{
			if (DoesTextContainNonGSMCharacters(sms.Body))
			{
				if (sms.Body.Length > 160)
				{
					return "https://server.mobilrem.com/developer/bulksmsv3/lTRbulksms.php";
				}
				return "https://server.mobilrem.com/developer/bulksmsv3/TRbulksms.php";
			}
			if (sms.Body.Length > 160)
			{
				return "https://server.mobilrem.com/developer/bulksmsv3/lTRbulksms.php";
			}
			return "https://server.mobilrem.com/developer/bulksmsv3/bulksms.php";
		}

		public static bool DoesTextContainNonGSMCharacters(string text)
		{
			string gsmCharacters = "@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞ\u001bÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijklmnopqrstuvwxyzäöñüà";
			for (int i = 0; i < text.Length; i++)
			{
				if (!gsmCharacters.Contains(text[i]))
				{
					return true;
				}
			}
			return false;
		}

		public static string FormatAllNumbersForSMS(List<string> numbers)
		{
			StringBuilder phoneNumberBuilder = new StringBuilder();
			for (int i = numbers.Count - 1; i >= 0; i--)
			{
				string phoneNumber = numbers[i];
				phoneNumber = phoneNumber.Replace("(", "");
				phoneNumber = phoneNumber.Replace(")", "");
				phoneNumber = phoneNumber.Replace(" ", "");
				phoneNumber = phoneNumber.Trim();
				if (!string.IsNullOrEmpty(phoneNumber) && !phoneNumber.Any((char c) => !char.IsNumber(c)))
				{
					string formatted = GetFormattedPhoneNumberForSMS(phoneNumber);
					if (formatted.Length == 10)
					{
						phoneNumberBuilder.Append("90");
						phoneNumberBuilder.Append(formatted);
						phoneNumberBuilder.Append(',');
					}
				}
			}
			phoneNumberBuilder.Remove(phoneNumberBuilder.Length - 1, 1);
			return phoneNumberBuilder.ToString();
		}

		public static string GetFormattedPhoneNumberForSMS(string number)
		{
			if (!string.IsNullOrEmpty(number))
			{
				if (number.StartsWith("+90"))
				{
					number = number.Remove(0, 3);
				}
				else if (number.StartsWith("90"))
				{
					number = number.Remove(0, 2);
				}
				else if (number.StartsWith("0"))
				{
					number = number.Remove(0, 1);
				}
			}
			return number;
		}

		public static List<string> GetPhoneAddressesForAESOB()
		{
			string connectionString = "Data Source=mssql06.trwww.com;Initial Catalog=sicil;User ID=kursad;Password=Asperox123.";
			string sqlQuery = "SELECT CEPTEL FROM SICIL WHERE CEPTEL IS NOT NULL";
			List<string> phoneNumbersList = new List<string>();
			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				connection.Open();
				using (SqlCommand command = new SqlCommand(sqlQuery, connection))
				{
					SqlDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						try
						{
							string phoneNumber = reader.GetString(0);
							if (phoneNumber != null)
							{
								phoneNumbersList.Add(phoneNumber);
							}
						}
						catch (Exception)
						{
						}
					}
					return phoneNumbersList;
				}
			}
		}
	}
}
