

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;


namespace LeaveManagementSystem.Application.Models.LeaveRequests
{
    public class LeaveRequestCreateVM : IValidatableObject
    {
        [DisplayName("Start Date")]
        [Required]
        public DateOnly StartDate { get; set; }
        [DisplayName("End Date")]
        [Required]
        public DateOnly EndDate { get; set; }

        [DisplayName("Desired Leave Type")]
        [Required]
        public int LeaveTypeId { get; set; }

        [DisplayName("Additional Information")]
        [StringLength(250)]
        public string? RequestComments { get; set; }


        public SelectList? LeaveTypes { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate > EndDate)
            {
                /*  The error message will be shown under both the StartDate and EndDate fields.*/
                yield return new ValidationResult("The Start Date must be before the End Date",
                    [nameof(StartDate), nameof(EndDate)]);

            }
        }
        /*
         Use IValidatableObject when you need to implement custom validation logic 
         that involves multiple properties of a model, or when you need validation 
         that cannot be expressed using simple data annotations like [Required], [Range], [StringLength]
         It’s especially useful in ViewModels for UI validation scenarios.
        */
    }
}