﻿using GroomerApi.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GroomerApi.Authorization
{
    public class ResourceOperationReirementHandler : AuthorizationHandler<ResourceOperationRequirement, User>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, User user)
        {
            if(requirement.ResourceOperation == ResourceOperation.Read || 
                requirement.ResourceOperation == ResourceOperation.Create)
            {
                context.Succeed(requirement);
            }

            var userId = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if(user.Id == int.Parse(userId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
