using Microsoft.AspNetCore.Builder;

namespace AppShapes.Core.Service
{
    public class UseProductionControllersCommand : UseControllersCommand
    {
        public override void Execute(IApplicationBuilder app, string policyName)
        {
            app.UseHsts();
            base.Execute(app, policyName);
        }
    }
}