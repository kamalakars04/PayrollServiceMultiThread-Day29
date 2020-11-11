﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileName.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Your name"/>
// --------------------------------------------------------------------------------------------------------------------
namespace payrollServiceMultiThreading
{
    using System;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            // Arrange
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'M';
            employeeDetails.empName = "RajKumar";
            employeeDetails.PayrollId = 1;
            employeeDetails.phoneNumber = "7412589633";
            employeeDetails.startDate = new System.DateTime(2017, 08, 26);
            employeeDetails.empAddress.city = "trivandrum";
            employeeDetails.empAddress.state = "Kerala";
            employeeDetails.empAddress.street = "nagar";
            employeeDetails.deptid.Add(1);
            employeeList.Add(employeeDetails);

            // Second employee
            employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'F';
            employeeDetails.empName = "Rani";
            employeeDetails.PayrollId = 1;
            employeeDetails.phoneNumber = "8569236458";
            employeeDetails.startDate = new System.DateTime(2017, 05, 26);
            employeeDetails.empAddress.city = "Hyderabad";
            employeeDetails.empAddress.state = "Telangana";
            employeeDetails.empAddress.street = "This nagar";
            employeeDetails.deptid.Add(3);
            employeeList.Add(employeeDetails);

            // UC 1 Add Multiple employees
            PayrollService payrollService = new PayrollService();
            bool actual = payrollService.AddEmployee(employeeList);
        }
    }
}
