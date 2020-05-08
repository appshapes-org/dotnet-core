using System;
using AppShapes.Core.Domain;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Domain
{
    public class SemanticVersionTests
    {
        [Theory]
        [InlineData("v1.0")]
        [InlineData("1.0")]
        [InlineData("1.2")]
        public void ConstructorMustNotThrowExceptionWhenVersionIsValid(string value)
        {
            Assert.Equal(value, new SemanticVersion(value).Value);
        }

        [Theory]
        [InlineData("V1.0")]
        [InlineData("1.0.0")]
        [InlineData("1")]
        public void ConstructorMustThrowExceptionWhenVersionIsNotValid(string value)
        {
            Assert.Equal($"Invalid version: {value}", Assert.Throws<ArgumentException>(() => new SemanticVersion(value)).Message);
        }
    }
}