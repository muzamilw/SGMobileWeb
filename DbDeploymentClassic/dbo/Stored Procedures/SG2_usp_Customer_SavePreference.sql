

CREATE Procedure [dbo].[SG2_usp_Customer_SavePreference]
	@iSocialProfileId   int=null,
    @rvcPreference1		nvarchar(255) ,
	@rvcPreference2		nvarchar(255) ,
	@rvcPreference3		nvarchar(255) ,
	@rvcPreference4		nvarchar(255) ,
	@iPreference5		int,
	@iPreference6		int,
	@iPreference7		int,
	@rvcPreference8		nvarchar(255) ,
	@rvcPreference9		nvarchar(255) ,
	@rvcPreference10	Int,
	@rvcInstaUser		nvarchar(255) ,
	@rvcInstaPassword	nvarchar(255),
	@rvcCity            int= null,
	@rvcSocialProfileName nvarchar(255),
	@iCustomerId        int,
	@riStatusQueueId smallint= null,
	@riAITargeting   int=2,
	@riSocialAccAs int=2
As  
 
 
Begin

If(@iSocialProfileId IS NULL)
Begin
	Insert into [dbo].[SG2_SocialProfile]
	(
		[CustomerId],
		[SocialProfileTypeId],
		[StatusId],
		[SocialProfileName],
		[CreatedOn],
		[CreatedBy],
		[UpdatedOn],
		[UpdatedBy]
	)
	SELECT 
		@iCustomerId,
		null,
		19,
		@rvcSocialProfileName,
		getdate(),
		(Select Concat(C.Firstname, c.Surname) From SG2_Customer C Where C.CustomerId = @iCustomerid),
		getdate(),
		(Select Concat(C.Firstname, c.Surname) From SG2_Customer C Where C.CustomerId = @iCustomerid)

	SELECT @iSocialProfileId = @@IDENTITY
End

If not exists(Select 1 From [dbo].[SG2_SocialProfile_TargetingInformation] where [SocialProfileId]=@iSocialProfileId  ) 
	Begin
		INSERT INTO [dbo].[SG2_SocialProfile_TargetingInformation]
           ([SocialProfileId]
           ,[Preference1]
           ,[Preference2]
           ,[Preference3]
           ,[Preference4]
           ,[Preference5]
           ,[Preference6]
           ,[Preference7]
           ,[Preference8]
           ,[Preference9]
		   ,CreatedOn,
			UpdatedOn,
			[QueueStatus],
			Preference10,
			SocialAccAs
		   )
		VALUES
		 (
			@iSocialProfileId,
			@rvcPreference1,
			@rvcPreference2,
			@rvcPreference3,	
			@rvcPreference4,
			@iPreference5,
			@iPreference6,
			@iPreference7,
			@rvcPreference8,	
			@rvcPreference9,
			getdate(),
			GETDATE(),
			@riStatusQueueId,
			@riAITargeting,
			@riSocialAccAs
		)
	END
	Else
		Begin
			UPDATE [dbo].[SG2_SocialProfile_TargetingInformation]
			   SET [Preference1] =  @rvcPreference1,
				 [Preference2] =   @rvcPreference2,
				 [Preference3] =   @rvcPreference3,
				 [Preference4] =   @rvcPreference4,
				 [Preference5] =   @iPreference5,
				 [Preference6] =   @iPreference6,
				 [Preference7] =   @iPreference7,
				 [Preference8] =   @rvcPreference8,
				 [Preference9] =   @rvcPreference9,
				 Preference10  =   @riAITargeting,
				 [QueueStatus] =   @riStatusQueueId,
				 UpdatedOn=GETDATE(),
				 SocialAccAs=@riSocialAccAs
			 WHERE [SocialProfileId]=@iSocialProfileId
	END
if @rvcSocialProfileName is not null
Begin
	Update 
		[dbo].[SG2_SocialProfile]
		SET 
			[SocialProfileName] = @rvcSocialProfileName,
			--[SocialUsername]=@rvcInstaUser, 
		--	[SocialPassword]=@rvcInstaPassword,
		--	[SocialPrefferedCity]=@rvcCity,
			--[SocialPrefferedCountry]=@rvcPreference10, 
			UpdatedOn=GETDATE()
		Where [SocialProfileId]=@iSocialProfileId
END
		SELECT [SocialProfileId],
			   [Preference1]
			  ,[Preference2]
			  ,[Preference3]
			  ,[Preference4]
			  ,[Preference5]
			  ,[Preference6]
			  ,[Preference7]
			  ,[Preference8]
			  ,[Preference9]
			  ,[Preference10]
		  FROM [dbo].[SG2_SocialProfile_TargetingInformation] 
		  Where[SocialProfileId]=@iSocialProfileId
         
End

