﻿using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile:Profile
    {
        public LeaveTypeAutoMapperProfile() 
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();

            CreateMap<LeaveTypeCreateVM, LeaveType>();
           
            //Two way mapping
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();

        }

    }
}
