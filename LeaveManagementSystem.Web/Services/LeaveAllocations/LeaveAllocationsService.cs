using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection.Metadata;
using static NuGet.Packaging.PackagingConstants;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public class LeaveAllocationsService(ApplicationDbContext _context,
       
        IMapper _mapper,
        IPeriodsService _periodsService,
        IUserService _userService) : ILeaveAllocationsService
    {
        public async Task AllocateLeave(string employeeId)
        {

            //all leave types that have not been allocated to a specific employee
            var leaveTypes = await _context.LeaveTypes
                .Where(q => !q.LeaveAllocations.Any(x=>x.EmployeeId==employeeId))
                .ToListAsync();


            var period = await _periodsService.GetCurrentPeriod();
            var monthsRemaining=period.EndDate.Month-DateTime.Now.Month;

           

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






        //employees(null) and administrator(not null)

        // For admin to retrieve the leave allocations for a specific employee
        // For employees to retrieve their own leave allocations
        public async Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId) 
        {
            var user = string.IsNullOrEmpty(userId)
                ? await _userService.GetLoggedInUser()
                : await _userService.GetUserById(userId);


            var allocations = await GetAllocations(user.Id);
            //convert the list of leaveallocations of the login user into leaveallocationVM
            var allocationVmList = _mapper.Map<List<LeaveAllocation>, List<LeaveAllocationVM>>(allocations);
           
            var leaveTypesCount = await _context.LeaveTypes.CountAsync();

            var employeeVm = new EmployeeAllocationVM
            {
                DateOfBirth=user.DateOfBirth,
                Email=user.Email,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Id=user.Id,
                LeaveAllocations=allocationVmList,
                //check if the number of leave types is equal to the number of leave allocations
                IsCompletedAllocation = leaveTypesCount == allocations.Count


            };

            return employeeVm;
        }


        //we didnt .Include employee details as we dont want to retrieve the employee details for every leaveallocation
        //since we are already going to retrieve just once in the GetEmployeeAllocations method

        //retreive the list of leaveallocations(sick leave,annual leave) of the login user
        private async Task<List<LeaveAllocation>> GetAllocations(string? userId)
        {

            var period = await _periodsService.GetCurrentPeriod();

            var leaveAllocations = await _context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Include(q => q.Period)
                .Where(q => q.EmployeeId == userId && q.Period.Id == period.Id)
                .ToListAsync();

            return leaveAllocations;
        }





        /*For Admin to get the list of employees*/
        public async Task<List<EmployeeListVM>> GetEmployees() 
        {
            var users = await _userService.GetEmployees();
            var employees=_mapper.Map<List<ApplicationUser>, List<EmployeeListVM>>(users.ToList());

            return employees;
        }
        
        
        
        /* Use.Include() to load related entities(via navigation properties/foreign keys).
         both the LeaveType and Employee objects are loaded for each LeaveAllocation, not just their foreign key IDs.*/

        //retrieve the specific leave allocation by its ID for editing purposes
        public async Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId)
        {
            // Retrieve the leave allocation by its ID, including related LeaveType and Employee details
            var allocation =await _context.LeaveAllocations
                .Include(q=>q.LeaveType)
                .Include(q=> q.Employee)
                .FirstOrDefaultAsync(q=>q.Id==allocationId);

            var model=_mapper.Map<LeaveAllocationEditVM>(allocation);

            return model;

        }

        public async Task EditAllocation(LeaveAllocationEditVM allocationEditVM)
        {
            /* 
              var leaveAllocation = await GetEmployeeAllocation(allocationEditVM.Id);
              if (leaveAllocation == null)
              {
                  throw new Exception("Leave allocation record does not exist");
              }
              leaveAllocation.Days = allocationEditVM.Days;
              _context.Update(leaveAllocation);  //update all the fields of the leaveAllocation object
              _context.Entry(leaveAllocation).State = EntityState.Modified;  // update only field(s) that have changed  
              await _context.SaveChangesAsync();
            */

            /*It's either that it cannot find it, so nothing gets updated or it found it and it gets updated.
            So that spares us all of this trouble of checking if it's null and everything.*/
            /*So that actually allows us to bypass all of this and spare selves a potential double query.*/

            await _context.LeaveAllocations
                .Where(q => q.Id == allocationEditVM.Id)
                .ExecuteUpdateAsync(q => q.SetProperty(e => e.Days, allocationEditVM.Days));
        }




        // retrieve a specific leave allocation based on the leave type and employee ID
        public async Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId)
        {

            var period=await _periodsService.GetCurrentPeriod();
            var allocation = await _context.LeaveAllocations
                .FirstAsync(q => q.LeaveTypeId == leaveTypeId 
                && q.EmployeeId == employeeId 
                && q.PeriodId == period.Id);
            return allocation;

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

