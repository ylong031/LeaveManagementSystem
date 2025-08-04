using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace LeaveManagementSystem.Data
{
    public static class DataServicesRegistration
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            /*This retrieves the connection string named DefaultConnection from your appsettings.json.
             If it’s missing or null, it throws a clear error.*/
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            /*Tell ASP.NET Core: "I want to use a database context called ApplicationDbContext in my app."*/
            /*When setting it up, use SQL Server as the database, and connect to it using the connectionString variable.*/


            /*In this example, the options object inside the lambda is a DbContextOptionsBuilder,
            which configures things like the connection string and provider.
            This configuration is then used to create the DbContextOptions that get passed to your ApplicationDbContext.*/

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
