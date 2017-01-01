using System;
using FluentAssertions;
using NUnit.Framework;

namespace WeatherService.Provider.Tests
{
	[TestFixture]
	public class Utils_Tests
	{
		[TestCase(11, "North")]
		[TestCase(33, "Northnortheast")]
		[TestCase(34, "Northeast")]
		[TestCase(57, "Eastnortheast")]
		[TestCase(79, "East")]
		[TestCase(102, "Eastsoutheast")]
		[TestCase(125, "Southeast")]
		[TestCase(147, "Southsoutheast")]
		[TestCase(180, "South")]
		[TestCase(203, "Southsouthwest")]
		[TestCase(225, "Southwest")]
		[TestCase(248, "Westsouthwest")]
		[TestCase(270, "West")]
		[TestCase(293, "Westnorthwest")]
		[TestCase(315, "Northwest")]
		[TestCase(337, "Northnorthwest")]
		[TestCase(720, "North")]
		public void DegreeToDirection_IfCalled_ReturnsExpectedResult(int degree, string direction)
		{
			var actualDirection = Utils.DegreeToDirection(degree);
			actualDirection.ShouldBeEquivalentTo(direction);
		}

		[Test]
		public void TimestampToDateTime_IfCalled_ReturnsExpectedResult()
		{
			var expectedDateTime = new DateTime(2017, 1, 1);
			var actualDateTime = Utils.TimestampToDateTime(1483228800);
			actualDateTime.ShouldBeEquivalentTo(expectedDateTime);
		}

		[Test]
		public void TimestampToDateTime_IfCalledWithInitialValue_ReturnsExpectedResult()
		{
			var expectedDateTime = new DateTime(1970, 1, 1);
			var actualDateTime = Utils.TimestampToDateTime(0);
			actualDateTime.ShouldBeEquivalentTo(expectedDateTime);
		}

		[TestCase(666, "Hurricane force")]
		[TestCase(29, "Violent storm")]
		[TestCase(25, "Storm")]
		[TestCase(21, "Strong gale")]
		[TestCase(18, "Gale")]
		[TestCase(14, "High wind")]
		[TestCase(11, "Strong breeze")]
		[TestCase(8.1, "Fresh breeze")]
		[TestCase(6, "Moderate breeze")]
		[TestCase(4, "Gentle breeze")]
		[TestCase(2, "Light breeze")]
		[TestCase(0.5, "Light air")]
		[TestCase(0.1, "Calm")]
		public void WindSpeedToDescription_IfCalled_ReturnsExpectedValue(double speed, string output)
		{
			var actualOutput = Utils.WindSpeedToDescription(speed);
			actualOutput.ShouldBeEquivalentTo(output);
		}
	}
}
