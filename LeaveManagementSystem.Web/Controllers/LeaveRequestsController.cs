using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService) : Controller
    {
        //Employee View requests
        public async Task<IActionResult> Index()
        {
            return View();
        }
        //Employee Create requests
        public async Task<IActionResult> Create()
        {
            var leaveTypes = await _leaveTypesService.GetAll();
            var leaveTypesList=new SelectList(leaveTypes, "Id", "Name");
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList
            };
            return View(model);
        }
        //Employee Create requests
        [HttpPost]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
         
            return View();
        }

        //Employee Cancel requests
        [HttpPost]
        public async Task<IActionResult> Cancel(int leaveRequestId)
        {

            return View();
        }
        //Admin/Supervisor review Requests
        public async Task<IActionResult> ListRequests()
        {
            return View();
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> Review(int leaveRequestId)
        {
            return View();
        }
        //Admin/Supervisor review Requests
        [HttpPost]
        public async Task<IActionResult> Review(/*Use View Model*/)
        {

            return View();
        }
    }
}
