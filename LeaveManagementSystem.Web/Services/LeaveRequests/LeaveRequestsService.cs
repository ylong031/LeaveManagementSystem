using AutoMapper;
using Azure.Core;
using Humanizer;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _context) : ILeaveRequestsService
    {
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest=await _context.LeaveRequests
                .FirstAsync(q => q.Id == leaveRequestId);

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;

            //When a leave request is cancelled, we need to add the days back to the allocation
            var numberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == leaveRequest.EmployeeId && q.LeaveTypeId == leaveRequest.LeaveTypeId);

            allocation.Days += numberOfDays;


            await _context.SaveChangesAsync();
        }

        //leaveRequestCreateVM(StartDate,EndDate,LeaveTypeId,RequestComments)
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            
            /* 
              
            To create a LeaveRequest, provide the foreign key IDs for the related entities.  
            The navigation properties will be handled by Entity Framework when you query the object later.
          
            ID: Use when referencing or linking entities, especially for database operations or when the client needs to select an entity.
            Object/Descriptive Property: Use in view models/DTOs for displaying information or when you want to show more details to the user.
            
             */

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.LeaveRequests.Add(leaveRequest);
         

            var numberOfDays = model.EndDate.DayNumber- model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);

            allocationToDeduct.Days -= numberOfDays;
            await _context.SaveChangesAsync();
        }

    


        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor?.HttpContext?.User);
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            
            /*   
               
            Maps each LeaveRequest entity to a LeaveRequestReadOnlyVM view model.    
            Calculates the number of days taken for each leave request 
            by subtracting the DayNumber property of start and end dates.
            Casts the numeric LeaveRequestStatusId to the LeaveRequestStatusEnum.*/

            /*
            You are using manual mapping mainly because you have custom logic(enum conversion, calculations, nested properties).
            */

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,


            }).ToList();
            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _context.LeaveAllocations
              .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);
            return allocation.Days < numberOfDays;
        }

        public Task ReviewLeaveRequest(ReviewLeaveRequestVM model)
        {
            throw new NotImplementedException();
        }

        Task<EmployeeLeaveRequestListVM> ILeaveRequestsService.AdminGetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }
    }
}
   