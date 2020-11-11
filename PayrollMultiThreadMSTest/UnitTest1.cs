using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using payrollServiceMultiThreading;

namespace PayrollMultiThreadMSTest
{
    [TestClass]
    public class UnitTest1
    {
        PayrollService payrollService;

       [TestInitialize]
        public void setup()
        {
            payrollService = new PayrollService();
        }

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
    }
}
