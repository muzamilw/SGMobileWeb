CREATE Procedure [dbo].[SG2_usp_PlanInformation_GetById]
  @riPlanId Int

As  
Begin

SELECT 
[PlanId],
[NoOfLikes] as Likes,
[DisplayPrice],
[PlanName],
[PlanShortDescription] as PlanDescription,
[PlanTypeId] as PlanType,
[StripePlanId],
ISNULL([NoOfLikesDuration],0) as NoOfLikesDuration ,
[StatusId],
[SortOrder],
[SocialPlanTypeId],
[StripePlanPrice] as PlanPrice,
[IsDefault]
FROM [dbo].[SG2_SocialProfile_PaymentPlan]
Where [PlanId] = @riPlanId

End
