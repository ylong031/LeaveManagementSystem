using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Web.Data.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            
            builder.HasData(
                new IdentityRole
                {
                    /*Why we use GUIDs for IDs: Always Unique
                   GUIDs make sure every role(like "Employee" or "Admin") 
                   has a unique ID that won’t accidentally match something else.*/

                    // NormalizedName is used for case-insensitive comparisons
                    Id = "b15308f5-eba8-43b4-80fe-b885d542014e",
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"

                   
                },

                new IdentityRole
                {
                    Id = "ad0a5c18-926f-46cf-97ef-dd3fa31366a0",
                    Name = "Supervisor",
                    NormalizedName = "SUPERVISOR"

                },
                new IdentityRole
                {
                    Id = "6114021a-4c3c-4052-bebd-ee35e7002e67",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
        }
    }
}
