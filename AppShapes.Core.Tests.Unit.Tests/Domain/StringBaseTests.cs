using AppShapes.Core.Domain;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Domain
{
    public class StringBaseTests
    {
        [Fact]
        public void CompareToMustReturnGreaterThanZeroWhenComparedToLessThanValue()
        {
            Assert.True(new NormalString("GHI").CompareTo(new NormalString("DEF")) > 0);
        }

        [Fact]
        public void CompareToMustReturnGreaterThanZeroWhenComparedToLessThanValueAndCaseSensitivityIsFalse()
        {
            Assert.True(new NormalString("GHI", true, false).CompareTo(new NormalString("DEF", true, false)) > 0);
        }

        [Fact]
        public void CompareToMustReturnLessThanZeroWhenComparedToGreaterThanValue()
        {
            Assert.True(new NormalString("ABC").CompareTo(new NormalString("DEF")) < 0);
        }

        [Fact]
        public void CompareToMustReturnNegativeOneWhenNotComparedToStringBase()
        {
            Assert.Equal(-1, new NormalString("test").CompareTo(new object()));
        }

        [Fact]
        public void ConstructorMustNotTrimValueWhenShouldTrimIsFalse()
        {
            Assert.Equal("12345 ", new NormalString("12345 ", false).Value);
        }

        [Fact]
        public void ConstructorMustTrimValueWhenShouldTrimIsTrue()
        {
            Assert.Equal("12345", new NormalString("12345 ").Value);
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValueIsNotStringBase()
        {
            Assert.False(new NormalString("test").Equals(new object()));
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValueIsNull()
        {
            Assert.False(new NormalString("test").Equals(null));
        }

        [Fact]
        public void EqualsMustReturnFalseWhenValuesAreNotEqual()
        {
            Assert.NotEqual(new NormalString("test"), new NormalString("not test"));
        }

        [Fact]
        public void EqualsMustReturnTrueWhenValuesAreEqual()
        {
            Assert.True(new NormalString("test").Equals(new NormalString("test")));
        }

        [Fact]
        public void EqualsMustReturnTrueWhenValuesAreEqualCaseInsensitive()
        {
            Assert.True(new NormalString("Test", true, false).Equals(new NormalString("test", true, false)));
        }

        [Fact]
        public void GetHashCodeMustReturnSameValueWhenValuesAreEqual()
        {
            Assert.Equal(new NormalString("test").GetHashCode(), new NormalString("test").GetHashCode());
        }

        [Fact]
        public void GetHashCodeMustReturnSameValueWhenValuesAreEqualCaseInsensitive()
        {
            Assert.Equal(new NormalString("test", true, false).GetHashCode(), new NormalString("test", true, false).GetHashCode());
        }

        private class NormalString : StringBase
        {
            public NormalString(string value, bool shouldTrim = true, bool isCaseSensitive = true) : base(value, shouldTrim, isCaseSensitive)
            {
            }
        }
    }
}