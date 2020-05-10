using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Core
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData("\t")]
        public void WordsFromCamelCaseMustNotReturnWordsWhenValueDoesNotContainWords(string value)
        {
            string[] words = value.WordsFromCamelCase();
            Assert.Empty(words);
        }

        [Fact]
        public void WordsFromCamelCaseMustReturnExpectedWordsWhenValueContainsAcronyms()
        {
            string[] words = "JSONParser".WordsFromCamelCase();
            Assert.Equal(2, words.Length);
            Assert.Equal("JSON", words[0]);
            Assert.Equal("Parser", words[1]);
        }

        [Fact]
        public void WordsFromCamelCaseMustReturnExpectedWordsWhenValueDoesNotContainAcronyms()
        {
            string[] words = nameof(WordsFromCamelCaseMustReturnExpectedWordsWhenValueDoesNotContainAcronyms).WordsFromCamelCase();
            Assert.Equal(14, words.Length);
            Assert.Equal("From", words[1]);
            Assert.Equal("Camel", words[2]);
            Assert.Equal("Case", words[3]);
        }
    }
}