/*       
Navigation Property is a property that represents a relationship between two tables in your database
(for example, between LeaveRequest and LeaveType).
It lets you easily access related data through your object model.
*/

/* 
The navigation property is nullable to allow for cases when the related entity is not loaded or assigned.
The foreign key is not nullable to enforce a required relationship in your database.
*/




namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequest: BaseEntity
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
       
        public LeaveType? LeaveType { get; set; }
        public int LeaveTypeId { get; set; }

        public LeaveRequestStatus? LeaveRequestStatus { get; set; }

        public int LeaveRequestStatusId { get; set; }

        public ApplicationUser? Employee { get; set; }
        public string EmployeeId { get; set; }= string.Empty;

        /*
        When we discussed foreign keys,
        we talked about the fact that this is showing a relationship between the two tables.
        Up until now, all of our foreign keys have been not nullable.
        Let's say that right in this situation, I have to make ReviewerId nullable,
        because I have to allow the record to go into the database without this data the first time,
        and if I fail to make it a nullable property that I'm going to end up with a referential integrity issue.
        */
        public ApplicationUser? Reviewer { get; set; }
        public string? ReviewerId { get; set; }
       
        
        public string? RequestComments { get; set; }
    }
}

