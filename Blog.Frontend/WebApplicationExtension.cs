using Blog.Frontend.Data;
using Microsoft.AspNetCore.Identity;

namespace Blog.Frontend;

public static class WebApplicationExtension
{
    public static Task SeedDatabase(this WebApplication webApplication)
    {
        try
        {
            var scope = webApplication.Services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            ctx.Database.EnsureCreated();
            var adminRole = new IdentityRole("Admin");
            if (!ctx.Roles.Any())
            {
                roleManager.CreateAsync(adminRole).ConfigureAwait(false).GetAwaiter().GetResult();
                //create a role
            }

            var adminUser = new IdentityUser()
            {
                UserName = "admin",
                Email = "admin@test.com"
            };
            if (!ctx.Users.Any(u => u.UserName == "admin"))
            {
                //create an admin
                var result = userManager
                    .CreateAsync(adminUser, "test")
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                userManager.AddToRoleAsync(adminUser, adminRole.Name).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return Task.CompletedTask;

    }
}