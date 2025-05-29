using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
            /*
           LeaveType is data model
           IndexVM is view model
            
           This will create a mapping configuration that
           maps properties from LeaveType to IndexVM.
            */

            CreateMap<LeaveTypeCreateVM, LeaveType>();
            /*And then I have to take this data,
             convert it into the data model and then send it to the database.*/

            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();

            CreateMap<LeaveTypeEditVM, LeaveType>();


        }

    }
}
