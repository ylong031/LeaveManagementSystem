namespace LeaveManagementSystem.Application.Models.LeaveTypes
{
    public class LeaveTypeReadOnlyVM : BaseLeaveTypeVM
    {

        public string Name { get; set; } = string.Empty;

        [Display(Name = "Number of Days")]
        public int NumberOfDays { get; set; }
    }


}

/*
how is it different from data model?

the data model handles the database stuff.
And then we want to take the data from the data model and put it into a view model.
And we can shape it all we want.We can massage it.
We can modify whatever it is we want based on what the view needs,
because we don't want to put too much shaping and too much logic into the data model.
So the view model is a class now that exists specifically for a view.

data model is for data storage and database mapping

view model is for UI representation,form validation 
and business rules related to user input
*/

