﻿namespace LeaveManagementSystem.Web.Models.Period
{
    public class PeriodVM
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
