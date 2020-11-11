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
	DBCC Checkident('employeedetails', Reseed)
	insert into EmployeeDetails values 
	(@EmpName,@gender,@PhoneNumber,@PayrollId,@start_date,1);
	select @empid = empid  from EmployeeDetails where empname = @EmpName and Gender = @Gender and PhoneNumber = @PhoneNumber
	and start_date = @start_date and _active = 1;

	insert into addresses values (@empid,@street,@city,@state);
	if(@deptid is not null)
		insert into dept_emp values (@empid,@deptid);
commit transaction;
return 1;
end try
begin catch
rollback transaction;
end catch
end