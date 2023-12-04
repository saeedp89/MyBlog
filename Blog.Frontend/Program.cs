using Blog.Frontend.Data;
using Blog.Frontend.Data.FileManager;
using Blog.Frontend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddControllersWithViews(o => { o.EnableEndpointRouting = false; });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["DefaultConnection"]));

builder.Services.AddTransient<IRepository, Repository>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
    })
    //.AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(o => { o.LoginPath = "/Auth/Login"; });

builder.Services.AddTransient<IFileManager, FileManager>();

var app = builder.Build();
app.UseDeveloperExceptionPage();
// app.UseMvcWithDefaultRoute();
app.UseStaticFiles();
app.UseAuthentication();
app.UseMvc(routeBuilder =>
{
    routeBuilder.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});
await SeedDatabase(app);

app.Run();

async Task SeedDatabase(WebApplication webApplication)
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
            await roleManager.CreateAsync(adminRole);
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
            IdentityResult result = await userManager.CreateAsync(adminUser, "test");

            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}