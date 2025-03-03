using DrinkingPassion.Shared.Extensions;
using NUnit.Framework;

namespace DrinkingPassion.Api.Shared.Tests.Extensions.StringExtensions
{
    [TestFixture]
    public class RemoveAccentReturnsCorrectString
    {
        [Test]
        public void PolishDiacriticsAreReplaced()
        {
            // Arrange
            var sentence = "ą, ć, ę, ł, ń, ó, ś, ź, ż";

            // Act
            var sentenceWithRemovedAccents = sentence.RemoveAccents();

            // Assert
            Assert.That(sentenceWithRemovedAccents, Is.EquivalentTo("a, c, e, l, n, o, s, z, z"));
        }

        [Test]
        public void EmptyStringReturnsEmptyString()
        {
            // Arrange
            var sentence = "";

            // Act
            var sentenceWithRemovedAccents = sentence.RemoveAccents();

            // Assert
            Assert.That(sentenceWithRemovedAccents, Is.EquivalentTo(""));
        }
    }
}
