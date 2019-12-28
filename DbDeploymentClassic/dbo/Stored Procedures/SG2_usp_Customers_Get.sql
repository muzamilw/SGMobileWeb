
CREATE Procedure [dbo].[SG2_usp_Customers_Get]
  @Id Int

As  
Begin

	-- Searches for Customers based on given parameters  

	SELECT TOP 1 
	   CST.[CustomerId]
	  ,CST.[GUID]
      ,CST.[FirstName]
      ,CST.[SurName]
      ,CST.[EmailAddress]
      ,CST.[Password]
      ,CST.[CreatedOn]
      ,CST.[CreatedBy]
      ,CST.[UpdatedOn]
      ,CST.[UpdatedBy]
      ,CST.[StatusId]
      ,CST.[LastLoginDate]
      ,CST.[LoginAttempts]
      ,CST.[LastLoginIP]
      ,CST.[Tocken]
    
      ,CST.[StripeCustomerId]
      ,CST.[UserName]
     
      ,CST.[Source]
      ,CST.[Register]
      ,CST.[ResponsibleTeamMemberId]
      ,CST.[AvailableToEveryOne]
      ,CST.[Comment]
      ,CST.[CancelledDate]
      ,cast(Coalesce(CST.[IsOptedEducationalEmailSeries], 0) as bit) IsOptedEducationalEmailSeries
      ,cast(Coalesce(CST.[IsOptedMarketingEmail] , 0) as bit) IsOptedMarketingEmail
	  ,CD.[ContactDetailsId]
      ,CD.[JobTitle]
      ,CD.[MobileNumber]
      ,CD.[PhoneNumber]
      ,CD.[AddressLine1]
      ,CD.[AddressLine2]
      ,CD.[City]
      ,CD.[Sate]
      ,CD.[Country]
      ,CD.[PostalCode]
      ,CD.[PhoneCode]
	  ,sub.StripeSubscriptionId
	  ,(Select top 1 SocialProfileId from SocialProfile where CustomerId = @Id) As DefaultSocialProfileId 
	  FROM  [dbo].[Customer] CST
			Left Join [dbo].[Customer_ContactDetail] CD on cd.customerid = cst.customerid
			Inner join [dbo].[SocialProfile] SP ON SP.[CustomerId]=cd.customerid
			Left Join [dbo].[SocialProfile_Payments] sub on sub.[SocialProfileId]= SP.[SocialProfileId] AND sub.StatusId = 25 -- Active Subsription
		Where CST.CustomerId=@Id
   
End

