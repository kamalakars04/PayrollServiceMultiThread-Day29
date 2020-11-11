CREATE PROCEDURE [dbo].[spEmployeeDelete]
	@Empid int = null
AS
begin
begin try
if(@Empid is not null)
update EmployeeDetails set _active = 0 where EmpID = @Empid;
else
update EmployeeDetails set _active = 0 ;
return 1;
end try
begin catch
return 0
end catch
end