using System;
using System.Collections.Generic;
using System.Linq;

namespace WeatherService.Web.Utils
{
	public static class LimitedCapacityCollectionHelper
	{
		public static IEnumerable<string> Add(IEnumerable<string> source, string item, int maxCapacity)
		{
			var shiftIndex = -1;
			var recentCitiesCount = source.Count();
			if (recentCitiesCount >= maxCapacity)
			{
				shiftIndex = 0;
			}
			var sourceArray = source.ToArray();
			if (source.Contains(item))
			{
				shiftIndex = Array.IndexOf(sourceArray, item);
			}
			if (shiftIndex >= 0)
			{
				for (var i = shiftIndex; i < recentCitiesCount - 1; i++)
				{
					sourceArray[i] = sourceArray[i + 1];
				}
				sourceArray[recentCitiesCount - 1] = null;
			}
			var result = sourceArray.Where(p => !string.IsNullOrEmpty(p)).ToList();
			result.Add(item);
			return result;
		}
	}
}