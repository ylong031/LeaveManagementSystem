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
using LeaveManagementSystem.Web.Models.LeaveTypes;
using AutoMapper;
using System.Numerics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Xml.Linq;

namespace LeaveManagementSystem.Web.Controllers
{
    public class LeaveTypesController : Controller
    {
        /*Dependency Injection means that your classes don’t create the objects they need.
        Instead, those needed objects (called dependencies) are passed in from the outside*/
        
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private const string NameExistsValidationMessage="This leave type already exists in the database";

        public LeaveTypesController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
            
            var data = await _context.LeaveTypes.ToListAsync();
            /* var viewData = data.Select(q => new IndexVM
             {
                 Id = q.Id,
                 Name = q.Name,
                 NumberOfDays = q.NumberOfDays
             });
            Manual mapping
             */


            //Convert this list of data models into a list of view models.
            var viewData=_mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
            return View(viewData);
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
            
            var leaveType = await _context.LeaveTypes //SELECT * FROM LeaveTypes WHERE Id = id
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound(); //leavetype doesnt exist
            }

            var viewData = _mapper.Map<LeaveTypeReadOnlyVM>(leaveType);
            //Convert one data model into a view model.

            return View(viewData);
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

            /* if (leaveTypeCreate.Name.Contains("dumb")) 
             {
                 ModelState.AddModelError(nameof(leaveTypeCreate.Name), "the word dumb is not allowed."); 
                 //custom error message
             } */
            /*This is a custom validation rule that checks 
             * if the name contains the word "vacation".
             If it does, it adds an error to the model state,
            which will be displayed in the view.*/

            if (await CheckIfLeaveTypeNameExists(leaveTypeCreate.Name)) 
            {
                ModelState.AddModelError(nameof(leaveTypeCreate.Name),
                    NameExistsValidationMessage);
                //custom error message

            }


            if (ModelState.IsValid) //it will also check if the field is empty or not
            {
                var leaveType=_mapper.Map<LeaveType>(leaveTypeCreate); //Convert view model into data model
                _context.Add(leaveType); /*Add leave application to database*/
                await _context.SaveChangesAsync(); /*Save changes to the database*/
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
            
            var leaveType = await _context.LeaveTypes.FindAsync(id); //SELECT * FROM LeaveTypes WHERE Id = id
            if (leaveType == null)
            {
                return NotFound(); //leave doesnt exist
            }

            var viewData = _mapper.Map<LeaveTypeEditVM>(leaveType);

            return View(viewData);
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

            if (await CheckIfLeaveTypeNameExistsForEdit(leaveTypeEdit))
            {
                ModelState.AddModelError(nameof(leaveTypeEdit.Name),
                    NameExistsValidationMessage);
                //custom error message

            }


            if (ModelState.IsValid)
            {
                try
                {
                    var leaveType = _mapper.Map<LeaveType>(leaveTypeEdit);
                    _context.Update(leaveType);//update the leave type
                    await _context.SaveChangesAsync();// save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveTypeExists(leaveTypeEdit.Id))
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

            var leaveType = await _context.LeaveTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leaveType == null)
            {
                return NotFound();
            }
            var viewData=_mapper.Map<LeaveTypeReadOnlyVM>(leaveType);

            return View(viewData);
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

        private async Task<bool> CheckIfLeaveTypeNameExists(string name)
        {
            var lowercaseName = name.ToLower();// Convert the name to lowercase for case-insensitive comparison
            return await _context.LeaveTypes.AnyAsync
                (q => q.Name.ToLower().Equals(lowercaseName));

            /*This will check if the name already exists in the database.*/
        }

        private async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
        {
            var lowercaseName = leaveTypeEdit.Name.ToLower();
            return await _context.LeaveTypes.AnyAsync
                (q => q.Name.ToLower().Equals(lowercaseName) 
                && q.Id != leaveTypeEdit.Id);
            //error message will be shown if name already exists in other ids
        }
    }
}
