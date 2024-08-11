using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HogwartsAPI.Authorization
{
    public class AuthorizationPolicyHandler : IAuthorizationPolicy
    {
        public void AddAuthorizationPolicy(IServiceCollection services)
        {
            services.AddAuthorization(o =>
            {
                o.AddPolicy("AtLeast15", b => b.AddRequirements(new MinimumAgeRequirement(15)));
            });

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, WandOperationRequirementHandler>();
        }
    }
}
