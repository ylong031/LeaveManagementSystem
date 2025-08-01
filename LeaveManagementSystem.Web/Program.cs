/*Dependency injection is a core concept in ASP.NET Core.
It allows you to request (or "inject") dependencies (services) into classes instead of creating them manually.*/


using LeaveManagementSystem.Web.Services.Email;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.EntityFrameworkCore;
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

/*AddScoped: One instance per HTTP request, reused throughout that request (like one waiter per meal).*/
builder.Services.AddScoped<ILeaveTypesService, LeaveTypesService>();

builder.Services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>();

builder.Services.AddScoped<ILeaveRequestsService, LeaveRequestsService>();

builder.Services.AddScoped<IPeriodsService, PeriodsService>();

builder.Services.AddScoped<IUserService, UserService>();



/*
When you add services with .AddTransient<IEmailSender, EmailSender>(), you are telling the DI container:
"Whenever someone asks for IEmailSender, create a new instance of EmailSender and give them that."
*/


/*The ResendEmailConfirmationModel class also has a constructor that takes an IEmailSender parameter and
stores it in a private field (_emailSender).
It uses _emailSender.SendEmailAsync(...) when resending the confirmation email.
These scripts request IEmailSender via constructor injection,
which is why you need to register your concrete implementation (EmailSender) in the dependency injection container in Program.cs.
This allows ASP.NET Core to automatically provide your EmailSender class whenever IEmailSender is needed.*/

/*AddTransient means a new instance of EmailSender will be created each time it's requested.
  (like a new waiter for every single request during the meal).*/

builder.Services.AddTransient<IEmailSender, EmailSender>();



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
