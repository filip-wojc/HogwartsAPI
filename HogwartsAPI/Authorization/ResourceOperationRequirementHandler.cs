using HogwartsAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using HogwartsAPI.Enums;
using System.Security.Claims;
using HogwartsAPI.Interfaces;

namespace HogwartsAPI.Authorization
{
    public class ResourceOperationRequirementHandler<T> : AuthorizationHandler<ResourceOperationRequirement, T> where T : IResource
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, T resource)
        {
            var userRole = context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value;

            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create ||
                userRole == "Admin")
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (resource.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;


        }
    }
}
                                