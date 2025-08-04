using AutoMapper;
using LeaveManagementSystem.Application.Models.LeaveTypes;

namespace LeaveManagementSystem.Application.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile : Profile
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
