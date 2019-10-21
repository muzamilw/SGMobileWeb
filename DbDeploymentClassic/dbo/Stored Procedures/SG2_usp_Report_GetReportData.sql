
CREATE Procedure [dbo].[SG2_usp_Report_GetReportData]

As
Begin	
SET FMTONLY OFF;

		 DECLARE  -- JV Box
		   @iTotalJVServersUsage BIGINT,
		   @iTotalServer BIGINT,

		   -- Proxy IPs
		   @iTotalUsedIPs BIGINT,
		   @iAvailableIPsbyCity BIGINT,
		   @iAllAvailableIPs BIGINT,

		   -- Most Used Plan
		   @nvPlanName nvarchar(max),
		   @iNoOfPlanUsed BIGINT,
		   @dTotalAmount Decimal(18,2)

		   CREATE TABLE #tblMaxUsedPlan
		   (
			   PlanName NVARCHAR(max),
			   NoOfPlanUsed  BIGINT,
			   TotalAmount DECIMAL(18,2)
		   )



			-- JV Box
			SELECT @iTotalServer = SUM(MaxLimit) From [dbo].[SG2_JVBox] WITH(NOLOCK)
			SELECT @iTotalJVServersUsage = COUNT(SocialProfileId) FROM [dbo].[SG2_SocialProfile] WITH(NOLOCK) Where JVBoxId IS NOT NULL
		  

			-- Proxy IPs

			SELECT @iAllAvailableIPs = COUNT(ProxyId) FROM [dbo].[SG2_Proxy] WITH(NOLOCK)

			SELECT @iTotalUsedIPs = COUNT(*) FROM [dbo].[SG2_SocialProfile_ProxyMapping] WITH(NOLOCK)

			INSERT INTO #tblMaxUsedPlan(PlanName,NoOfPlanUsed,TotalAmount)
			SELECT PP.PlanName, COUNT(S.PaymentPlanID),
					SUM(PP.StripePlanPrice)
			From [dbo].[SG2_SocialProfile_Subscription] S 
					INNER JOIN [dbo].[SG2_SocialProfile_PaymentPlan] PP ON S.PaymentPlanID = PP.PlanID
			GROUP BY PP.PlanName



			SELECT @nvPlanName = PlanName, @iNoOfPlanUsed = NoOfPlanUsed, @dTotalAmount =TotalAmount
			FROM #tblMaxUsedPlan
			Where NoOfPlanUsed >= (SELECT max(NoOfPlanUsed) FROM #tblMaxUsedPlan)
	
		 DROP TABLE #tblMaxUsedPlan

			SELECT 
			  CAST(ISNULL(@iTotalServer,0) AS BIGINT ) AS TotalJVServer,
			  CAST(ISNULL(@iTotalJVServersUsage,0) AS BIGINT ) AS TotalJVServersUsage,
			  CAST((ISNULL(@iTotalServer,0) - ISNULL(@iTotalJVServersUsage,0))AS BIGINT ) AS FreeSlotsPerServer,
			  CAST(ISNULL(@iAllAvailableIPs,0)AS BIGINT ) AS AllAvailableIPs,
			  CAST(ISNULL(@iTotalUsedIPs,0)AS BIGINT ) AS TotalUsedIPs,
			  CAST(ISNULL(@nvPlanName,'') AS nvarchar(max) ) AS PlanName,
			  CAST(ISNULL(@iNoOfPlanUsed,0)AS BIGINT ) AS NoOfPlanUsed,
			  CAST(ISNULL(@dTotalAmount,0.00)AS decimal ) AS TotalAmount

	

			
	-- GROUP BY [SocialProfileId], [Username],[Date]
End

