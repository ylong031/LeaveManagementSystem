
using LeaveManagementSystem.Application.Models.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveAllocations;
using LeaveManagementSystem.Application.Services.LeaveTypes;


namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveAllocationController(ILeaveAllocationsService _leaveAllocationsService,
        ILeaveTypesService _leaveTypesService
        ) : Controller
    {

        /*For Admin to get the list of employees*/
        [Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> Index()
        {
            var employees = await _leaveAllocationsService.GetEmployees();
            return View(employees);

        }

        /* Now, more often than not, when you are creating some method that is going to go ahead and modify data
 in the system, you don't want to rely on a simple anchor tag, because what happens here?
 When I click Allocate leave, it simply calls that URL, which carries out some changing action, which
 means that an employee who is paying attention might be able to use something like postman or so on and actually spoof that operation.
 The only difference is that we have tied it down to the administrator role.
 But that's not really, uh, foolproof or that's really not enough.
 Because we still are at risk of cross-site scripting and other vulnerabilities.*/

        /*   And we've reviewed why it's important to use forms because forms come with that token validation.
    So if we that validate Anti-forgery token and it's a Post request so it hides sensitive data.*/


        [Authorize(Roles = Roles.Administrator)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AllocateLeave(string? Id)
        {
            await _leaveAllocationsService.AllocateLeave(Id);


            /*When you use methods like RedirectToAction, Url.Action,
            or tag helpers that generate links and you pass route values:
            The key(userId) must match the action method's parameter name (userId).*/

            //redirect to the details page of the employee passing the userId
            return RedirectToAction(nameof(Details), new { userId = Id });

        }

        public async Task<IActionResult> EditAllocation(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var allocation = await _leaveAllocationsService.GetEmployeeAllocation(id.Value);
            if (allocation == null)
            {
                return NotFound();
            }
            //View name must match the action method name
            return View(allocation);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAllocation(LeaveAllocationEditVM allocation)
        {
            if (await _leaveTypesService.DaysExceedMaximum(allocation.LeaveType.Id, allocation.Days))
            {
                ModelState.AddModelError("Days", "The allocation exceeds the maximum leave type value");

            }
            if (ModelState.IsValid)
            {
                await _leaveAllocationsService.EditAllocation(allocation);
                return RedirectToAction(nameof(Details), new { userId = allocation.Employee.Id });

            }

            //ModelState is invalid, so we need to return the same view with the model

            // Preserve the number of days before the edit
            var days = allocation.Days;

            // Re-fetch the EmployeeAllocation data for views
            // (LeaveAllocationEditVM.Employee & LeaveAllocationEditVM.LeaveType)
            allocation = await _leaveAllocationsService.GetEmployeeAllocation(allocation.Id);

            allocation.Days = days;

            return View(allocation);


        }


        /*
           Now the importance of making it nullable is if the employee clicks on the details link.
           There is no need for an ID, right?Because we're using the ID of the logged in person.
           However, an admin needs to see that employee real time.
        */

        //employees(null) and administrator(not null)

        // This method retrieves the leave allocations for a specific employee
        public async Task<IActionResult> Details(string? userId)
        {
            var employeeVm = await _leaveAllocationsService.GetEmployeeAllocations(userId);
            return View(employeeVm);
        }
    }
}
