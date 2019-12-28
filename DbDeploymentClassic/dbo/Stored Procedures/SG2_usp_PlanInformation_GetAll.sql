  
CREATE Procedure [dbo].[SG2_usp_PlanInformation_GetAll] -- '',1,1,1
(  
  @rsSearchCrite Nvarchar(MAX),  
  @riPageNumber Int,  
  @riPageSize varchar(8),  
  @riStatusId int=null  
)  
As    
Begin  
  
  
 -- Searches for Products based on given parameters    
 Declare @iFirstRow Int      
 Declare @iLastRow Int   
  
 Declare @xmlSearchCriteria Xml  
   
   
 --Set @xmlSearchCriteria = dbo.udf_UTL_UpdateWildcardInput(@rsSearchCriteria)  
  
 Set @iFirstRow  = (@riPageNumber-1) * @riPageSize + 1      
 Set @iLastRow = @riPageSize + @iFirstRow - 1   
   
 ;with cte as (  
 SELECT      [paymentPlanId],  
    [NoOfLikes] as Likes,  
    [DisplayPrice] DisplayPrice,  
    [PlanName],  
    [PlanShortDescription] as PlanDescription,  
    isparentplan as PlanType,  
    isparentplan as PlanTypeId,  
    [StripePlanId],  
    [StripePlanPrice] as PlanPrice,  
    [NoOfLikesDuration] as  NoOfLikesDuration,  
    EV2.[Name] as [Status],  
    [SortOrder] as SortOrder,  
    EV3.[Name]      as SocialPlanType,  
      (Select Count(1) From [dbo].[PaymentPlan]) TotalRecord,  
       ROW_NUMBER() OVER (ORDER BY  paymentPlanId desc) AS RankId  
   FROM [dbo].[PaymentPlan] LA  
   --Left join [dbo].[EnumerationValue] EV on LA.PlanTypeId=EV.EnumerationValueId  
   Left Join [dbo].[EnumerationValue] EV2 on LA.StatusId=EV2.EnumerationValueId  
   Left join  [dbo].[EnumerationValue] EV3 on LA.SocialPlatform=EV2.EnumerationValueId  
   Where (  
   ((@riStatusId is null)or LA.StatusId = @riStatusId)  
  And (   
    (@rsSearchCrite is null or @rsSearchCrite = '')   
    or (LA.PlanName like '%' +@rsSearchCrite +'%' or LA.[NoOfLikes] like '%' +@rsSearchCrite +'%' )  
   )  
 )  
 )  
  
 SELECT  [paymentPlanId] as PlanId,  
     [Likes] as Likes,  
     DisplayPrice as DisplayPrice,  
     [PlanName] as PlanName,  
     [PlanDescription] as PlanDescription,  
     [PlanType] as PlanType,  
     PlanTypeId as PlanTypeId,  
     [StripePlanId] as StripePlanId,  
     TotalRecord,  
     PlanPrice,  
     NoOfLikesDuration,  
     [Status],  
     SortOrder,  
     SocialPlanType  
 FROM cte where RankId Between @iFirstRow And @iLastRow  
 Order by SortOrder asc  
  
End  