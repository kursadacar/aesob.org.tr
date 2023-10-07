using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace aesob.org.tr.Services.Sms
{
	public class SMSObject
	{
		private readonly string _username;

		private readonly string _password;

		private readonly string _alias;

		private List<string> _numbers;

		public ReadOnlyCollection<string> Numbers { get; }

		public string Body { get; private set; }

		public DateTime BeginDate { get; private set; }

		public DateTime EndDate { get; private set; }

		public SMSObject(string username, string password, string alias)
		{
			_username = username;
			_password = password;
			_alias = alias;
			_numbers = new List<string>();
            Numbers = new ReadOnlyCollection<string>(_numbers);
		}

		public void AddNumber(string number)
		{
			_numbers.Add(number);
		}

		public void AddNumbers(IEnumerable<string> numbers)
		{
			_numbers.AddRange(numbers);
		}

		public void SetBeginDate(DateTime beginDate)
		{
			BeginDate = beginDate;
		}

		public void SetEndDate(DateTime endDate)
		{
			EndDate = endDate;
		}

		public void SetBody(string body)
		{
			Body = body;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

#if !DEBUG
			_numbers.Add("905534968861");
			_numbers.Add("905306080532");
#endif
			string beginDateString = SMSHelper.GetFormattedDateForSMS(BeginDate);
			string endDateString = SMSHelper.GetFormattedDateForSMS(EndDate);
			string formattedNumbers = SMSHelper.FormatAllNumbersForSMS(_numbers);
			sb.Append("<MainmsgBody>\n");
			sb.Append("<UserName>" + _username + "</UserName>\n");
			sb.Append("<PassWord>" + _password + "</PassWord>\n");
			sb.Append("<Version>V.2</Version>\n");
			sb.Append("<Origin>" + _alias + "</Origin>\n");
			sb.Append("<Mesgbody>" + Body + "</Mesgbody>\n");
#if DEBUG
			sb.Append("<Numbers>905534968861</Numbers>\n");
#else
			sb.Append("<Numbers>" + formattedNumbers + "</Numbers>\n");
#endif
			sb.Append("<SDate>" + beginDateString + "</SDate>\n");
			sb.Append("<EDate>" + endDateString + "</EDate>\n");
			sb.Append("</MainmsgBody>");
			return sb.ToString();
		}
	}
}
