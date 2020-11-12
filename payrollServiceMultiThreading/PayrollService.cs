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

        // UC 3 Locking the object for synchronization of threads
        private readonly Object empTableLocker = new object();
        private readonly Object payrollTableLocker = new object();


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
            SqlConnection connection = GetConnection();
            try
            {
                // open connection and create transaction
                connection.Open();
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
                command.Parameters.AddWithValue("@PayrollId", emp.payroll.PayrollId);
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
                if (emp.deptid.Count == 0)
                {
                    // Execute command
                    result = command.ExecuteNonQuery();
                }

                connection.Close();
                if (result == 0)
                {
                    Console.WriteLine($"Failed to add employee {emp.empName}");
                    Trace.WriteLine("Failed to add employee {0}", emp.empName);
                    return false;
                }
                Console.WriteLine($"Added Employee {emp.empName}");
                Trace.WriteLine("added employee {0}", emp.empName);
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
                Trace.Flush();
            }

        }

        /// <summary>
        /// UC 5 Adds the employee with synchronization to both payrollDetails and employee table
        /// </summary>
        /// <param name="emp">The emp.</param>
        /// <returns></returns>
        public bool AddEmployeeWithSynchronization(EmployeeDetails emp)
        {
            SqlConnection connection = GetConnection();
            SqlTransaction transaction = null;
            try
            {
                // open connection and create transaction
                connection.Open();
                transaction = connection.BeginTransaction();
                int result = 0;
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.Transaction = transaction;

                lock (payrollTableLocker)
                {
                    // If new employee payroll details are given then add them
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "dbo.addNewPayroll";
                    command.Parameters.AddWithValue("@basepay", emp.payroll.BasePay);
                    command.Parameters.AddWithValue("@deductions", emp.payroll.Deductions);
                    SqlParameter returnvalue = new SqlParameter();
                    returnvalue.Direction = System.Data.ParameterDirection.InputOutput;
                    returnvalue.DbType = System.Data.DbType.Int32;
                    returnvalue.ParameterName = "@payrollid";
                    returnvalue.Value = emp.payroll.PayrollId;
                    command.Parameters.Add(returnvalue);
                    command.ExecuteScalar();
                    Trace.WriteLine("AddPayroll Successfull");
                    emp.payroll.PayrollId = (int)returnvalue.Value;
                }

                lock (empTableLocker)
                {
                    Console.WriteLine($"Adding Employee {emp.empName}");

                    // create a new command in transaction
                    command.Parameters.Clear();
                    command.CommandText = "dbo.addEmployee";
                    command.Parameters.AddWithValue("@EmpName", emp.empName);
                    command.Parameters.AddWithValue("@gender", emp.gender);
                    command.Parameters.AddWithValue("@PhoneNumber", emp.phoneNumber);
                    command.Parameters.AddWithValue("@PayrollId", emp.payroll.PayrollId);
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
                    if (emp.deptid.Count == 0)
                    {
                        // Execute command
                        result = command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    connection.Close();
                    Console.WriteLine($"Added Employee {emp.empName}");
                    Trace.WriteLine("added employee {0}", emp.empName);
                    return true;
                }
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                return false;
            }
            finally
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    connection.Close();
                Trace.Flush();
            }

        }

        /// <summary>
        /// UC 1 Adds the employee.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool AddMultipleEmployee(List<EmployeeDetails> employeeDetails)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (EmployeeDetails employee in employeeDetails)
            {
                bool result = AddEmployee(employee);
                if (result == false)
                    return false;
            }
            stopwatch.Stop();
            Console.WriteLine("Time taken without threads is :{0} ", stopwatch.ElapsedMilliseconds);
            return true;
        }

        /// <summary>
        /// UC 2 Adds the employee with threads.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool AddMultipleEmployeeWithThreads(List<EmployeeDetails> employeeDetails)
        {
            bool result = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread[] thread = new Thread[employeeDetails.Count];
            int i = 0;
            foreach (EmployeeDetails employee in employeeDetails)
            {
                // Store all the threads
                thread[i++] = new Thread(() =>
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

        /// <summary>
        /// UC 5 Adds the employee with threads.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool AddMultipleEmployeeWithThreadsAndSynchronisation(List<EmployeeDetails> employeeDetails)
        {
            bool result = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread[] thread = new Thread[employeeDetails.Count];
            int i = 0;
            foreach (EmployeeDetails employee in employeeDetails)
            {
                // Store all the threads
                thread[i++] = new Thread(() =>
                {
                    EmployeeDetails employeeInstance = new EmployeeDetails();
                    employeeInstance = employee;
                    result = AddEmployeeWithSynchronization(employeeInstance);
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


        /// <summary>
        /// UC 5 Adds to payroll table.
        /// </summary>
        /// <param name="payrollDetails">The payroll details.</param>
        /// <returns></returns>
        public int AddToPayrollTable(PayrollDetails payrollDetails)
        {
            lock (payrollTableLocker)
            {
                SqlConnection connection = GetConnection();
                try
                {
                    // open connection and create transaction
                    connection.Open();

                    // create a new command in transaction
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    // Execute command
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "dbo.addNewPayroll";
                    command.Parameters.AddWithValue("@basepay", payrollDetails.BasePay);
                    command.Parameters.AddWithValue("@deductions", payrollDetails.Deductions);
                    SqlParameter returnvalue = new SqlParameter();
                    returnvalue.Direction = System.Data.ParameterDirection.InputOutput;
                    returnvalue.DbType = System.Data.DbType.Int32;
                    returnvalue.ParameterName = "@payrollid";
                    returnvalue.Value = payrollDetails.PayrollId;
                    command.Parameters.Add(returnvalue);
                    command.ExecuteScalar();
                    Trace.WriteLine("AddPayroll Successfull");
                    return (int)returnvalue.Value;
                }
                catch
                {
                    Trace.WriteLine("Error in AddPayroll");
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                    return 0;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                        connection.Close();
                    Trace.Flush();
                }
            }
        }

        /// <summary>
        /// UC 6 Updates the salary of employee.
        /// </summary>
        /// <param name="empid">The empid.</param>
        /// <param name="payrollid">The payrollid.</param>
        /// <param name="basepay">The basepay.</param>
        /// <param name="deduction">The deduction.</param>
        /// <returns></returns>
        public bool UpdateEmployeeSalaryWithSynchronization(UpdateSalary updateSalary)
        {
            // open connection and create transaction
            SqlConnection connection = GetConnection();
            SqlTransaction transaction = null;
            int result = 0;
            
            try
            {
                // create a new command in transaction
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCommand command = new SqlCommand();
                command.Transaction = transaction;
                command.Connection = connection;

                // Execute command
                lock(payrollTableLocker)
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "dbo.UpdatePayroll";
                    command.Parameters.AddWithValue("@basepay", updateSalary.BasePay);
                    command.Parameters.AddWithValue("@deductions", updateSalary.Deductions);
                    SqlParameter returnvalue = new SqlParameter();
                    returnvalue.Direction = System.Data.ParameterDirection.InputOutput;
                    returnvalue.DbType = System.Data.DbType.Int32;
                    returnvalue.ParameterName = "@payrollid";
                    returnvalue.Value = updateSalary.PayrollId;
                    command.Parameters.Add(returnvalue);
                    command.ExecuteScalar();
                }
                
                lock(empTableLocker)
                {
                    // Execute second command
                    command.CommandText = "dbo.UpdateEmployeeColumn";
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@empid", updateSalary.empId);
                    command.Parameters.AddWithValue("@payrollid", returnvalue.Value);
                    result = command.ExecuteNonQuery();
                }
                transaction.Commit();
                connection.Close();
                Trace.WriteLine("Update successful");
                return true;
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();
                Trace.WriteLine("Update Failed");
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

        /// <summary>
        /// UC 6 Updates the multiple employee with threads and synchronisation.
        /// </summary>
        /// <param name="employeeDetails">The employee details.</param>
        /// <returns></returns>
        public bool UpdateMultipleEmployeeWithThreadsAndSynchronisation(List<UpdateSalary> updateSalaries )
        {
            bool result = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Thread[] thread = new Thread[updateSalaries.Count];
            int i = 0;
            foreach (UpdateSalary employee in updateSalaries)
            {
                // Store all the threads
                thread[i++] = new Thread(() =>
                {
                    UpdateSalary employeeInstance = new UpdateSalary();
                    employeeInstance = employee;
                    result = UpdateEmployeeSalaryWithSynchronization(employeeInstance);
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
