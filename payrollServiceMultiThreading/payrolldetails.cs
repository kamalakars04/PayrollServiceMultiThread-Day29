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

    public class PayrollDetails
    {
        public int PayrollId { get; set; }
        public int BasePay { get; set; }
        public int Deductions { get; set; }
        public int TaxablePay { get; set; }
        public int IncomeTax { get; set; }
        public int NetPay { get; set; }
    }
}
