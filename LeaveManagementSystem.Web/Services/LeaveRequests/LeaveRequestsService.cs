using AutoMapper;
using Azure.Core;
using Humanizer;
using LeaveManagementSystem.Web.Data;
using LeaveManagementSystem.Web.Models.LeaveRequests;
using LeaveManagementSystem.Web.Services.LeaveAllocations;
using LeaveManagementSystem.Web.Services.Periods;
using LeaveManagementSystem.Web.Services.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SQLitePCL;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LeaveManagementSystem.Web.Services.LeaveRequests
{
    public partial class LeaveRequestsService(IMapper _mapper,
        
        ApplicationDbContext _context,
        IPeriodsService _periodsService,
        ILeaveAllocationsService _leaveAllocationsService,
        IUserService _userService) : ILeaveRequestsService
    {
        //for employee to cancel a leave request
        public async Task CancelLeaveRequest(int leaveRequestId)
        {
            var leaveRequest=await _context.LeaveRequests
                .FirstAsync(q => q.Id == leaveRequestId);

            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Cancelled;


            await UpdateAllocationDays(leaveRequest, false);
            await _context.SaveChangesAsync();
        }


        //leaveRequestCreateVM(StartDate,EndDate,LeaveTypeId,RequestComments)

        //for employee to create a leave request
        public async Task CreateLeaveRequest(LeaveRequestCreateVM model)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(model);

            
            /* 
              
            To create a LeaveRequest, provide the foreign key IDs for the related entities.  
            The navigation properties will be handled by Entity Framework when you query the object later.
          
            ID: Use when referencing or linking entities, especially for database operations or when the client needs to select an entity.
            Object/Descriptive Property: Use in view models/DTOs for displaying information or when you want to show more details to the user.
            
             */

            var user = await _userService.GetLoggedInUser();
            leaveRequest.EmployeeId = user.Id;
            leaveRequest.LeaveRequestStatusId = (int)LeaveRequestStatusEnum.Pending;
            _context.LeaveRequests.Add(leaveRequest);

            await UpdateAllocationDays(leaveRequest, true);

            await _context.SaveChangesAsync();
        }



        //for employee to see all his leave requests
        public async Task<List<LeaveRequestReadOnlyVM>> GetEmployeeLeaveRequestsHistory()
        {
            var user = await _userService.GetLoggedInUser();
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == user.Id)
                .ToListAsync();

            
            /*   
               
            Maps each LeaveRequest entity to a LeaveRequestReadOnlyVM view model.    
            Calculates the number of days taken for each leave request 
            by subtracting the DayNumber property of start and end dates.
            Casts the numeric LeaveRequestStatusId to the LeaveRequestStatusEnum.*/

            /*
            You are using manual mapping mainly because you have custom logic(enum conversion, calculations, nested properties).
            */

            var model = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,


            }).ToList();
            return model;
        }

        public async Task<bool> RequestDatesExceedAllocation(LeaveRequestCreateVM model)
        {
            var user = await _userService.GetLoggedInUser();

            var period = await _periodsService.GetCurrentPeriod();



            var numberOfDays = model.EndDate.DayNumber - model.StartDate.DayNumber;
            var allocation = await _leaveAllocationsService.GetCurrentAllocation(model.LeaveTypeId, user.Id);

            return allocation.Days < numberOfDays;
        }


        //for admin to get all leave requests submitted by all employees
        public async Task<EmployeeLeaveRequestListVM> AdminGetAllLeaveRequests()
        {
            var leaveRequests = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .Include(q => q.Employee)
                .ToListAsync();

            var leaveRequestModels = leaveRequests.Select(q => new LeaveRequestReadOnlyVM
            {
                StartDate = q.StartDate,
                EndDate = q.EndDate,
                Id = q.Id,
                LeaveType = q.LeaveType.Name,
                LeaveRequestStatus = (LeaveRequestStatusEnum)q.LeaveRequestStatusId,
                NumberOfDays = q.EndDate.DayNumber - q.StartDate.DayNumber,
                Employee = $"{q.Employee.FirstName} {q.Employee.LastName}"
            }).ToList();

            var model = new EmployeeLeaveRequestListVM
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Approved),
                PendingRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Pending),
                DeclinedRequests = leaveRequests.Count(q => q.LeaveRequestStatusId == (int)LeaveRequestStatusEnum.Declined),
                LeaveRequests =leaveRequestModels
            };
            return model;
        }

        // This method retrieves a specific leave request for review by its ID.
        public async Task<ReviewLeaveRequestVM> GetLeaveRequestForReview(int id)
        {
            var leaveRequest = await _context.LeaveRequests
                .Include(q => q.LeaveType)
                .FirstAsync(q => q.Id == id);

            var user=await _userService.GetUserById(leaveRequest.EmployeeId);

            var model=new ReviewLeaveRequestVM
            {
             
                StartDate = leaveRequest.StartDate,
                EndDate = leaveRequest.EndDate,
                NumberOfDays = leaveRequest.EndDate.DayNumber - leaveRequest.StartDate.DayNumber,
                LeaveRequestStatus = (LeaveRequestStatusEnum)leaveRequest.LeaveRequestStatusId,
                Id = leaveRequest.Id,
                LeaveType = leaveRequest.LeaveType.Name,
                RequestComments = leaveRequest.RequestComments,
                Employee =new EmployeeListVM
                {
                    Id = leaveRequest.EmployeeId,
                    Email=user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                }
            };
            return model;
        }


        // This method reviews a leave request by its ID and updates its status to either approved or declined.
        public async Task ReviewLeaveRequest(int leaveRequestId, bool approved)
        {
            var user = await _userService.GetLoggedInUser();

            var leaveRequest = await _context.LeaveRequests.FindAsync(leaveRequestId);

            leaveRequest.LeaveRequestStatusId = approved
                ? (int)LeaveRequestStatusEnum.Approved
                : (int)LeaveRequestStatusEnum.Declined;

            leaveRequest.ReviewerId = user.Id;
            if (!approved)
            {


                await UpdateAllocationDays(leaveRequest, false);

            }
            await _context.SaveChangesAsync();
        }

        private async Task UpdateAllocationDays(LeaveRequest leaveRequest,bool deductDays)
        {
            var allocation=await _leaveAllocationsService.GetCurrentAllocation
                (leaveRequest.LeaveTypeId,leaveRequest.EmployeeId);
            var numberOfDays=CalculateDays(leaveRequest.StartDate, leaveRequest.EndDate);
            if (deductDays)
            {
                allocation.Days -= numberOfDays;
            }
            else
            {
                allocation.Days += numberOfDays;
            }
            _context.Entry(allocation).State = EntityState.Modified;
        }

        private int CalculateDays(DateOnly start, DateOnly end)
        {
            return end.DayNumber - start.DayNumber;
        }
    }
}
   