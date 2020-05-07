using Microsoft.AspNetCore.Builder;

namespace AppShapes.Core.Service
{
    public class UseControllersCommand
    {
        public virtual void Execute(IApplicationBuilder app, string policyName)
        {
            app.UseResponseCaching();
            app.UseRouting();
            app.UseCors(policyName);
        }
    }
}