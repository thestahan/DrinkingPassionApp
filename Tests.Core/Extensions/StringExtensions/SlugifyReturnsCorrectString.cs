using Core.Extensions;
using NUnit.Framework;

namespace Tests.Core.Extensions.StringExtensions
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
