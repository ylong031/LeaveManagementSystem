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
    }

  

    /*
    ApplicationDbContext is a custom database context class in ASP.NET Core
    that acts as a bridge between your C# code and the database,
    using Entity Framework Core(EF Core).
    
    It tells EF Core:
    How to connect to the database
    Which tables (entities) exist
    How to query or save data
     
     */
}
