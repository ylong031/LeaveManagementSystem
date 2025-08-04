using LeaveManagementSystem.Application.Models.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LeaveManagementSystem.Web.Controllers
{
    //only authenticated users can access this controller
    [Authorize]
    public class LeaveRequestsController(ILeaveTypesService _leaveTypesService, ILeaveRequestsService _leaveRequestsService
        ) : Controller
    {
        //Employee View requests
        public async Task<IActionResult> Index()
        {
            var model = await _leaveRequestsService.GetEmployeeLeaveRequestsHistory();
            return View(model);
        }
        //Employee Create requests
        public async Task<IActionResult> Create(int? leaveTypeId)
        {


            var leaveTypes = await _leaveTypesService.GetAll();


            /* 
            Converts the list of leave types into a SelectList,
            which is suitable for binding to a dropdown list in the view(displaying the name, but using the ID as the value).
            When a user selects an option and submits the form, the value sent to the server is the selected option's Id, not its name.
            
            leaveTypeId is the selected leave type Id, if it is null,
            it will default to the first item in the list.
             */

            var leaveTypesList = new SelectList(leaveTypes, "Id", "Name", leaveTypeId);
            var model = new LeaveRequestCreateVM
            {
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                LeaveTypes = leaveTypesList,

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
                //Top of the screen error
                ModelState.AddModelError(string.Empty, "You have exceeded your allocation");
                //under the EndDate field error
                ModelState.AddModelError(nameof(model.EndDate), "The number of days requested is invalid");
            }

            if (ModelState.IsValid)
            {
                await _leaveRequestsService.CreateLeaveRequest(model);
                return RedirectToAction(nameof(Index));
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

        /*[HttpPost] = Only respond to POST requests(form submissions).*/
        /* [ValidateAntiForgeryToken] is a safety feature in ASP.NET that helps protect your website from fake form submissions.*/
        //Employee Cancel requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            await _leaveRequestsService.CancelLeaveRequest(id);
            return RedirectToAction(nameof(Index));
        }


        //Admin/Supervisor review Requests
        [Authorize(Policy = "AdminSupervisorOnly")]
        public async Task<IActionResult> ListRequests()
        {
            var model = await _leaveRequestsService.AdminGetAllLeaveRequests();
            return View(model);
        }

        //Admin/Supervisor review Requests
        public async Task<IActionResult> Review(int id)
        {
            var model = await _leaveRequestsService.GetLeaveRequestForReview(id);
            return View(model);
        }
        //Admin/Supervisor review Requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, bool approved)
        {
            await _leaveRequestsService.ReviewLeaveRequest(id, approved);
            return RedirectToAction(nameof(ListRequests));
        }
    }
}
