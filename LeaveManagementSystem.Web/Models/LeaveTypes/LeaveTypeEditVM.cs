using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Web.Models.LeaveTypes
{
    public class LeaveTypeEditVM:BaseLeaveTypeVM
    {
       

        [Required]
        [Length(4, 150, ErrorMessage = "You have violated the length requirements.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, 90)]
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

*/

