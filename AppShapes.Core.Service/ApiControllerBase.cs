using Microsoft.AspNetCore.Mvc;

namespace AppShapes.Core.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected virtual string GetRouteUrl(string routeName, object routeValues)
        {
            return Url.RouteUrl(routeName, routeValues);
        }
    }
}