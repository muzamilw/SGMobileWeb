  
CREATE Procedure [dbo].[SG2_usp_Report_GetMostUsedProductData]  
(  
  @dtFromDate Date = null,  
  @dtToDate   Date = null  
)  
As  
Begin   
SET FMTONLY OFF;  
  
   DECLARE @iTotalPlan BIGINT  
       
   SELECT   @iTotalPlan =  count(t.[SubscriptionId])   
   FROM [dbo].[SocialProfile_Subscription] t  
   WHERE T.StartDate BETWEEN @dtFromDate AND @dtToDate  
  
   SELECT S.Name as PlanName, COUNT(S.PaymentPlanID) PlanSold, @iTotalPlan as TotalPlanSold  
   FROM [dbo].[SocialProfile_Subscription] S   
   WHERE S.StartDate BETWEEN @dtFromDate AND @dtToDate  
   GROUP BY S.Name  
  
  
End  
  