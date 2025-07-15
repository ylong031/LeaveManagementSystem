using LeaveManagementSystem.Web.Services.LeaveAllocations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService) : Controller
    {
        public async Task<IActionResult> Details()
        {
            
        
            var employeeVm=await _leaveAllocationsService.GetEmployeeAllocations();
            return View(employeeVm);
        }
    }
}
