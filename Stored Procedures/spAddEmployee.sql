USE [Payroll_Service]
GO

/****** Object:  StoredProcedure [dbo].[AddEmployee]    Script Date: 06-11-2020 20:45:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddEmployee]
	@EmpName varchar(50),
	@Gender varchar(1),
	@PhoneNumber varchar(10),
	@PayrollId int ,
	@start_date datetime,
	@street varchar(30),
	@city varchar(30),
	@state varchar(30),
	@empid int = null,
	@deptid int = null
as
begin
begin try
begin transaction
	insert into EmployeeDetails values 
	(@EmpName,@gender,@PhoneNumber,@PayrollId,@start_date);
	select @empid = empid  from EmployeeDetails where empname = @EmpName and Gender = @Gender and PhoneNumber = @PhoneNumber
	and start_date = @start_date;

	insert into addresses values (@empid,@street,@city,@state);
	if(@deptid is not null)
		insert into dept_emp values (@empid,@deptid);
commit transaction;
return -1;
end try
begin catch
rollback transaction;
end catch
end
GO

