CREATE PROCEDURE [dbo].[SG2_usp_PlanInformation_Save]
(
	@riPlanId			INT,
	@rvcPlanName			NVARCHAR(15),
	@rvcPlanDescription   NVARCHAR(250),
	@rvcPlanType			NVARCHAR(50),
	@riLikes				INT,
	@riPrice				float,
	@riNoOfLikesDuration   INT,
	@riStatusId   INT,
	@SortOrder INT,
	@rbIsDefault bit,
	@rvcStripePlanId Nvarchar(250),
	@rvcStripePlanPrice float,
	@riSocialPlanTypeId int
)
 
AS  
 
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].[SG2_SocialProfile_PaymentPlan] WHERE [PlanId] = @riPlanId ) 
		BEGIN
			INSERT INTO [dbo].[SG2_SocialProfile_PaymentPlan]
					   (
		   				[NoOfLikes]         ,
						 [DisplayPrice]      , 
						 [PlanName]        ,
						[PlanShortDescription]  ,
						[PlanTypeId]  ,
						[NoOfLikesDuration],
						 StripePlanId,
						 StatusId,
						 SortOrder,
						 IsDefault,
						 [CreatedOn],
						[UpdatedOn],
						[StripePlanPrice],
						SocialPlanTypeId
						)
					 VALUES
					 (
						@riLikes	,
						 @riPrice,
						 @rvcPlanName,
						 @rvcPlanDescription,
						 @rvcPlanType,
						 @riNoOfLikesDuration,
						 @rvcStripePlanId,
						 @riStatusId,
						 @SortOrder,
						 @rbIsDefault,
						 GETDATE(),
						 GETDATE(),
						 @rvcStripePlanPrice,
						 @riSocialPlanTypeId
						 
						 )
					 Select @riPlanId = SCOPE_IDENTITY() 
			END
	ELSE
		BEGIN
			UPDATE [dbo].[SG2_SocialProfile_PaymentPlan]
			   SET
						
						[NoOfLikes] =  @riLikes   ,    
						[DisplayPrice]  =  @riPrice  ,    
						[PlanName]=   @rvcPlanName ,   
						[PlanShortDescription]=@rvcPlanDescription,
						[PlanTypeId] =@rvcPlanType,
						[NoOfLikesDuration]=@riNoOfLikesDuration,
						StatusId=@riStatusId,
						SortOrder=@SortOrder,
						IsDefault=@rbIsDefault,
						[CreatedOn]=GETDATE(),
						[UpdatedOn]=GETDATE(),
						[StripePlanId]=@rvcStripePlanId,
						[StripePlanPrice]=@rvcStripePlanPrice,
						SocialPlanTypeId=@riSocialPlanTypeId
			 WHERE  [PlanId]= @riPlanId
		End
END
