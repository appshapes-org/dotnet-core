using AppShapes.Core.Service;
using Xunit;

namespace AppShapes.Core.Tests.Unit.Tests.Service
{
    public class ApiControllerBaseTests
    {
        [Fact]
        public void ConstructorMustReturnInstanceOfApiControllerBaseWhenCalled()
        {
            Assert.IsAssignableFrom<ApiControllerBase>(new FakeController());
        }

        private class FakeController : ApiControllerBase
        {
        }
    }
}