using System.Security.Claims;
using WorkRecord.Domain.Models;
using WorkRecord.Infrastructure.DataAccess.Interfaces;

namespace WorkRecord.API
{
    public class AuthorizationManager
    {
        public WebApplicationBuilder ConfigureAuthorization(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        if (context.User.FindFirstValue(ClaimTypes.Role) is null)
                        {
                            return false;
                        }
                        Role role = (Role)int.Parse(context.User.FindFirstValue(ClaimTypes.Role)!);
                        return role is Role.admin;
                    });
                });

                options.AddPolicy("Manager", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        if (context.User.FindFirstValue(ClaimTypes.Role) is null)
                        {
                            return false;
                        }
                        Role role = (Role)int.Parse(context.User.FindFirstValue(ClaimTypes.Role)!);
                        return role is Role.admin || role is Role.manager;
                    });
                });

                options.AddPolicy("Coordinator", options =>
                {
                    options.RequireAssertion(context =>
                    {
                        if (context.User.FindFirstValue(ClaimTypes.Role) is null)
                        {
                            return false;
                        }
                        Role role = (Role)int.Parse(context.User.FindFirstValue(ClaimTypes.Role)!);
                        return role is Role.admin || role is Role.manager || role is Role.coordinator;
                    });
                });
            });
            return builder;
        }
    }
}
