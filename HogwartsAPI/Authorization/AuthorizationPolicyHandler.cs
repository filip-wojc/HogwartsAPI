using HogwartsAPI.Entities;
using HogwartsAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HogwartsAPI.Authorization
{
    public class AuthorizationPolicyHandler : IAuthorizationPolicy
    {
        public void AddAuthorizationPolicy(IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<Wand>>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<Pet>>();
            services.AddScoped<IAuthorizationHandler, ResourceOperationRequirementHandler<Course>>();
        }
    }
}
