using LeaveManagementSystem.Application.Models.LeaveTypes;
using LeaveManagementSystem.Application.Models.Period;

namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{

    /*The leave allocation VM is going to serve the purpose of mapping for the leave allocations that I'm
      pulling from the database.*/
    public class LeaveAllocationVM
    {
        public int Id { get; set; }

        [Display(Name = "Number Of Days")]
        public int Days { get; set; }

        [Display(Name = "Allocation Period")]
        public PeriodVM Period { get; set; } = new PeriodVM();
        public LeaveTypeReadOnlyVM LeaveType { get; set; } = new LeaveTypeReadOnlyVM();
    }


}
