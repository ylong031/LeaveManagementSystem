using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using Microsoft.EntityFrameworkCore;



namespace LeaveManagementSystem.Web.Controllers
{
    /*Only allowing Administrator to access LeaveTypesController*/
    [Authorize(Roles = Roles.Administrator)]
    public class LeaveTypesController(ILeaveTypesService _leaveTypesService) : Controller
    {
        /*Dependency Injection means that your classes don’t create the objects they need.
        Instead, those needed objects (called dependencies) are passed in from the outside*/


        private const string NameExistsValidationMessage = "This leave type already exists in the database";




        /*async means:
       "This method might take time to finish, so let it run in the background."
       It helps your app not freeze or get stuck while waiting for something(like getting data from a database).
       await means:
       "Wait for this task to finish, but don’t block everything else.
       It tells your program:
       "Go do other things while we wait for this to finish."
       IActionResult – What is that?
       It means:
       "This method returns something that the browser will see."
       Example: a view, a redirect, a JSON response, or a status code like 404.
       Task means:
       "This method does something in the background, and we’ll get the result later."
       */
        /*  linq
           var data = SELECT * FROM LeaveTypes

          it will do the conversion of the data coming 
          from a database into C sharp objects,
          which I can now store in my C sharp variable.
        
         Next we will convert datamodel into viewmodel
         */
        // GET: LeaveTypes
        public async Task<IActionResult> Index()
        {

            var viewData = await _leaveTypesService.GetAll();
            return View(viewData);
        }





        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); //id not provided
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            //Get the leave type by id


            if (leaveType == null)
            {
                return NotFound(); //leavetype doesnt exist
            }




            return View(leaveType);
        }

        // GET: LeaveTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeaveTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        /*So the problem with using the data model here is that the data model has the ID, the name, and the
        number of days, which means that somebody could actually in real time, like when they load up the
        form, they could modify the form in the browser using inspect element and insert a new input for a
        field like ID that I should not be accepting.
        And because I have this code, I would inadvertently actually accept that data.
        So that's what over posting really means.

        savechangesAsync would not allow when you try to insert a value into an identity column(primary key)
        */

        // This method is called when the user submits the form to create a new leave type.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM leaveTypeCreate)
        {

            /*
             If it finds duplication, it will add an error to the model state,
             which will be displayed in the view
            */
            if (await _leaveTypesService.CheckIfLeaveTypeNameExists(leaveTypeCreate.Name))
            {

                ModelState.AddModelError(nameof(leaveTypeCreate.Name),
                    NameExistsValidationMessage);


            }


            if (ModelState.IsValid) //it will also check if the field is empty or not
            {
                await _leaveTypesService.Create(leaveTypeCreate);
                return RedirectToAction(nameof(Index));  /*return to index*/
            }
            return View(leaveTypeCreate); /*if the model state is not valid, return to the view*/
        }



        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); //empty id
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeEditVM>(id.Value);
            //In the service,it will get the leave type in LeaveTypeEditVM by id 


            if (leaveType == null)
            {
                return NotFound(); //leavetype doesnt exist
            }


            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM leaveTypeEdit) //id is the hidden field in the form
        {
            //check if the id is the same as the id in the leaveType object
            if (id != leaveTypeEdit.Id)
            {
                return NotFound(); //make sure you are editing the correct leave type
            }

            if (await _leaveTypesService.CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name),
                    NameExistsValidationMessage);
                //custom error message

            }


            if (ModelState.IsValid)
            {
                try
                {
                    //In the service,it will convert view model into data model and update it in the database
                    await _leaveTypesService.Edit(leaveTypeEdit);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_leaveTypesService.LeaveTypeExists(leaveTypeEdit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(leaveTypeEdit); //if the model state is not valid, return to the view and show the errors with the invalid data
        }




        /*Now it's wrapped in a try catch because of the risk of what we call a database update concurrency exception.
        What this means in a nutshell is what if two persons access the same record made this made changes to
        the same record, but somebody submitted first.
        So that means now that I would submit second would be trying to update a stale version of the record
        because, um, the version has changed since the last time I loaded it.
        So that's what we call a database concurrency exception.
        So if it catches such an exception, it will just tell you, hey, um, it doesn't exist.
        If the ID changed, which is highly unlikely, IDs won't change like that.
        Otherwise it will throw a nasty exception which we as a developer need to handle.*/


        // GET: LeaveTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leaveType = await _leaveTypesService.Get<LeaveTypeReadOnlyVM>(id.Value);
            //Get the leave type by id

            if (leaveType == null)
            {
                return NotFound();
            }


            return View(leaveType);
        }

        // POST: LeaveTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _leaveTypesService.Remove(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
