using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WeatherService.Web.Filters
{
	public class HeaderBasedAuthorizationAttribute : Attribute, IAuthorizationFilter
	{
		private const string AuthorizationHeaderName = "Api-Key";

		private const string AuthKeySettingName = "AuthKeys";
		
		private const char AuthKeyDelimiter = ',';
		
		public bool AllowMultiple { get; private set; }

		public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
			HttpActionContext actionContext, CancellationToken cancellationToken,
			Func<Task<HttpResponseMessage>> continuation)
		{
			try
			{
				var apiKeyValues = actionContext.Request.Headers.GetValues(AuthorizationHeaderName);
				var apiKey = apiKeyValues.FirstOrDefault();
				var authKeySetting = ConfigurationManager.AppSettings[AuthKeySettingName];
				if (!string.IsNullOrEmpty(authKeySetting))
				{
					var authKeys = authKeySetting.Split(AuthKeyDelimiter).ToList();
					if (authKeys.Contains(apiKey))
					{
						return continuation();
					}
				}
				return Task.FromResult(
					actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
			}
			catch (Exception)
			{
				return Task.FromResult(
					actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
			}
			
		}
	}
}