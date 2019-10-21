
CREATE Procedure [dbo].[SG2_usp_PlanInformation_Delete]
  @riPlanInformationId Int
As  
Begin
DECLARE @StripPlanId int
SELECT @StripPlanId=[StripePlanId]  FROM [dbo].[SG2_SocialProfile_PaymentPlan] where PlanId=@riPlanInformationId
if not exists(SELECT 1 from [dbo].[SG2_SocialProfile_Subscription] where [StripeSubscriptionId]=@StripPlanId)
BEGIN
DELETE FROM [dbo].[SG2_SocialProfile_PaymentPlan] where [StripePlanId]=@StripPlanId
END
ELSE
BEGIN
   UPDATE [dbo].[SG2_SocialProfile_PaymentPlan]
   SET StatusID = 18
   WHERE [PlanId] = @riPlanInformationId
END 
 Return 1;
    
End
