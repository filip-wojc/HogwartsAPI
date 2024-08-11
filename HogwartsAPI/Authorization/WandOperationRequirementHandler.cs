using HogwartsAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using HogwartsAPI.Enums;
using System.Security.Claims;

namespace HogwartsAPI.Authorization
{
    public class WandOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Wand>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, Wand wand)
        {
            if (requirement.ResourceOperation == ResourceOperation.Read ||
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (wand.CreatedById == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;

            // To Samo dla innych encji
            // Zmiana w serwisach
        }
    }
}
