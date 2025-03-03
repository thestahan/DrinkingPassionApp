using DrinkingPassion.Shared.Extensions;
using NUnit.Framework;

namespace DrinkingPassion.Api.Shared.Tests.Extensions.StringExtensions
{
    [TestFixture]
    public class SlugifyReturnsCorrectString
    {
        [Test]
        [TestCase("Łabądź w jeziorze")]
        [TestCase("  Łabądź  w  jeziorze  ")]
        [TestCase(",,ŁabąDŹ w Jeziorze,")]
        public void SentenceReturnsCorrectSlug(string sentence)
        {
            // Act
            var slug = sentence.Slugify();

            // Assert
            Assert.That(slug, Is.EquivalentTo("labadz-w-jeziorze"));
        }
    }
}
