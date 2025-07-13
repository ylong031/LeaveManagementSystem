/*
ApplicationUser inherits from IdentityUser, so it already has all the properties of IdentityUser (like Id, UserName, Email, etc.).
You’ve added three custom properties: FirstName, LastName, and DateOfBirth.
This class is used to represent users with additional profile information in your system.
*/


using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Web.Data
{
    public class ApplicationUser:IdentityUser
    {
  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; } 
    }
    
}



