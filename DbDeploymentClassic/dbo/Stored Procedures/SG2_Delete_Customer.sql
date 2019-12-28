

CREATE Procedure [dbo].[SG2_Delete_Customer]
@riCustomerId int,
@riSocialProfileId Int

 
As  
Begin

 -- Searches for Customers based on given parameters  
DELETE FROM [dbo].[SG2_SocialProfile_ProxyMapping] where [SocialProfileId]=@riSocialProfileId


UPDATE [dbo].[SG2_SocialProfile] set 

JVBoxStatusId=Case when JVBoxStatusId IS not null then 35 ELSE NULL END,
[JVBoxId]=Case when JVBoxStatusId is null then NULL else JVBoxId  END
,[StatusId]=18
 where [SocialProfileId]=@riSocialProfileId
 AND [CustomerId]=@riCustomerId

 return 1
     
End

