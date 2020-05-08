using System;
using AppShapes.Core.Domain;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Domain
{
    public class NonEmptyStringTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void ConstructorMustThrowExceptionWhenValueIsNullOrWhitespace(string value)
        {
            Assert.Throws<ArgumentNullException>(() => new NonEmptyString(value));
        }

        [Fact]
        public void ImplicitOperatorMustReturnNonEmptyStringWhenStringCastToNonEmptyString()
        {
            Assert.IsType<NonEmptyString>((NonEmptyString) "test");
        }

        [Fact]
        public void ImplicitOperatorMustReturnNullWhenNonEmptyStringIsNull()
        {
            Assert.Null((string) (NonEmptyString) null);
        }

        [Fact]
        public void ImplicitOperatorMustReturnNullWhenValueIsNull()
        {
            Assert.Null((string) (StringBase) null);
        }

        [Fact]
        public void ImplicitOperatorMustReturnStringWhenNonEmptyStringCastToString()
        {
            Assert.IsType<string>((string) new NonEmptyString("test"));
        }

        [Fact]
        public void ImplicitOperatorMustReturnValueWhenValueIsNotNull()
        {
            Assert.Equal("test", (StringBase) new NonEmptyString("test"));
        }
    }
}