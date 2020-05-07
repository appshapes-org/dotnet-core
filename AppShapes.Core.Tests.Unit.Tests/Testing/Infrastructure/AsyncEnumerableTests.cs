using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using AppShapes.Core.Testing.Infrastructure;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class AsyncEnumerableTests
    {
        [Fact]
        public async void GetAsyncEnumeratorMustReturnAsyncEnumeratorWhenCancellationTokenIsNotSpecified()
        {
            IAsyncEnumerator<int> enumerator = new AsyncEnumerable<int>(Expression.Constant(new[] {42})).GetAsyncEnumerator();
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(42, enumerator.Current);
        }

        [Fact]
        public async void GetAsyncEnumeratorMustReturnAsyncEnumeratorWhenCancellationTokenIsSpecified()
        {
            IAsyncEnumerator<int> enumerator = new AsyncEnumerable<int>(Expression.Constant(new[] {42})).GetAsyncEnumerator(CancellationToken.None);
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(42, enumerator.Current);
        }
    }
}