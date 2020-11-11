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
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;

    public class PayrollService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static string connectionString = @"Server=LAPTOP-CTKSHLKD\SQLEXPRESS; Initial Catalog =payroll_service; User ID = sa; Password=kamal@99";

        /// <summary>
        /// Gets the connection.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            // Create connection string 
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        /// <summary>
        /// Adds the employee.
        /// </summary>
        /// <param name="emp">The emp.</param>
        /// <returns></returns>
        public bool AddEmployee(EmployeeDetails emp)
        {
            // open connection and create transaction
            SqlConnection connection =  GetConnection();
            connection.Open();
            try
            {
                int result = 0;
                Console.WriteLine($"Adding Employee {emp.empName}");

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
                

                // Parallel concept if dept is given
                Parallel.ForEach(emp.deptid, dept =>
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@deptid";
                    parameter.SqlDbType = System.Data.SqlDbType.Int;
                    command.Parameters.Add(parameter);
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
                Console.WriteLine($"Added Employee {emp.empName}");
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
            }
        }

        /// <summary>
        /// UC 1 Adds the employee.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool AddEmployee(List<EmployeeDetails> employeeDetails)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach(EmployeeDetails employee in employeeDetails)
            {
                bool result = AddEmployee(employee);
                if (result == false)
                    return false;
            }
            stopwatch.Stop();
            Console.WriteLine("Time taken without threads is :{0} ",stopwatch.ElapsedMilliseconds);
            return true;
        }

        /// <summary>
        /// UC 2 Adds the employee with threads.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool AddEmployeeWithThreads(List<EmployeeDetails> employeeDetails)
        {
            bool result = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread[] thread = new Thread[employeeDetails.Count];
            int i = 0;
            foreach (EmployeeDetails employee in employeeDetails)
            {
                // Store all the threads
                thread[i++] =  new Thread(() => 
                {
                    EmployeeDetails employeeInstance = new EmployeeDetails();
                    employeeInstance = employee;
                    result = AddEmployee(employeeInstance);
                });
            }
            // Start all the threads
            for (i = 0; i < thread.Length; i++)
                thread[i].Start();

            // Let the main program wait untill all the threads are finished
            for (i = 0; i < thread.Length; i++)
            {
                thread[i].Join();
            }
            stopwatch.Stop();
            Console.WriteLine("Time taken with threads is :{0} ", stopwatch.ElapsedMilliseconds);
            return true;
        }
    }
}
