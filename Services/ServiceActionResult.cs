using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;

namespace aesob.org.tr.Services
{
	public class ServiceActionResult : JsonResult
	{
		public enum ActionResult
		{
			Success,
			SuccessWithWarning,
			Fail
		}

		public ActionResult Result { get; private set; }
		public string Message { get; private set; }

		private ServiceActionResult(ActionResult result, string message)
			: base(null)
		{
			Result = result;
			Message = message;

			Value = new
			{
				Result = result.ToString(),
				Message = message
			};
		}

		public static ServiceActionResult CreateSuccess(string message = null)
		{
			return new ServiceActionResult(ActionResult.Success, message ?? string.Empty);
		}

		public static ServiceActionResult CreateFail(string message = null)
		{
			return new ServiceActionResult(ActionResult.Fail, message ?? string.Empty);
		}

		public static ServiceActionResult CreateSuccessWithWarning(string message = null)
		{
			return new ServiceActionResult(ActionResult.SuccessWithWarning, message ?? string.Empty);
		}
	}
}
