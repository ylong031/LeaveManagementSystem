
using LeaveManagementSystem.Web.Models.LeaveAllocations;

namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocateLeave(string employeeId);
        Task<EmployeeAllocationVM> GetEmployeeAllocations(string? userId);

        Task<List<EmployeeListVM>> GetEmployees();

        Task<LeaveAllocationEditVM> GetEmployeeAllocation(int allocationId);
     
        Task EditAllocation(LeaveAllocationEditVM allocationEditVM);

        Task<LeaveAllocation> GetCurrentAllocation(int leaveTypeId, string employeeId);
    }
}