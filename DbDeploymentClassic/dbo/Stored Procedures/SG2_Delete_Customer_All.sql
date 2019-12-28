  
  
CREATE Procedure [dbo].[SG2_Delete_Customer_All]  
@riCustomerId int,  
@riSocialProfileId Int  
  
   
As    
Begin  
  
IF @riSocialProfileId =0   
BEGIN   
 -- Searches for Customers based on given parameters    

        
DELETE TI FROM [dbo].[SocialProfile_Instagram_TargetingInformation] TI Inner join [dbo].[SocialProfile]  SP  
     ON TI.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
DELETE SST  
 FROM [dbo].[SocialProfile_Statistics] SST Inner join [dbo].[SocialProfile]  SP  
     ON SST.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
  
DELETE SS  
FROM [dbo].[SocialProfile_Payments] SS Inner join [dbo].[SocialProfile]  SP  
     ON SS.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
DELETE SPSH  
FROM [dbo].[SocialProfile_StatusHistory] SPSH Inner join [dbo].[SocialProfile]  SP  
     ON SPSH.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
  
  
DELETE FROM [dbo].[SocialProfile]   
 Where [CustomerId]=@riCustomerId  
  
DELETE FROM  [dbo].[Customer_ContactDetail]  Where [CustomerId]=@riCustomerId  
DELETE FROM [dbo].[Customer] Where [CustomerId]=@riCustomerId   
END   
  
ELSE  
  
BEGIN   
  

        
DELETE TI FROM [dbo].[SocialProfile_Instagram_TargetingInformation] TI  
      Where TI.SocialProfileId=@riSocialProfileId  
DELETE SST  
 FROM [dbo].[SocialProfile_Statistics] SST   
      Where SST.SocialProfileId=@riSocialProfileId  
  
DELETE SS  
FROM [dbo].[SocialProfile_Payments] SS  
      Where SS.SocialProfileId=@riSocialProfileId  
DELETE SPSH  
FROM [dbo].[SocialProfile_StatusHistory] SPSH   
      Where SPSH.SocialProfileId=@riSocialProfileId  
  
  
DELETE FROM [dbo].[SocialProfile]   
 Where SocialProfileId=@riSocialProfileId  
  
END  
  
SELECT 1  
       
End  
  