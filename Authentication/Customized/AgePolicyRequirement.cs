using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Authentication.Customized
{
    public class AgePolicyRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; set; }
        public AgePolicyRequirement(int minimumage)
        {
            MinimumAge = minimumage;
        }
    }


    public class AgePolicyRequirementHandler : AuthorizationHandler<AgePolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AgePolicyRequirement requirement)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                var ageClaim = context.User.Claims.FirstOrDefault(claim => claim.Type == "Age");
                if (ageClaim != null && int.TryParse(ageClaim.Value,out int age))
                {
                    if(age >= requirement.MinimumAge)
                    {
                        context.Succeed(requirement);
                    }
                }


            }
            return Task.CompletedTask;
        }
    }
}
