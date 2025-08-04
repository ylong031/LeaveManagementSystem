using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LeaveManagementSystem.Application
{
    //we have to do this because now the automapper is in a different assembly (LeaveManagementSystem.Application)
    public static class ApplicationServicesRegistration
    {


        // adding application services to the DI container
        //In program.cs, you will call this method to register all application services.
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //add auto mapper to the DI container
            services.AddAutoMapper(Assembly.GetExecutingAssembly());



            /*AddScoped: One instance per HTTP request, reused throughout that request (like one waiter per meal).*/
            services.AddScoped<ILeaveTypesService, LeaveTypesService>();

            services.AddScoped<ILeaveAllocationsService, LeaveAllocationsService>();

            services.AddScoped<ILeaveRequestsService, LeaveRequestsService>();

            services.AddScoped<IPeriodsService, PeriodsService>();

            services.AddScoped<IUserService, UserService>();



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

            services.AddTransient<IEmailSender, EmailSender>();



            return services;
        }
    }
}
