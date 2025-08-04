using System.ComponentModel.DataAnnotations.Schema;

namespace LeaveManagementSystem.Data
{
    public class LeaveType : BaseEntity
    {
        //if you use "Id" it will automatically become your primary key


        [Column(TypeName = "nvarchar(150)")]
        public string Name { get; set; }

        public int NumberOfDays { get; set; }

        public List<LeaveAllocation> LeaveAllocations { get; set; }
    }
}
