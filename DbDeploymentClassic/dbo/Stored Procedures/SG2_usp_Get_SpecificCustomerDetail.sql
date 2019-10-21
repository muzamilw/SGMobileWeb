  
CREATE Procedure [dbo].[SG2_usp_Get_SpecificCustomerDetail]  
  @riCustomerId int,  
  @riProfileId int  
  
As  
Begin  
--declare @riCustomerId int = 3;  
 Select   
  C.CustomerId as Id,  
  C.UserName [Name],  
  C.EmailAddress as Email,  
  C.FirstName as FirstName,  
  C.SurName as LastName,   
  CCD.PhoneNumber as Tel,   
  CCD.MobileNumber as Mobile,  
  CCD.AddressLine1 as AddressLine,   
  '' as Town,  
  CCD.City as City,  
  CCD.PostalCode as PostalCode,   
  CCD.Country as Country,   
  SP.[SocialUsername] as InstaUsrName,   
  SP.[SocialPassword] as InstaPassword,   
  C.CreatedOn as UpdatedOn,  

  C.IsOptedEducationalEmailSeries as OptedEdEmailSeries,  
  C.IsOptedMarketingEmail as OptedMarkEmail,  
  C.Source as Source,  
  C.Register as Register ,   
  ISNULL(C.ResponsibleTeamMemberId,0) as  ResTeamMember,  
  ISNULL(C.AvailableToEveryOne,0) as AvaToEveryOne,  
  ISNULL(C.Comment,'') as Comment,  
  C.Title as Title,  
  SP.SocialProfileName,  
  C.StatusId as CustomerStatus,  
  ISNULL(SP.IsArchived,0) as IsArchived
  from Customer C   
  inner join  Customer_ContactDetail CCD  
   ON C.CustomerId=CCD.CustomerId  
  Inner join [dbo].[SocialProfile] SP ON SP.[SocialProfileId] =@riProfileId  
  
  Left join [dbo].[SocialProfile_Instagram_TargetingInformation] CTI ON CTI.[SocialProfileId]=SP.[SocialProfileId]  
  Left join SystemUser SU ON SU.SystemUserId=C.ResponsibleTeamMemberId  
 
 where C.CustomerId=@riCustomerId  
End  
  