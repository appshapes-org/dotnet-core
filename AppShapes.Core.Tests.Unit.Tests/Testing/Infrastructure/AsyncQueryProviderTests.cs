using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AppShapes.Core.Testing.Infrastructure;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Testing.Infrastructure
{
    public class AsyncQueryProviderTests
    {
        [Fact]
        public async void CreateQueryMustReturnIQueryableGenericWhenCalled()
        {
            IAsyncEnumerator<int> enumerator = ((IAsyncEnumerable<int>) new AsyncQueryProvider<int>(null).CreateQuery<int>(Expression.Constant(new[] {42}))).GetAsyncEnumerator();
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(42, enumerator.Current);
        }

        [Fact]
        public async void CreateQueryMustReturnIQueryableWhenCalled()
        {
            IAsyncEnumerator<int> enumerator = ((IAsyncEnumerable<int>) new AsyncQueryProvider<int>(null).CreateQuery(Expression.Constant(new[] {42}))).GetAsyncEnumerator();
            Assert.True(await enumerator.MoveNextAsync());
            Assert.Equal(42, enumerator.Current);
        }

        [Fact]
        public void ExecuteAsyncMustReturnResultOfExpressionWhenCalled()
        {
            AsyncQueryProvider<int> provider = new AsyncQueryProvider<int>(new EnumerableQuery<int>(Expression.Empty()));
            Assert.Equal(42, provider.ExecuteAsync<Task<int>>(Expression.Constant(42)).Result);
        }

        [Fact]
        public void ExecuteGenericMustReturnResultOfExpressionWhenCalled()
        {
            AsyncQueryProvider<int> provider = new AsyncQueryProvider<int>(new EnumerableQuery<int>(Expression.Empty()));
            Assert.Equal(42, provider.Execute<int>(Expression.Constant(42)));
        }

        [Fact]
        public void ExecuteMustReturnResultOfExpressionWhenCalled()
        {
            AsyncQueryProvider<int> provider = new AsyncQueryProvider<int>(new EnumerableQuery<int>(Expression.Empty()));
            Assert.Equal(42, provider.Execute(Expression.Constant(42)));
        }
    }
}