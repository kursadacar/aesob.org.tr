using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace aesob.org.tr.Services.Sms
{
	public class SMSObject
	{
		private List<string> _numbers;

		public ReadOnlyCollection<string> Numbers { get; }

		public string Body { get; private set; }

		public DateTime BeginDate { get; private set; }

		public DateTime EndDate { get; private set; }

		public SMSObject()
		{
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
	}
}
