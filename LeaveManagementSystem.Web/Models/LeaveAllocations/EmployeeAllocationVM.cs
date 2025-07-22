using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{
    /*Now notice that this is going to be the view model 
     * for that detail page that we're going to create.*/
    public class EmployeeAllocationVM:EmployeeListVM
    {
     
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set;}

        public bool IsCompletedAllocation { get; set; }
        public List<LeaveAllocationVM> LeaveAllocations {  get; set; }  
   
    }
}
/*View models should only ever reference view models.
You should never reference a data model from inside of a view model.
So I have to create a leave allocation VM to talk to the employee allocation VM, because I don't want
my employee allocation VM to have direct knowledge or any integration with the leave allocation data
model.*/