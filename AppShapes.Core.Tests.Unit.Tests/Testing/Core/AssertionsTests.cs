using System;
using System.Collections.Generic;
using AppShapes.Core.Testing.Core;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Core
{
    public class AssertionsTests
    {
        [Fact]
        public void ContainsMustReturnExpectedValueWhenCollectionContainsExpectedValue()
        {
            Assert.Equal("Bob", Assertions.Contains("42", new Dictionary<string, string> {{"41", "Alice"}, {"42", "Bob"}}));
        }

        [Fact]
        public void ContainsMustThrowExceptionWhenCollectionDoesNotContainExpectedValue()
        {
            Assert.Equal("Collection does not contain expected value.", Assert.Throws<ArgumentException>(() => Assertions.Contains("42", new Dictionary<string, string> {{"41", "Alice"}})).Message);
        }

        [Fact]
        public void NotEqualMustReturnActualWhenExpectedDoesNotEqualActual()
        {
            Assert.Equal("Alice", Assertions.NotEqual("Bob", "Alice"));
        }

        [Fact]
        public void NotEqualMustThrowExceptionWhenExpectedEqualsActual()
        {
            Assert.Equal("Bob equals Bob", Assert.Throws<ArgumentException>(() => Assertions.NotEqual("Bob", "Bob")).Message);
        }

        [Fact]
        public void NotNUllMustReturnActualWhenActualIsNotNull()
        {
            Assert.Equal("Alice", Assertions.NotNull("Alice"));
        }

        [Fact]
        public void NotNullMustThrowExceptionWhenActualIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'actual')", Assert.Throws<ArgumentNullException>(() => Assertions.NotNull(null)).Message);
        }

        [Fact]
        public void TrueMustNotThrowExceptionWhenConditionIsTrue()
        {
            Assertions.True(true, null);
        }

        [Fact]
        public void TrueMustThrowExceptionWhenConditionIsFalse()
        {
            Assert.Equal("42", Assert.Throws<InvalidOperationException>(() => Assertions.True(false, "42")).Message);
        }
    }
}