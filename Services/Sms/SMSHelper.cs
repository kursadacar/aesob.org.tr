using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;

namespace aesob.org.tr.Services.Sms
{
	public static class SMSHelper
	{
		public enum MemberGender
		{
			None = -1,
			All = 0,
			Male = 1,
			Female = 2,
		}

		public struct MemberInfo
		{
			public int SicilNo { get; set; }
			public string AdSoyad { get; set; }
			public string TelefonNumarasi { get; set; }
			public string TCKimlikNo { get; set; }
		}

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

		public static List<string> FormatAllNumbersForSMS(IEnumerable<string> numbers)
		{
			List<string> result = new List<string>();

			var numbersList = numbers.ToList();

			for (int i = numbersList.Count - 1; i >= 0; i--)
			{
				string rawNumber = numbersList[i];

				if (!string.IsNullOrEmpty(rawNumber))
				{
                    var formattedNumber = GetFormattedPhoneNumberForSMS(rawNumber);
					if(formattedNumber.Length == 12)
					{
						result.Add(formattedNumber);
					}
				}
			}

			return result;
		}

		public static string GetFormattedPhoneNumberForSMS(string number)
		{
			if (!string.IsNullOrEmpty(number))
			{
				string newNumber = "";

				for(int i = 0; i< number.Length; i++)
				{
					if (char.IsNumber(number[i]))
					{
						newNumber += number[i];
					}
				}

				if (newNumber.StartsWith("90"))
				{
                    newNumber = number.Remove(0, 2);
				}
				else if (newNumber.StartsWith("0"))
				{
                    newNumber = newNumber.Remove(0, 1);
				}

				return "90" + newNumber;
			}

			return string.Empty;
		}

		public static List<MemberInfo> GetMemberInfoForAESOB(MemberGender? genderFilter)
		{
            string connectionString = "Data Source=37.77.4.71\\SQLEXPRESS;Initial Catalog=sicil;User ID=kursad;Password=Asperox123.";
            //string sqlQuery = $"SELECT CEPTEL FROM SICIL WHERE CEPTEL IS NOT NULL";

            string genderFilterQuery = "";
            if (genderFilter != null && genderFilter > MemberGender.All)
            {
                genderFilterQuery = $"AND _sicil.CINSIYET = {((int)genderFilter).ToString()}";
            }

            string sqlQuery = $"SELECT " +
                $"_sicil.SICILNO, " +
                $"_sicil.ADSOYAD, " +
                $"_sicil.CEPTEL, " +
                $"_sicil.TCKIMLIKNO " +
                $"FROM SICILMESLEK _sicilMeslek " +
                $"LEFT JOIN SICIL _sicil ON _sicil.ID = _sicilMeslek.SICILID " +
                $"WHERE (_sicilMeslek.MESLEKTERKTAR IS NULL OR _sicilMeslek.MESLEKTERKTAR = NULL) AND _sicil.CEPTEL IS NOT NULL " +
                genderFilterQuery +
                $"GROUP BY _sicil.SICILNO, _sicil.ADSOYAD, _sicil.CEPTEL, _sicil.TCKIMLIKNO " +
                $"ORDER BY _sicil.SICILNO ASC ";

            List<MemberInfo> membersList = new List<MemberInfo>();
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
							MemberInfo memberInfo = new MemberInfo()
							{
								SicilNo = reader.GetInt32(0),
								AdSoyad = reader.GetString(1),
								TelefonNumarasi = reader.GetString(2),
								TCKimlikNo = reader.GetString(3),
							};

                            if (memberInfo.SicilNo >= 0
								&& !string.IsNullOrEmpty(memberInfo.AdSoyad)
								&& !string.IsNullOrEmpty(memberInfo.TelefonNumarasi)
								&& !string.IsNullOrEmpty(memberInfo.TCKimlikNo))
                            {
                                membersList.Add(memberInfo);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    return membersList.Distinct().ToList();
                }
            }
        }

		public static List<string> GetPhoneNumbersForAESOB(MemberGender? genderFilter)
		{
			string connectionString = "Data Source=37.77.4.71\\SQLEXPRESS;Initial Catalog=sicil;User ID=kursad;Password=Asperox123.";
			//string sqlQuery = $"SELECT CEPTEL FROM SICIL WHERE CEPTEL IS NOT NULL";

			string genderFilterQuery = "";
			if(genderFilter != null && genderFilter > MemberGender.All)
			{
				genderFilterQuery = $"AND _sicil.CINSIYET = {((int)genderFilter).ToString()}";
            }

			string sqlQuery = $"SELECT " +
				$"_sicil.SICILNO, " +
				$"_sicil.ADSOYAD, " +
				$"_sicil.CEPTEL, " +
				$"_sicil.TCKIMLIKNO " +
				$"FROM SICILMESLEK _sicilMeslek " +
				$"LEFT JOIN SICIL _sicil ON _sicil.ID = _sicilMeslek.SICILID " +
				$"WHERE (_sicilMeslek.MESLEKTERKTAR IS NULL OR _sicilMeslek.MESLEKTERKTAR = NULL) AND _sicil.CEPTEL IS NOT NULL " +
                genderFilterQuery +
				$"GROUP BY _sicil.SICILNO, _sicil.ADSOYAD, _sicil.CEPTEL, _sicil.TCKIMLIKNO " +
				$"ORDER BY _sicil.SICILNO ASC ";

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
							string phoneNumber = reader.GetString(3);
							if (phoneNumber != null)
							{
								phoneNumbersList.Add(phoneNumber);
							}
						}
						catch (Exception)
						{
						}
					}

					return phoneNumbersList.Distinct().ToList();
				}
			}
		}
	}
}
