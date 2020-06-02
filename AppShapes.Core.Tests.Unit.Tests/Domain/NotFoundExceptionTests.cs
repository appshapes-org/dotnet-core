using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AppShapes.Core.Domain.Exceptions;
using AppShapes.Core.Testing.Core;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Domain
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void ConstructorMustReturnExpectedTypeWhenCalled()
        {
            Assert.IsType<NotFoundException>(new NotFoundException());
        }

        [Fact]
        public void ConstructorMustSerializeExceptionWhenExceptionIsBeingSerialized()
        {
            NotFoundException exception = new NotFoundException("Test");
            using MemoryStream stream = new MemoryStream();
            Assert.Equal("Test", FormatterHelper.Deserialize<NotFoundException>(new BinaryFormatter(), FormatterHelper.Serialize(new BinaryFormatter(), stream, exception)).Message);
        }

        [Fact]
        public void ConstructorMustSetInnerExceptionWhenCalledWithInnerException()
        {
            Exception exception = new Exception();
            Assert.Equal(exception, new NotFoundException("Test", exception).InnerException);
        }

        [Fact]
        public void ConstructorMustSetMessageWhenCalledWithMessage()
        {
            Assert.Equal("Test", new NotFoundException("Test").Message);
        }
    }
}