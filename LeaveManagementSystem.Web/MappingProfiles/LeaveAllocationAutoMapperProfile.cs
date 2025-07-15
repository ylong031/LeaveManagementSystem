﻿using AutoMapper;
using LeaveManagementSystem.Web.Models.LeaveTypes;
using LeaveManagementSystem.Web.Models.LeaveAllocations;
using LeaveManagementSystem.Web.Models.Period;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveAllocationAutoMapperProfile:Profile
    {
        public LeaveAllocationAutoMapperProfile() 
        {
            CreateMap<LeaveAllocation, LeaveAllocationVM>();
            CreateMap<Period, PeriodVM>();
            
            
        
        
        }
    }
}
