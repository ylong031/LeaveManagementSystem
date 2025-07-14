using Humanizer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveAllocation:BaseEntity
    {
        public LeaveType? LeaveType { get; set; }

        /*
        just by calling it leavetype ID coupled
        with this navigation property(the one above it),
        it will automatically deduce that this is a foreign key reference to
        the ID column inside of this navigation property.
        */
        public int LeaveTypeId {  get; set; }

        public ApplicationUser Employee {  get; set; }
        public string EmployeeId { get; set; }

        public Period Period { get; set; }
        public int PeriodId {  get; set; }

        public int Days { get; set;}


    }
}
