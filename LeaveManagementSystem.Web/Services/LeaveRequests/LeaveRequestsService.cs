using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(IMapper _mapper,
        UserManager<ApplicationUser> _userManager,
        IHttpContextAccessor _httpContextAccessor,
        ApplicationDbContext _context) : ILeaveRequestsService
    {
        public Task CancelLeaveRequest(int leaveRequestId)
        {
            throw new NotImplementedException();
        }

        //leaveRequestCreateVM(StartDate,EndDate,LeaveTypeId,RequestComments)
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatus.Pending;
            _context.LeaveRequests.Add(leaveRequest);
         

            var numberOfDays = model.EndDate.DayNumber- model.StartDate.DayNumber;
            var allocationToDeduct = await _context.LeaveAllocations
                .FirstAsync(q => q.EmployeeId == user.Id && q.LeaveTypeId == model.LeaveTypeId);

            allocationToDeduct.Days =- numberOfDays;
            await _context.SaveChangesAsync();
        }

        public Task<LeaveRequestListVM> GetAllLeaveRequests()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveRequestListVM> GetEmployeeLeaveRequests()
        {
            throw new NotImplementedException();
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


    }
}
   