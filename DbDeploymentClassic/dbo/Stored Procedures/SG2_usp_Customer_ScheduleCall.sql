

CREATE Procedure [dbo].[SG2_usp_Customer_ScheduleCall]
@riCustomerId int,
@rdtScheduleDate datetime,
@rvcTest Nvarchar(max)

 
As  
Begin

Update [dbo].[SG2_Customer_ContactDetail]
		Set [ScheduleCallDate]=@rdtScheduleDate,
     [Notes]=@rvcTest
	 Where [CustomerId]=@riCustomerId

Select 1

End

