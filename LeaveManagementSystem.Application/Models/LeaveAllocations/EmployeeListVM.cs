




public class EmployeeListVM
{
    /*
    When you allow the property to not be nullable,
    then it is going to be seen as required,
    even if you don't put the required tag above it.
    So that's something that you have to be very mindful of your properties.
    The data type matters.
    So I'm just going to initialize all of these to string dot empty 
    and do the same for all the other string properties.
    */
    public string Id { get; set; } = string.Empty;

    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

}
//Email is part of IdentityUser class so it wont be an issue
//even if ApplicationUser doesnt have it