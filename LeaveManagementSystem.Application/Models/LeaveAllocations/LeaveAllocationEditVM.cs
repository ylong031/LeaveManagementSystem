

namespace LeaveManagementSystem.Application.Models.LeaveAllocations
{
    public class LeaveAllocationEditVM : LeaveAllocationVM
    {
        public EmployeeListVM Employee { get; set; }


    }


}
/*
1.Separation of Concerns

Read-only ViewModel
Contains only the properties needed to display data.
No validation or properties for editing.

Edit ViewModel
Includes properties required for creating or editing data,
often with validation attributes.

2. Security

Prevents Overposting:
If you use the same model for both display and editing, users could potentially send data for properties you didn’t intend to expose. Using a dedicated edit model ensures you only bind the intended fields.

3. Validation

Edit models can have [Required], [StringLength], etc., for input validation, while read - only models don’t need them.

4. Performance & Simplicity

Read-only models can be lightweight, containing only what you need for display—faster to transfer and easier to work with in views.

5. Flexibility

If your display and edit needs change (for example, you want to show more info in details than in edit), you can update each ViewModel independently.
*/
