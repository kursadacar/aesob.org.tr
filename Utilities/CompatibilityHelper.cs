using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace aesob.org.tr.Utilities
{
	public static class CompatibilityHelper
	{
		private static Dictionary<char, char> _turkishCharLookup;

		public static string ConvertStringForSEO(string text)
		{
			text = (text ?? "").Trim().ToLower();
			text = text.Replace("-", "");
			text = text.Replace(" ", "-");
			text = text.Replace("ç", "c");
			text = text.Replace("ğ", "g");
			text = text.Replace("ı", "i");
			text = text.Replace("ö", "o");
			text = text.Replace("ş", "s");
			text = text.Replace("ü", "u");
			text = text.Replace("\"", "");
			text = text.Replace("/", "");
			text = text.Replace("(", "");
			text = text.Replace(")", "");
			text = text.Replace("{", "");
			text = text.Replace("}", "");
			text = text.Replace("%", "");
			text = text.Replace("&", "");
			text = text.Replace("+", "");
			text = text.Replace(".", "-");
			text = text.Replace("?", "");
			text = text.Replace(",", "");
			text = text.Replace("'", "-");
			text = text.Replace(":", "");
			while (text.Contains("--"))
			{
				text = text.Replace("--", "-");
			}
			return text;
		}

		public static string ReplaceTurkishChars(string text)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char characterToUse = text[i];
				char c;
				if (_turkishCharLookup.TryGetValue(characterToUse, out c))
				{
					characterToUse = c;
				}
				sb.Append(characterToUse);
			}
			return sb.ToString();
		}

		public static string ConvertTextToLocalized(string text)
		{
			text = Regex.Replace(text, "[^\\u0000-\\u007F]+", string.Empty);
			return text;
		}

		public static string GetCurrentDateTimeCompatible()
		{
			return DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		}

		public static string ToCulturalString(this DateTime dateTime)
		{
			return dateTime.ToString("dd/MM/yyyy");
		}

		public static DateTime GetDateTimeCompatible(string dateString)
		{
			DateTime result = DateTime.Now;
			string stringDatePart = ((dateString != null) ? dateString.Split(' ').FirstOrDefault() : null);
			if (string.IsNullOrEmpty(stringDatePart))
			{
				return result;
			}
			try
			{
				result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture);
				return result;
			}
			catch
			{
				try
				{
					result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
					return result;
				}
				catch
				{
					try
					{
						result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
						return result;
					}
					catch
					{
						try
						{
							result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
							return result;
						}
						catch
						{
							try
							{
								result = DateTime.ParseExact(stringDatePart, "dd.MM.yyyy", CultureInfo.InvariantCulture);
								return result;
							}
							catch
							{
								try
								{
									result = DateTime.ParseExact(stringDatePart, "MM.dd.yyyy", CultureInfo.InvariantCulture);
									return result;
								}
								catch
								{
									try
									{
										result = DateTime.ParseExact(stringDatePart, "MM\\dd\\yyyy", CultureInfo.InvariantCulture);
										return result;
									}
									catch
									{
										try
										{
											result = DateTime.ParseExact(stringDatePart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
											return result;
										}
										catch
										{
											try
											{
												result = DateTime.ParseExact(stringDatePart, "dd\\MM\\yyyy", CultureInfo.InvariantCulture);
												return result;
											}
											catch
											{
												try
												{
													result = DateTime.ParseExact(stringDatePart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
													return result;
												}
												catch
												{
													try
													{
														result = DateTime.ParseExact(stringDatePart, "yyyy-MM-dd", CultureInfo.InvariantCulture);
														return result;
													}
													catch
													{
														try
														{
															result = DateTime.ParseExact(stringDatePart, "yyyy-dd-MM", CultureInfo.InvariantCulture);
															return result;
														}
														catch
														{
															return result;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		static CompatibilityHelper()
		{
			Dictionary<char, char> dictionary = new Dictionary<char, char>();
			dictionary['Ğ'] = 'G';
			dictionary['Ü'] = 'U';
			dictionary['Ş'] = 'S';
			dictionary['İ'] = 'I';
			dictionary['Ö'] = 'O';
			dictionary['Ç'] = 'C';
			dictionary['ğ'] = 'g';
			dictionary['ü'] = 'u';
			dictionary['ş'] = 's';
			dictionary['ı'] = 'i';
			dictionary['ö'] = 'o';
			dictionary['ç'] = 'c';
			_turkishCharLookup = dictionary;
		}
	}
}
