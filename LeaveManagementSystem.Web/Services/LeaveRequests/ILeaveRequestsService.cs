﻿using LeaveManagementSystem.Web.Models.LeaveRequests;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public interface ILeaveRequestsService
    {
        Task CreateLeaveRequest(LeaveRequestCreateVM model);
        Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsHistory();

        Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests();

        Task CancelLeaveRequest(int leaveRequestId);

        Task ReviewLeaveRequest(int leaveRequestId,bool approved);

        Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model);
        Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id);
    }
}