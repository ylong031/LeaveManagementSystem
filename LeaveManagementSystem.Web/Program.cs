using Humanizer;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using System.ComponentModel;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


/*This retrieves the connection string named DefaultConnection from your appsettings.json.
If it’s missing or null, it throws a clear error.*/
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

/*Tell ASP.NET Core: "I want to use a database context called ApplicationDbContext in my app."*/
/*When setting it up, use SQL Server as the database, and connect to it using the connectionString variable.*/


/*In this example, the options object inside the lambda is a DbContextOptionsBuilder,
which configures things like the connection string and provider.
This configuration is then used to create the DbContextOptions that get passed to your ApplicationDbContext.*/

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddScoped<ILeaveTypesService, LeaveTypesService>();
/*So now this service is registered in the IoC container and
it is usable by other classes that are also registered in the container.
There are some limitations, but generally speaking, anywhere I need the service,
I can just inject it the same way I injected the mapper and the context and so on.*/

/*IoC
 * Instead of you creating and managing dependencies manually,
 * you delegate that responsibility to a framework or container 
 * � in this case, the IoC container.
*/

builder.Services.AddTransient<IEmailSender, EmailSender>();



builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
/*
it tells Asp.net core dependency injection system to register automapper
 and automatically scan the current project for mapping profiles
it will find the profile reads the mapping configuration 
and make it availablea app-wide
 */

//SignIn.RequireConfirmedAccount = true
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // This adds the IdentityRole service
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
