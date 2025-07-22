

/*In the LeaveAllocation class, the property EmployeeId is defined as a string because
it is referencing the primary key of the ApplicationUser entity.
In ASP.NET Core Identity (which is commonly used in ASP.NET projects for user management),
the primary key for the ApplicationUser class is typically a string, not an integer.
This is because Identity uses strings (often GUIDs or other unique string identifiers) for user IDs by default,
providing more flexibility and uniqueness than integers.
Unless you specifically configure Identity to use an integer (which requires custom setup),
the default is a string.*/

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveAllocation:BaseEntity
    {
        public LeaveType? LeaveType { get; set; }

        public int LeaveTypeId {  get; set; }

        public ApplicationUser? Employee {  get; set; }
        public string EmployeeId { get; set; }

        public Period? Period { get; set; }
        public int PeriodId {  get; set; }

        public int Days { get; set;}


    }
}
/*
just by calling it leavetype ID coupled
with this navigation property(the one above it),
it will automatically deduce that this is a foreign key reference to
the ID column inside of this navigation property.
*/
/*With navigation property,You can use it to access details of the leave type from a leave allocation(allocation.LeaveType.Name).*/

/*
+----------------+ +----------------+ +----------------+
| ApplicationUser| | LeaveType      | |    Period      |
+----------------+ +----------------+ +----------------+
         |                    |                    |
         |                    |                    |
         |                    |                    |
         |                    |                    |
         |                    |                    |
         v                    v                    v
+---------------------------------------------------------------+
|                    LeaveAllocation                            |
+---------------------------------------------------------------+
| LeaveAllocationId (PK)                                        |
| ApplicationUserId (FK)  ------+                               |
| LeaveTypeId (FK)        ------+                               |
| PeriodId (FK)           ------+                               |
| Days                                                      ... |
+---------------------------------------------------------------+
*/