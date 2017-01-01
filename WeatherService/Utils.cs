using System;
using System.Collections.Generic;

namespace WeatherService.Provider
{
	public static class Utils
	{
		public static string DegreeToDirection(int degree)
		{
			var directions = new[]
			{
				"North", 
				"Northnortheast", 
				"Northeast", 
				"Eastnortheast", 
				"East", 
				"Eastsoutheast", 
				"Southeast", 
				"Southsoutheast", 
				"South", 
				"Southsouthwest", 
				"Southwest", 
				"Westsouthwest", 
				"West", 
				"Westnorthwest", 
				"Northwest", 
				"Northnorthwest"
			};
			var index = (int)((degree / 22.5 + 0.5) % 16);
			return directions[index];
		}

		public static DateTime TimestampToDateTime(int timestamp)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
				.AddSeconds(timestamp);
		}

		public static string WindSpeedToDescription(double speed)
		{
			var beafortScale = new Dictionary<double, string>()
			{
				{32.7, "Hurricane force"},
				{28.5, "Violent storm"},
				{24.5, "Storm"},
				{20.8, "Strong gale"},
				{17.2, "Gale"},
				{13.9, "High wind"},
				{10.8, "Strong breeze"},
				{8, "Fresh breeze"},
				{5.5, "Moderate breeze"},
				{3.4, "Gentle breeze"},
				{1.6, "Light breeze"},
				{0.3, "Light air"}
			};
			foreach (var item in beafortScale)
			{
				if (speed > item.Key)
				{
					return item.Value;
				}
			}
			return "Calm";
		}
	}
}
