using System;
using System.Collections.Generic;
using System.Text;

namespace payrollServiceMultiThreading
{
    public class UpdateSalary
    {
        public int PayrollId { get; set; }
        public int BasePay { get; set; }
        public int Deductions { get; set; }
        public int empId { get; set; }

    }
}
