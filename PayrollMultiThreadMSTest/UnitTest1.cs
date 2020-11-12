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
            employeeDetails.payroll.PayrollId = 1;
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
            employeeDetails.payroll.PayrollId = 1;
            employeeDetails.phoneNumber = "8569236458";
            employeeDetails.startDate = new System.DateTime(2017, 05, 26);
            employeeDetails.empAddress.city = "Hyderabad";
            employeeDetails.empAddress.state = "Telangana";
            employeeDetails.empAddress.street = "This nagar";
            employeeDetails.deptid.Add(3);
            employeeList.Add(employeeDetails);

            // Act
            bool actual = payrollService.AddMultipleEmployee(employeeList);
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
            employeeDetails.payroll.PayrollId = 3;
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
            employeeDetails.payroll.PayrollId = 2;
            employeeDetails.phoneNumber = "4564564564";
            employeeDetails.startDate = new System.DateTime(2013, 05, 26);
            employeeDetails.empAddress.city = "Chennai";
            employeeDetails.empAddress.state = "TamilNadu";
            employeeDetails.empAddress.street = "JN nagar";
            employeeDetails.deptid.Add(3);
            employeeList.Add(employeeDetails);

            // Act
            bool actual = payrollService.AddMultipleEmployeeWithThreads(employeeList);
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// TC  5 Adds the multiple employee with threads and synchronisation to multiple tables
        /// </summary>
        [TestMethod]
        public void AddMultipleEmployeeWithThreadsAndSynchronisation()
        {
            // Arrange
            List<EmployeeDetails> employeeList = new List<EmployeeDetails>();
            List<PayrollDetails> payrolls = new List<PayrollDetails>();
            EmployeeDetails employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'M';
            employeeDetails.empName = "Sriraj";
            employeeDetails.payroll.PayrollId = 5;
            employeeDetails.payroll.BasePay = 90000;
            employeeDetails.payroll.Deductions = 1000;
            employeeDetails.phoneNumber = "8528528528"; 
            employeeDetails.startDate = new System.DateTime(2013, 08, 26);
            employeeDetails.empAddress.city = "karimnagar";
            employeeDetails.empAddress.state = "Telangana";
            employeeDetails.empAddress.street = "BackStreet";
            employeeDetails.deptid.Add(2);
            PayrollDetails payroll = new PayrollDetails() { BasePay = 90000, Deductions = 1000, PayrollId = 5 };
            employeeList.Add(employeeDetails);

            // Second employee
            employeeDetails = new EmployeeDetails();
            employeeDetails.gender = 'F';
            employeeDetails.empName = "Valli";
            employeeDetails.payroll.PayrollId = 6;
            employeeDetails.payroll.BasePay = 9000;
            employeeDetails.payroll.Deductions = 1000;
            employeeDetails.phoneNumber = "4564564564";
            employeeDetails.startDate = new System.DateTime(2012, 05, 26);
            employeeDetails.empAddress.city = "panaji";
            employeeDetails.empAddress.state = "Goa";
            employeeDetails.empAddress.street = "Gn nagar";
            employeeDetails.deptid.Add(3);
            employeeList.Add(employeeDetails);

            // Act
            bool actual = payrollService.AddMultipleEmployeeWithThreadsAndSynchronisation(employeeList);
            bool expected = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// UC 6 Updates the multiple employee with threads and synchronisation.
        /// </summary>
        [TestMethod]
        public void UpdateMultipleEmployeeWithThreadsAndSynchronisation()
        {
            // Arrange
            UpdateSalary updateSalary = new UpdateSalary() { empId = 1, PayrollId = 1, BasePay = 75000, Deductions = 6000 };
            List<UpdateSalary> updateSalaries = new List<UpdateSalary> { updateSalary };
            updateSalary = new UpdateSalary() { empId = 2, PayrollId = 3, BasePay = 150000, Deductions = 6000 };
            updateSalaries.Add(updateSalary);

            // Act
            bool actual = payrollService.UpdateMultipleEmployeeWithThreadsAndSynchronisation(updateSalaries);
            bool expected = true;

            // Assert
            Assert.AreEqual(actual, expected);

        }


    }
}
