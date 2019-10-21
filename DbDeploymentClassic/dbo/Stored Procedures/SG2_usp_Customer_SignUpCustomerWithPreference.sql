  
CREATE Procedure [dbo].[SG2_usp_Customer_SignUpCustomerWithPreference]  
 @rvcFirstName   Nvarchar(50),  
    @rvcLastName   Nvarchar(50),  
    @rvcEmailAddress  Nvarchar(64),  
    @rvcPassword   Nvarchar(64),  
 @rvcGUID    Nvarchar(50),  
 @rvcLastLoginIP   Nvarchar(20),  
    @rvcPreference1   nvarchar(255) ,  
 @rvcPreference2   nvarchar(255) ,  
 @rvcPreference3   nvarchar(255) ,  
 @rvcPreference4   nvarchar(255) ,  
 @iPreference5   int,  
 @iPreference6   int,  
 @rvcCity          int= null,  
 @rvcStatusId   Int=1  
As    
   
  -- Searches for Customers based on given parameters    
 Declare @iCustomerId int,@iSocialProfileId int  
 If not exists(Select 1 From dbo.[Customer] where CustomerId= @iCustomerId )   
 Begin  
  
  INSERT INTO [dbo].[Customer]  
           ([GUID]  
           ,[FirstName]  
           ,[SurName]  
           ,[EmailAddress]  
           ,[Password]  
           ,[CreatedOn]  
           ,[CreatedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy]  
           ,[StatusId]  
           ,[LastLoginDate]  
           ,[LoginAttempts]  
           ,[LastLoginIP]  
           ,[Tocken]  
     --,JVBoxStatusId  
     )  
     VALUES  
           ( @rvcGUID  
           ,@rvcFirstName  
           ,@rvcLastName  
           ,@rvcEmailAddress  
           ,@rvcPassword  
           ,GETDATE()  
           ,''  
           ,GETDATE()  
           ,''  
           ,5--@rvcStatusId  
           ,Getdate()  
           ,0  
           ,@rvcLastLoginIP  
           ,0  
     --,11  
     )  
  
  SELECT @iCustomerId= @@IDENTITY  
  
  Begin  
   Insert into dbo.Customer_ContactDetail   
   (  
    [CustomerId],     
    [GUID]  
   )  
   Values (  
    @iCustomerId,  
    NEWID()  
   )  
  END  
  
 END  
 Begin  
  If Not exists (Select 1 From [dbo].[SocialProfile] where [CustomerId]= @iCustomerId)  
  BEGIN  
   INSERT INTO [dbo].[SocialProfile]  
           ([CustomerId]  
           ,[SocialProfileTypeId]  
         
           ,[StatusId]  
           ,[StripeCustomerId]  
           ,[SocialUsername]  
           ,[SocialPassword]  
           ,[SocialProfileName]  
         
           ,[CreatedOn]  
           ,[CreatedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy])  
     VALUES  
           (@iCustomerId  
           ,30  
         
           ,5  
           ,null  
           ,null  
           ,null  
           ,'Social Profile 1'  
         
           ,getdate()  
           ,@rvcEmailAddress  
           ,getdate()  
           ,@rvcEmailAddress  
     )  
  
   SELECT @iSocialProfileId= @@IDENTITY  
  END  
  ELSE  
  BEGIN   
   SELECT TOP 1 @iSocialProfileId=[SocialProfileId]   
    From [dbo].[SocialProfile] where [CustomerId]= @iCustomerId order by CreatedOn desc  
  END  
 --If not exists(Select 1 From [dbo].[SocialProfile_Instagram_TargetingInformation] where [SocialProfileId]= @iSocialProfileId )   
 --Begin  
 -- INSERT INTO [dbo].[SocialProfile_Instagram_TargetingInformation]  
 --          ([SocialProfileId]  
 --          ,[Preference1]  
 --          ,[Preference2]  
 --          ,[Preference3]  
 --          ,[Preference4]  
 --          ,[Preference5]  
 --          ,[Preference6]  
 --          --,[Preference7]  
 --          --,[Preference8]  
 --          --,[Preference9]  
 --          --,[Preference10]  
 --          --,[InstaUser]  
 --          --,[InstaPassword]  
 --    ,CreatedOn,  
 --  UpdatedOn  
 --  -- City  
 --    )  
 --   VALUES  
 --   (@iSocialProfileId ,  
 --    @rvcPreference1 ,  
 --    @rvcPreference2 ,  
 --    @rvcPreference3,   
 --    @rvcPreference4 ,  
 --    @iPreference5 ,  
 --    @iPreference6 ,  
 --    --@iPreference7 ,  
 --    --@rvcPreference8,   
 --    --@rvcPreference9 ,  
 --    --@rvcPreference10,  
 --    --@rvcInstaUser ,  
 --    --@rvcInstaPassword,  
 --    getdate(),  
 --    GETDATE()  
       
  
 --   )  
 --END  
  
 SELECT TOP 1  
  GUID,  
  C.CustomerId,  
  C.[FirstName],  
  [EmailAddress],  
  [SurName],  
  [Password],  
  C.StripeCustomerId,  
  SP.[SocialProfileId]  
 From [dbo].[Customer]  C  
  inner join [dbo].[SocialProfile] SP ON C.[CustomerId]=SP.[CustomerId]  
   AND SP.[SocialProfileId]=@iSocialProfileId  
 WHERE C.CustomerId= @iCustomerId  
  
End  
  