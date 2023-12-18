using Blog.Frontend;
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

WebApplication app = builder.Build();

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
app.SeedDatabase();

app.Run();