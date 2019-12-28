  
CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_Payments_Save]  
(  
 @riSocialProfileId     INT,  
 @riStripeSubscriptionId    NVARCHAR(255),  
 @rvcDescription      NVARCHAR(250),  
 @rvcName       NVARCHAR(255),  
 @riPrice       Decimal(18,2),  
 @riStripePlanId      NVARCHAR(255),  
 @rvcSubscriptionType    NVARCHAR(255),  
 @rdtStartDate      datetime,  
 @rdtEndDate       datetime,  
 @riStatusId       Int,  
 @riPaymentPlanId     Int,  
 @rvcStripeInvoiceId     NVARCHAR(255)  
)  
   
AS    
   
   
BEGIN  
   declare @subId int;  
  
      Update [dbo].[SocialProfile_Payments]  
   Set StatusId=27  
   where [SocialProfileId]=@riSocialProfileId  
   AND StatusId not in (26,27)  
  
    
   INSERT INTO [dbo].[SocialProfile_Payments]  
           ([Name]  
           ,[Description]  
           ,[SubscriptionType]  
           ,[Price]  
           ,[StartDate]  
           ,[EndDate]  
           ,[SocialProfileId]  
           ,[StripeSubscriptionId]  
           ,[StatusId]  
           ,[StripePlanId]  
           ,[PaymentPlanId]  
     ,StripeInvoiceId  
     )  
      VALUES  
      (  
       @rvcName,  
       @rvcDescription,  
       @rvcSubscriptionType,  
       @riPrice,  
       @rdtStartDate,  
       @rdtEndDate,  
       @riSocialProfileId,  
       @riStripeSubscriptionId,  
       @riStatusId,  
       @riStripePlanId,  
       (select top 1 [PlanId] from [dbo].[SocialProfile_PaymentPlan] where [StripePlanId] = @riStripePlanId),  
      @rvcStripeInvoiceId  
       )  
    
  
 select @subId = @@identity  
   
 SELECT [SubscriptionId]  
      ,[Name]  
      ,[Description]  
      ,[SubscriptionType]  
      ,[Price]  
      ,[StartDate]  
      ,[EndDate]  
      ,[SocialProfileId]  
      ,[StripeSubscriptionId]  
      ,[StatusId]  
      ,[StripePlanId]  
      ,[PaymentPlanId]  
   ,StripeInvoiceId  
  FROM [dbo].[SocialProfile_Payments]  
 where [SubscriptionId] = @subId  
    
END  