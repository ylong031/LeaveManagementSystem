﻿
namespace LeaveManagementSystem.Web.Data
{
    public class LeaveRequestStatus : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}