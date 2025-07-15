using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context,
        IHttpContextAccessor _httpContextAccessor,
        UserManager<ApplicationUser> _userManager,
        IMapper _mapper) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {
            //get all the leave types
            var leaveTypes = await _context.LeaveTypes.ToListAsync();
            
            
            //get the current period based on the year
            var currentDate= DateTime.Now;
            //SingleAsync will throw an exception if it finds more than one entry
            var period = await _context.Periods.SingleAsync(q => q.EndDate.Year == currentDate.Year);
            var monthsRemaining=period.EndDate.Month-currentDate.Month;

           

            //foreach leave type,create an allocation entry
            foreach (var leaveType in leaveTypes)    
            {
                //24 days annual leaves divided by 12 = getting 2 days per month
                //12 days sick leaves divided by 12 = getting 1 day per month
                var accrualRate = decimal.Divide(leaveType.NumberOfDays, 12); 
                var leaveAllocation = new LeaveAllocation
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = leaveType.Id,
                    PeriodId = period.Id,
                    Days = (int)Math.Ceiling(accrualRate*monthsRemaining)
                };
                _context.Add(leaveAllocation);//it will only be added to the database when savechanges is used.

            }
            await _context.SaveChangesAsync();


        }
        public async Task<List<LeaveAllocation>> GetAllocations()
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            return leaveAllocations;
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocations() 
        {
            var allocations = await GetAllocations();
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);

            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth=user.DateOfBirth,
                Email=user.Email,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Id=user.Id,
                LeaveAllocations=allocationVmList


            };

            return employeeVm;
        }

    }
    
}
/*
The code _httpContextAccessor.HttpContext?.User retrieves the user who is currently logged in to the website.
This means it gets the identity (and claims) of the user making the current HTTP request.
When you use this in your service, any queries or actions (like fetching leave allocations)
will be performed for the currently authenticated user—the one who is signed in and using the site at that moment.
*/

/*
_userManager.GetUserAsync(...) takes that identity and retrieves the full user object from your database.
*/

