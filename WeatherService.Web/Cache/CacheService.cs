using System;
using System.Runtime.Caching;

namespace WeatherService.Web.Cache
{
	public class CacheService : ICacheService
	{
		private const int CacheDuration = 60;
		
		public void Save(string key, object value)
		{
			if (value != null)
			{
				MemoryCache.Default.Add(key, value, DateTime.Now.AddMinutes(CacheDuration));
			}
		}

		public T Get<T>(string key) where T : class
		{
			return MemoryCache.Default.Get(key) as T;
		}
	}
}