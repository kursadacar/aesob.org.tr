using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace aesob.org.tr.Utilities
{
	public static class Common
	{
		public static IWebHostEnvironment GetWebHostEnviroment()
		{
			HttpContextAccessor _accessor = new HttpContextAccessor();
			return _accessor.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
		}
	}
}
