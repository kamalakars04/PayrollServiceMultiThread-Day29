USE [Payroll_Service]
GO

/****** Object:  StoredProcedure [dbo].[GetPayRollDetails]    Script Date: 06-11-2020 20:46:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetPayRollDetails]
	@empId int = null
AS
begin
if(@empid is null)
select ED.empid,PD.* from EmployeeDetails ED inner join PayrollDetails PD on PD.payrollId = ED.PayrollId where ED._active = 1
else
select ED.empid,PD.* from EmployeeDetails ED inner join PayrollDetails PD on PD.payrollId = ED.PayrollId where ED.EmpID = @empId and ED._active =1
end
GO

