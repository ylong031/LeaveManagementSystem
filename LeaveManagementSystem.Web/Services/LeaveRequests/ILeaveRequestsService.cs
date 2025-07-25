using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequests();

        Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests();

        Task CancelLeaveRequest(int leaveRequestId);

        Task ReviewLeaveRequest(ReviewLeaveRequestVM model);

        Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
    }
}