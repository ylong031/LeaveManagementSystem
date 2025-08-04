using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LeaveManagementSystem.Data.Configurations
{
    public class IdentityRoleUserConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
            new IdentityUserRole<string>
            {
                RoleId = "6114021a-4c3c-4052-bebd-ee35e7002e67", // Administrator Role
                UserId = "371fde73-4b3e-4038-8c02-f68bcbf32497" // Admin User
            } //ManyToMany Relationship join table      
            );
        }
    }
}
