
CREATE Procedure [dbo].[SG2_usp_Customer_GetPendingSocialProfiles]
@riStatusId int

As
Begin	
	Select
		[TargetingInformationId]
	   ,[Preference1]
       ,[Preference2]
       ,[Preference3]
       ,[Preference4]
       ,[Preference5]
       ,[Preference6]
       ,[Preference7]
       ,[Preference8]
       ,[Preference9]
	   ,[Preference10]
	   ,SP.SocialProfileId
	   ,SP.SocialProfileName
	   ,SP.SocialUsername
	   ,JV.ExchangeName
	   ,JV.JVBoxId as JVServerId
	   
	FROM  [dbo].[SG2_SocialProfile_TargetingInformation] CTI
		INNER JOIN [dbo].[SG2_SocialProfile] SP 
			ON CTI.[SocialProfileId]=SP.[SocialProfileId]
		INNER JOIN [dbo].[SG2_Customer] C 
			ON C.[CustomerId]=SP.[CustomerId]
			Left Join [dbo].[SG2_JVBox] JV ON JV.JVBoxId=SP.JVBoxId
			Where CTI.QueueStatus=@riStatusId


		
End

