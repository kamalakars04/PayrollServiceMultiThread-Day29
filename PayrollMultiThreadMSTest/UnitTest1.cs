// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileName.cs" company="Bridgelabz">
//   Copyright © 2018 Company
// </copyright>
// <creator Name="Your name"/>
// --------------------------------------------------------------------------------------------------------------------
namespace PayrollMultiThreadMSTest
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using payrollServiceMultiThreading;

    [TestClass]
    public class UnitTest1
    {
        PayrollService payrollService;

       [TestInitialize]
        public void setup()
        {
            payrollService = new PayrollService();
        }

        /// <summary>
        /// TC 1 Adds the multiple employee.
        /// </summary>
        [TestMethod]
        public void AddMultipleEmployee()
        {
            // Arrange
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails =new EmployeeDetails();
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

            // Act
            bool actual = payrollService.AddEmployee(employeeList);
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// TC 2 Adds the multiple employee with threads.
        /// </summary>
        [TestMethod]
        public void AddMultipleEmployeeWithThreads()
        {
            // Arrange
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'M';
            employeeDetails.empName = "Charan";
            employeeDetails.PayrollId = 3;
            employeeDetails.phoneNumber = "8528528528";
            employeeDetails.startDate = new System.DateTime(2013, 08, 26);
            employeeDetails.empAddress.city = "Kochi";
            employeeDetails.empAddress.state = "Kerala";
            employeeDetails.empAddress.street = "That nagar";
            employeeDetails.deptid.Add(2);
            employeeList.Add(employeeDetails);

            // Second employee
            employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'F';
            employeeDetails.empName = "Sahana";
            employeeDetails.PayrollId = 2;
            employeeDetails.phoneNumber = "4564564564";
            employeeDetails.startDate = new System.DateTime(2013, 05, 26);
            employeeDetails.empAddress.city = "Chennai";
            employeeDetails.empAddress.state = "TamilNadu";
            employeeDetails.empAddress.street = "JN nagar";
            employeeDetails.deptid.Add(3);
            employeeList.Add(employeeDetails);

            // Act
            bool actual = payrollService.AddEmployeeWithThreads(employeeList);
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
