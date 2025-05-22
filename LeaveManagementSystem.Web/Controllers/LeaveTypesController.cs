using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LeaveManagementSystem.Web.Data;
using NuGet.ContentModel;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.FileSystemGlobbing;
using NuGet.Packaging.Signing;
using System.Runtime.Intrinsics.X86;
using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;
using System.Security.Policy;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        /*Dependency Injection means that your classes don’t create the objects they need.
        Instead, those needed objects (called dependencies) are passed in from the outside*/
        
        private readonly ApplicationDbContext _context;

        public LeaveTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        // GET: LeaveTypes
        public async Task<IActionResult> Index() 
        {
            /*  linq
            var data = SELECT * FROM LeaveTypes
            
           it will do the conversion of the data coming 
           from a database into C sharp objects,
           which I can now store in my C sharp variable.*/

            var data = await _context.LeaveTypes.ToListAsync();
            return View(data);
        }



        /* Go to the leave types table and get me the first or default record that matches this lambda expression.
        So that's a lambda expression.
        And the lambda expression is used in link statements to define some condition.
        That must be true in order for the data to be returned.
        So in other words, it must be true that there there is some ID.
        So M here represents each record.
        So it's going to go through each record.
        So M here represents a record in the database or in that table rather.
        So that represents a leave type object.
        And then we're saying check for every leave type object or record.
        Check the ID property to see if there is any.
        That's equivalent to the ID value that came in,*/


        // GET: LeaveTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); //id not provided
            }
            //SELECT * FROM LeaveTypes WHERE Id = id
            var leaveType = await _context.LeaveTypes 
                .FirstOrDefaultAsync(m => m.Id == id);
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

        // This method is called when the user submits the form to create a new leave type.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,NumberOfDays")] LeaveType leaveType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leaveType); /*Add leave application to database*/
                await _context.SaveChangesAsync(); /*Save changes to the database*/
                return RedirectToAction(nameof(Index));  /*return to index*/
            }
            return View(leaveType); /*if the model state is not valid, return to the view*/
        }

        // GET: LeaveTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); //empty id
            }
            //SELECT * FROM LeaveTypes WHERE Id = id
            var leaveType = await _context.LeaveTypes.FindAsync(id); 
            if (leaveType == null)
            {
                return NotFound(); //leave doesnt exist
            }
            return View(leaveType);
        }

        // POST: LeaveTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,NumberOfDays")] LeaveType leaveType) //id is the hidden field in the form
        {
            //check if the id is the same as the id in the leaveType object
            if (id != leaveType.Id)  
            {
                return NotFound(); //make sure you are editing the correct leave type
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leaveType);//update the leave type
                    await _context.SaveChangesAsync();// save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveType.Id))
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
            return View(leaveType); //if the model state is not valid, return to the view and show the errors with the invalid data
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

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
