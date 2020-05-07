using System;
using System.Collections.Generic;
using AppShapes.Core.Testing.Infrastructure;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class AsyncEnumeratorTests
    {
        [Fact]
        public void ConstructorMustThrowExceptionWhenEnumeratorIsNull()
        {
            Assert.Equal("Value cannot be null. (Parameter 'enumerator')", Assert.Throws<ArgumentNullException>(() => new AsyncEnumerator<int>(null)).Message);
        }

        [Fact]
        public void DisposeAsyncMustDisposeAsyncEnumeratorWhenCalled()
        {
            AsyncEnumerator<int> enumerator = new AsyncEnumerator<int>(new List<int> {42}.GetEnumerator());
            Assert.True(enumerator.DisposeAsync().IsCompletedSuccessfully);
        }

        [Fact]
        public async void MoveNextAsyncMustReturnTrueWhenEnumeratorMovesNext()
        {
            AsyncEnumerator<int> enumerator = new AsyncEnumerator<int>(new List<int> {42}.GetEnumerator());
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(42, enumerator.Current);
        }
    }
}