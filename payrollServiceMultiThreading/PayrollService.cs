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
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Text;
    using System.Threading.Tasks;
    using NLog;

    public class PayrollService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        // Create connection string 
        public static string connectionString = @"Server=LAPTOP-CTKSHLKD\SQLEXPRESS; Initial Catalog =payroll_service; User ID = sa; Password=kamal@99";
        SqlConnection connection = new SqlConnection(connectionString);

        public bool AddEmployee(EmployeeDetails emp)
        {
            // open connection and create transaction
            connection.Open();
            try
            {
                int result = 0;

                // create a new command in transaction
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "dbo.addEmployee";
                command.Parameters.AddWithValue("@EmpName", emp.empName);
                command.Parameters.AddWithValue("@gender", emp.gender);
                command.Parameters.AddWithValue("@PhoneNumber", emp.phoneNumber);
                command.Parameters.AddWithValue("@PayrollId", emp.PayrollId);
                command.Parameters.AddWithValue("@start_date", emp.startDate);
                command.Parameters.AddWithValue("@street", emp.empAddress.street);
                command.Parameters.AddWithValue("@city", emp.empAddress.city);
                command.Parameters.AddWithValue("@state", emp.empAddress.state);
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@deptid";
                parameter.SqlDbType = System.Data.SqlDbType.Int;
                command.Parameters.Add(parameter);

                // Parallel concept if dept is given
                Parallel.ForEach(emp.deptid, dept =>
                {
                       parameter.Value = dept;

                       // Execute command
                       result = command.ExecuteNonQuery();
                });

                // If dept is not given add as a single contact
                if(emp.deptid.Count == 0)
                {
                    // Execute command
                    result = command.ExecuteNonQuery();
                }
            connection.Close();
            if (result == 0)
                    return false;
                return true;
        }
            catch
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        public bool AddEmployee(List<EmployeeDetails> employeeDetails)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach(EmployeeDetails employee in employeeDetails)
            {
                Console.WriteLine($"Adding Employee {employee.empName}");
                bool result = AddEmployee(employee);
                if (result == false)
                    return false;
                Console.WriteLine($"Added Employee {employee.empName}");
            }
            stopwatch.Stop();
            Console.WriteLine("Time taken without threads is :{0} ",stopwatch.ElapsedMilliseconds);
            return true;
        }
    }
}
