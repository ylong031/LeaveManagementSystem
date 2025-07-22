/*You pass options to your context class (ApplicationDbContext), and through inheritance,
those options are ultimately received by DbContext, which is where the actual EF Core database logic lives.
This allows you to add customizations in ApplicationDbContext or IdentityDbContext if needed,
while still ensuring the underlying DbContext has all the necessary configuration.

No matter how many layers of inheritance you have (ApplicationDbContext → IdentityDbContext → DbContext),
it is the DbContext at the root that actually connects and talks to your database.*/

namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequestStatus : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}