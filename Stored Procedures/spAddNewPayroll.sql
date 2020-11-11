USE [Payroll_Service]
GO

/****** Object:  StoredProcedure [dbo].[AddNewPayroll]    Script Date: 06-11-2020 20:47:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddNewPayroll]
	@payrollid int output,
	@Basepay int,
	@Deductions int
AS
begin 
begin try
Insert into PayrollDetails values (@payrollid,@Basepay,@Deductions)
return @payrollid
end try
begin catch
select @payrollid = payrollId from PayrollDetails where basepay = @Basepay and Deductions =@Deductions
return @payrollid
end catch
end
GO

