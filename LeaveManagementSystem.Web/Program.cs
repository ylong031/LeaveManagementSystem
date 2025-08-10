/*Dependency injection is a core concept in ASP.NET Core.
It allows you to request (or "inject") dependencies (services) into classes instead of creating them manually.*/


using LeaveManagementSystem.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;


var builder = WebApplication.CreateBuilder(args);









// Add authorization policies
// only users with the Administrator or Supervisor roles can access the AdminSupervisorOnly policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy =>
    {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor);
    }
    );

});





/*
IHttpContextAccessor provides access to the current HttpContext (which contains information about the HTTP request, user, session, etc.)
from places where it isn’t directly available (e.g., background services or repositories).
*/
builder.Services.AddHttpContextAccessor();

// Add data services
DataServicesRegistration.AddDataServices(builder.Services,builder.Configuration);

// Add application services
ApplicationServicesRegistration.AddApplicationServices(builder.Services);

/*
builder.Host.UseSerilog(...) sets up Serilog as the logging provider for the ASP.NET Core application.
The lambda receives ctx (context of building up the application) and config (Serilog logger configuration).

config.WriteTo.Console() configures Serilog to output logs to the console.

.ReadFrom.Configuration(ctx.Configuration) reads further Serilog configuration
from the app's configuration files (such as appsettings.json).
*/
builder.Host.UseSerilog((ctx, config) =>

    config.WriteTo.Console()
          .ReadFrom.Configuration(ctx.Configuration)
        
);


//SignIn.RequireConfirmedAccount = true
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    // Minimum password length
    options.Password.RequiredLength = 8;

    // Allow passwords without special characters
    options.Password.RequireNonAlphanumeric=false; 

})
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
