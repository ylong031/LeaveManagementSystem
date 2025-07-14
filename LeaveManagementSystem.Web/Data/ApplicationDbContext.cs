/*You pass options to your context class (ApplicationDbContext), and through inheritance,
those options are ultimately received by DbContext, which is where the actual EF Core database logic lives.
This allows you to add customizations in ApplicationDbContext or IdentityDbContext if needed,
while still ensuring the underlying DbContext has all the necessary configuration.

No matter how many layers of inheritance you have (ApplicationDbContext → IdentityDbContext → DbContext),
it is the DbContext at the root that actually connects and talks to your database.*/

/*
ApplicationDbContext is a custom database context class in ASP.NET Core
that acts as a bridge between your C# code and the database,
using Entity Framework Core(EF Core).

ApplicationDbContext tells EF Core:
How to connect to the database
Which tables (entities) exist
How to query or save data.
(You use ApplicationDbContext in your services or controllers to call methods 
like .Add(), .Find(), .Update(), .Remove(), and .SaveChangesAsync() to interact with the database)
*/



/*
It connects your app to the database and sets up tables for users, roles, and leave types.
It creates three roles: Employee, Supervisor, and Administrator.
It adds a default admin user and puts them in the Administrator role.
It has a place for storing different types of leave.
In short, ApplicationDbContext helps your app keep track of users, their roles, and types of leave in the database.
*/



using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.Packaging.Signing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using NuGet.ContentModel;
using System.Reflection.Metadata;
using Microsoft.DotNet.Scaffolding.Shared;
using NuGet.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Configuration;
using System.Threading;
using System;

namespace LeaveManagementSystem.Web.Data
{
    /*By specifying<ApplicationUser>,
    you're telling Identity to use your custom user class
    (likely inheriting from ApplicationUser) instead of the default one.*/
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            /*DbContextOptions is a class in Entity Framework Core(EF Core) that holds configuration information for a DbContext.
            It tells the DbContext how to connect to the database, which database provider to use (like SQL Server, SQLite, etc.),
            and other settings such as logging, lazy loading, or command timeouts.*/

            /*
              This is the constructor for ApplicationDbContext.
            It takes DbContextOptions<ApplicationDbContext> as a parameter.
            These options include things like the connection string and database provider(e.g., SQL Server).

            base(options):This passes the options to the base class constructor (constructor of IdentityDbContext<ApplicationUser>)
            so that EF Core knows how to configure the database.
            (Base Constructor Call (Constructor Chaining))
             */
        }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<IdentityRole>().HasData(
        new IdentityRole
        {
            Id= "b15308f5-eba8-43b4-80fe-b885d542014e",
            Name = "Employee",
            NormalizedName = "EMPLOYEE"

            /*Why we use GUIDs for IDs: Always Unique
            GUIDs make sure every role(like "Employee" or "Admin") 
            has a unique ID that won’t accidentally match something else.*/

            // NormalizedName is used for case-insensitive comparisons
            },
        
        new IdentityRole
        {
            Id = "ad0a5c18-926f-46cf-97ef-dd3fa31366a0",
                Name = "Supervisor",
                NormalizedName = "SUPERVISOR"
            
        },
            
        new IdentityRole 
            
        {
            Id = "6114021a-4c3c-4052-bebd-ee35e7002e67",
            Name = "Administrator",     
            NormalizedName = "ADMINISTRATOR"
            
        }
          
            
        );

            
            
            var hasher= new PasswordHasher<ApplicationUser>();
            builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "371fde73-4b3e-4038-8c02-f68bcbf32497",
                Email="admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                UserName = "admin@gmail.com",
                PasswordHash = hasher.HashPassword(null,"Pokemon@123"),
                EmailConfirmed = true,
                FirstName = "Default",
                LastName = "Admin",
                DateOfBirth = new DateOnly(1990, 1, 1) 
            }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> 
            {
                RoleId = "6114021a-4c3c-4052-bebd-ee35e7002e67", // Administrator Role
                UserId = "371fde73-4b3e-4038-8c02-f68bcbf32497" // Admin User
            } //ManyToMany Relationship join table      
            );
        }
        /* 
    if you need to have default LeaveTypes in the system,
    you would repeat the builder.Entity<LeaveType>().HasData(
    Just change this to leaveType and 
    then you have new leaveType objects being created there.
    And then that would now set up the DB context to know that 
    when you're creating the model, make sure that you have this default data.
    So this technique is not unique to roles and users 
    But you can use it for other tables 
    if you need the database to start off with default values.*/

        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<Period> Periods { get; set; }

        public DbSet<LeaveAllocation> LeaveAllocations { get; set; }

    }

  

    
}




