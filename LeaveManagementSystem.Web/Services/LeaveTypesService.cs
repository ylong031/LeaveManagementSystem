using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
namespace LeaveManagementSystem.Web.Services;

/*A primary constructor is a feature that allows you
to declare constructor parameters directly in the class*/
public class LeaveTypesService(ApplicationDbContext _context, IMapper _mapper) : ILeaveTypesService
{
   

    public async Task<List<LeaveTypeReadOnlyVM>> GetAll()
    {
        // Fetch all leave types from the database
        var data = await _context.LeaveTypes.ToListAsync();
        var viewData = _mapper.Map<List<LeaveTypeReadOnlyVM>>(data);
        return viewData;
    }

    public async Task<T?> Get<T>(int id) where T : class // T? can be null
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        /* First will look for the record and return the first element.
        If nothing is present, it throws an exception  

        FirstOrDefault, which we know will look
        for the first element that meets the condition, 
        and if none is found,then it will return null.*/

        if (data == null)
        {
            return null;
        }
        var viewData = _mapper.Map<T>(data);
        /* Caller will specify the type of view model
         they want to map to when they call the Get<T> method*/


        return viewData;
        //type T this will be either LeaveTypeReadOnlyVM or LeaveTypeEditVM or LeaveTypeCreateVM
    }
    public async Task Remove(int id)
    {
        var data = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (data != null)
        {
            _context.Remove(data);
            await _context.SaveChangesAsync();
        }


    }

    public async Task Edit(LeaveTypeEditVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model);
        _context.Update(leaveType);//update the leave type
        await _context.SaveChangesAsync();// save changes to the database
    }


    public async Task Create(LeaveTypeCreateVM model)
    {
        var leaveType = _mapper.Map<LeaveType>(model); //Convert view model into data model
        _context.Add(leaveType); /*Add leave application to database*/
        await _context.SaveChangesAsync(); /*Save changes to the database*/
    }

    public bool LeaveTypeExists(int id)
    {
        return _context.LeaveTypes.Any(e => e.Id == id);
    }

    public async Task<bool> CheckIfLeaveTypeNameExists(string name)
    {
        var lowercaseName = name.ToLower();// Convert the name to lowercase for case-insensitive comparison
        return await _context.LeaveTypes.AnyAsync
            (q => q.Name.ToLower().Equals(lowercaseName));

        /*This will check if the name already exists in the database.*/
    }

    public async Task<bool> CheckIfLeaveTypeNameExistsForEdit(LeaveTypeEditVM leaveTypeEdit)
    {
        var lowercaseName = leaveTypeEdit.Name.ToLower();
        return await _context.LeaveTypes.AnyAsync
            (q => q.Name.ToLower().Equals(lowercaseName)
            && q.Id != leaveTypeEdit.Id);
        //error message will be shown if name already exists in other ids
    }

}
 