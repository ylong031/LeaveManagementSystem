
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context,
        IHttpContextAccessor _httpContextAccessor,UserManager<ApplicationUser> _userManager) : ILeaveAllocationsService
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
                .Include(q => q.Employee)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            return leaveAllocations;
        }
    }
    
}


