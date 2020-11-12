// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileName.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Your name"/>
// --------------------------------------------------------------------------------------------------------------------
namespace payrollServiceMultiThreading
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EmployeeDetails
    {
        public int empId { get; set; }
        public string empName { get; set; }
        public char gender { get; set; }
        public string phoneNumber { get; set; }
        public DateTime startDate { get; set; }

        public PayrollDetails payroll = new PayrollDetails();
        public class EmpAddress
        {
            public string street { get; set; }
            public string city { get; set; }
            public string state { get; set; }
        }
        public EmpAddress empAddress = new EmpAddress();
        public List<int> deptid = new List<int>();
    }
}
