using System;
using System.Globalization;
using System.Linq;

namespace aesob.org.tr.Utilities
{
    public static class CompatibilityHelper
    {
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

        public static string ConvertTextToLocalized(string text)
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

            var stringDatePart = dateString?.Split(' ').FirstOrDefault();

            if (string.IsNullOrEmpty(stringDatePart))
            {
                return result;
            }

            try
            {
                result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture);
            }
            catch
            {
                try
                {
                    result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
                }
                catch
                {
                    try
                    {
                        result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                    }
                    catch
                    {
                        try
                        {
                            result = DateTime.Parse(stringDatePart, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                        }
                        catch
                        {
                            try
                            {
                                result = DateTime.ParseExact(stringDatePart, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                try
                                {
                                    result = DateTime.ParseExact(stringDatePart, "MM.dd.yyyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    try
                                    {
                                        result = DateTime.ParseExact(stringDatePart, "MM\\dd\\yyyy", CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            result = DateTime.ParseExact(stringDatePart, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        catch
                                        {
                                            try
                                            {
                                                result = DateTime.ParseExact(stringDatePart, "dd\\MM\\yyyy", CultureInfo.InvariantCulture);
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    result = DateTime.ParseExact(stringDatePart, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                }
                                                catch
                                                {
                                                    try
                                                    {
                                                        result = DateTime.ParseExact(stringDatePart, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                                    }
                                                    catch
                                                    {
                                                        try
                                                        {
                                                            result = DateTime.ParseExact(stringDatePart, "yyyy-dd-MM", CultureInfo.InvariantCulture);
                                                        }
                                                        catch
                                                        {
                                                            //blank intentionally
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
            return result;
        }
    }
}
