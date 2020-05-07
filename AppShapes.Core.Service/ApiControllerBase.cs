using Microsoft.AspNetCore.Mvc;

namespace AppShapes.Core.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
    }
}