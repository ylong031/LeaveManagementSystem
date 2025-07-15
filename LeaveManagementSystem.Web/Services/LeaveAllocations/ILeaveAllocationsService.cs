namespace LeaveManagementSystem.Web.Services.LeaveAllocations
{
    public interface ILeaveAllocationsService
    {
        Task AllocattionLeave(string employeeId);
    }
}