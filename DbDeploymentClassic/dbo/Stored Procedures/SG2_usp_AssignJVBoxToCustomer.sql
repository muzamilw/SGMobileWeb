CREATE Procedure [dbo].[SG2_usp_AssignJVBoxToCustomer]
  @riCustomerId Int,
  @riProfileId int

As  
Begin

	DECLARE @JVBoxId int

	SELECT 
			@JVBoxId=JVBoxId FROM [dbo].[SG2_SocialProfile] 
		where 
			CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

	DECLARE @JVBoxes table(JVBoxId int, MaxCount int, AssignedCount int, JVBoxStatus int)

	if (
		(@JVBoxId is null) 
		and 
		exists(SELECT 1 from [dbo].[SG2_SocialProfile_Subscription] where [SocialProfileId]= @riProfileId and StatusId=25))
	Begin 
		INSERT INTO @JVBoxes(JVBoxId,MaxCount,AssignedCount,JVBoxStatus)
		SELECT JVBox.JVBoxId,MaxLimit, Count(C.JVBoxId),JVBox.StatusId FROM SG2_JVBox JVBox  
		Left join [dbo].[SG2_SocialProfile] C 
		On JVBox.JVBoxId=C.JVBoxId
		where  JVBox.StatusId=19
		AND ISNULL(JVBox.QueueStatusId,45) <> 44
		Group by JVBox.JVBoxId,MaxLimit,JVBox.StatusId

		IF Exists(Select 1 from @JVBoxes where AssignedCount<MaxCount and JVBoxStatus=19)
		Begin
			SELECT TOP 1 @JVBoxId=JVBoxId from @JVBoxes where AssignedCount<MaxCount order by JVBoxId asc

			Update [dbo].[SG2_SocialProfile] set JVBoxId=@JVBoxId
			where CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

		END
	END

	Select 
		CustomerId, SP.JVBoxId as JVBoxId ,JV.BoxName as JVBoxName 
		FROM [dbo].[SG2_SocialProfile] SP
			Inner JOIN [dbo].[SG2_JVBox] JV ON SP.JVBoxId=JV.JVBoxId
	 where 
		CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

End
