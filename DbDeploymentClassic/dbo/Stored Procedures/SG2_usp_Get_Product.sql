
CREATE Procedure [dbo].[SG2_usp_Get_Product]
 
As  
Begin

SELECT PlanName as [Name],StripePlanId as StripeSubscriptionId
FROM [dbo].[SG2_SocialProfile_PaymentPlan] 

End

