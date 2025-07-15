using AutoMapper;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveTypes;

namespace LeaveManagementSystem.Web.MappingProfiles
{
    public class LeaveTypeAutoMapperProfile:Profile
    {
        public LeaveTypeAutoMapperProfile() 
        {
            CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
           

            /*And then I have to take this data,
            convert it into the data model and then send it to the database.*/
            CreateMap<LeaveTypeCreateVM, LeaveType>();
           


            //Two way mapping
            CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap();

      


        }

    }
}
