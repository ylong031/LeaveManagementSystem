using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace LeaveManagementSystem.Web.Controllers
{
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService,ILeaveRequestsService _leaveRequestsService
        ) : Controller
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

            /* 
            Converts the list of leave types into a SelectList,
            which is suitable for binding to a dropdown list in the view(displaying the name, but using the ID as the value).
            When a user selects an option and submits the form, the value sent to the server is the selected option's Id, not its name.
            */

            var leaveTypesList =new SelectList(leaveTypes, "Id", "Name");
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM model)
        {
            //Validate that the days don't exceed the allocation
            if (await _leaveRequestsService.RequestDatesExceedAllocation(model))
            {
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested is invalid");
            }

            if (ModelState.IsValid) 
            {
                await _leaveRequestsService.CreateLeaveRequest(model);

            }
            
            /*           
            If the model state is not valid, 
            we need to repopulate the LeaveTypes list
            as it is not bound to the model state
            */
            var leaveTypes = await _leaveTypesService.GetAll();
            model.LeaveTypes = new SelectList(leaveTypes, "Id", "Name");
            return View(model);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(/*Use View Model*/)
        {

            return View();
        }
    }
}
