
using System.ComponentModel;

namespace LeaveManagementSystem.Web.Models.LeaveRequests
{
  
        
    public class ReviewLeaveRequestVM: LeaveRequestReadOnlyVM
    {
        public EmployeeListVM Employee { get; set; } = new EmployeeListVM();

        [DisplayName("Additional Information")]
        public string RequestComments { get; set; }

    }
    
}

/*
In most cases, especially in CRUD (Create, Read, Update, Delete) applications,
the view model will have an Id property that matches the data model’s Id because:
You need to identify which record you are viewing/editing/deleting.
It simplifies mapping between the view model and data model (for example, with tools like AutoMapper).
*/
