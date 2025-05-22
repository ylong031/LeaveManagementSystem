using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

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
