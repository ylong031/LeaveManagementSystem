using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.Period;

namespace LeaveManagementSystem.Web.Models.LeaveAllocations
{

    /*The leave allocation VM is going to serve the purpose of mapping for the leave allocations that I'm
      pulling from the database.*/
    public class LeaveAllocationVM
    {
        public int Id {  get; set; }

        [Display(Name ="Number Of Days")]
        public int NumberOfDays {  get; set; }

        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();
        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM(); 
    }
}
