CREATE Procedure [dbo].[SG2_usp_Get_SocialProfile_PaymentPlan]  
As  
Begin  
  
 SELECT   
  [paymentPlanId],  
  [NoOfLikes] as Likes,  
  [StripePlanPrice] as PlanPrice,  
  [PlanName],  
  [PlanShortDescription] as PlanDescription,  
  isparentplan,  

  [StripePlanId],  
  ISNULL([NoOfLikesDuration],0) as NoOfLikesDuration ,  
  [StatusId],  
  EV2.[Name] as StatusName,  
  [SortOrder],  
  socialplatform,  
  SP.[IsDefault],  
  DisplayPrice  
 FROM [dbo].[PaymentPlan] SP  
  --Left join [dbo].[EnumerationValue] EV on SP.PlanTypeId=EV.EnumerationValueId  
  Left Join [dbo].[EnumerationValue] EV2 on SP.StatusId=EV2.EnumerationValueId  
 WHERE  
  SP.STATUSID = 19  
 Order by SortOrder asc  
  
End  