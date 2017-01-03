using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using WeatherService.Web.Utils;

namespace WeatherService.Web.Tests
{
	[TestFixture]
	public class LimitedCapacityCollectionHelper_Tests
	{
		[Test]
		public void Add_ItemDoesnotExist_ReturnsExpectedValue()
		{
			var collection = new[] {"kek"};
			collection = LimitedCapacityCollectionHelper.Add(collection, "lel", 3).ToArray();
			collection.ShouldAllBeEquivalentTo(new [] {"kek", "lel"});
		}

		[Test]
		public void Add_OnlyItemExists_ReturnsExpectedValue()
		{
			var collection = new[] { "kek" };
			collection = LimitedCapacityCollectionHelper.Add(collection, "kek", 3).ToArray();
			collection.ShouldAllBeEquivalentTo(new[] { "kek"});
		}

		[Test]
		public void Add_ItemExists_ReturnsExpectedValue()
		{
			var collection = new[] { "kek", "lul" };
			collection = LimitedCapacityCollectionHelper.Add(collection, "kek", 3).ToArray();
			collection.ShouldAllBeEquivalentTo(new[] { "lul", "kek" });
		}

		[Test]
		public void Add_CapacityExceeded_ReturnsExpectedValue()
		{
			var collection = new[] { "kek", "lul" };
			collection = LimitedCapacityCollectionHelper.Add(collection, "lox", 2).ToArray();
			collection.ShouldAllBeEquivalentTo(new[] { "lul", "lox" });
		}
	}
}
