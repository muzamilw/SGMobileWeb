-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[SG2_usp_Customer_GetSocialProfilesByCustomerId]   
 @CustomerId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 SELECT   
  SP.SocialProfileId,  
  SP.CustomerId,  
  SP.SocialProfileTypeId,  
  SP.StatusId,  
  SP.StripeCustomerId,  
  SP.SocialUsername,  
  SP.SocialPassword,  
  SP.SocialProfileName as ProfileName,  
  EV.[Description] StatusName,  
  EV2.[DESCRIPTION] SocialProfileTypeName,  
  
  CASE   
  WHEN SS.SubscriptionId IS Null  
  THEN 'Expired'  
  ELSE   
  'Active'  
  End SubscriptionStatus,  
  SS.[Name] as SubscriptionName  
 FROM [dbo].[SocialProfile] SP  
 LEFT JOIN [dbo].[SocialProfile_Payments] SS ON SP.[SocialProfileId]=SS.[SocialProfileId]  
               AND SS.StatusId=25  
 INNER JOIN [dbo].[EnumerationValue] EV  
  ON EV.[EnumerationValueId] = SP.STATUSID  

 LEFT JOIN [dbo].[EnumerationValue] EV2  
  ON EV2.[EnumerationValueId] = SP.SOCIALPROFILETYPEID  
 
 WHERE SP.CUSTOMERID = @CustomerId;  
END  