-- <Migration ID="08b0585b-7819-4cf7-a986-9cea9327590c" />
GO

PRINT N'Creating [dbo].[SocialProfile_Payments]'
GO
CREATE TABLE [dbo].[SocialProfile_Payments]
(
[SubscriptionId] [int] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (255) NOT NULL,
[Description] [nvarchar] (255) NOT NULL,
[SubscriptionType] [nvarchar] (255) NULL,
[Price] [decimal] (18, 2) NULL,
[StartDate] [datetime] NOT NULL,
[EndDate] [datetime] NOT NULL,
[SocialProfileId] [int] NOT NULL,
[StripeSubscriptionId] [nvarchar] (255) NULL,
[StatusId] [int] NULL,
[StripePlanId] [nvarchar] (255) NULL,
[PaymentPlanId] [int] NULL,
[StripeInvoiceId] [nvarchar] (255) NULL
)
GO
PRINT N'Creating primary key [PK_SG2_Subscription] on [dbo].[SocialProfile_Payments]'
GO
ALTER TABLE [dbo].[SocialProfile_Payments] ADD CONSTRAINT [PK_SG2_Subscription] PRIMARY KEY CLUSTERED  ([SubscriptionId])
GO
PRINT N'Creating [dbo].[SocialProfile_Statistics]'
GO
CREATE TABLE [dbo].[SocialProfile_Statistics]
(
[SocialStatisticsId] [bigint] NOT NULL IDENTITY(1, 1),
[SocialProfileId] [int] NOT NULL,
[Username] [nvarchar] (50) NOT NULL,
[Date] [datetime] NOT NULL,
[FollowersGain] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_FollowersGain] DEFAULT ((0)),
[Followers] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Followers] DEFAULT ((0)),
[Followings] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Followings] DEFAULT ((0)),
[FollowingsRatio] [decimal] (18, 2) NULL CONSTRAINT [DF_SG2_SocialProfile_Statistics_Followings1] DEFAULT ((0)),
[Joiner] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Joiner] DEFAULT ((0)),
[Unjoiner] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Unjoiner] DEFAULT ((0)),
[Follow] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Follow] DEFAULT ((0)),
[Unfollow] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Unfollow] DEFAULT ((0)),
[ContactMassage] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_ContactMassage] DEFAULT ((0)),
[ContactFriends] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_ContactFriends] DEFAULT ((0)),
[REPinTweetBlog] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_RE(Pin/Tweet/Blog] DEFAULT ((0)),
[Bump] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Bump] DEFAULT ((0)),
[Like] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Like] DEFAULT ((0)),
[Comment] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Comment] DEFAULT ((0)),
[Engagement] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Engagement] DEFAULT ((0)),
[Repost] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_Repost] DEFAULT ((0)),
[LikeComments] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_LikeComments] DEFAULT ((0)),
[StoryViewer] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_StoryViewer] DEFAULT ((0)),
[BlockedFollowers] [bigint] NULL CONSTRAINT [DF_CustomerStatistics_BlockedFollowers] DEFAULT ((0)),
[CreatedDate] [datetime] NOT NULL,
[UpdateDate] [datetime] NULL
)
GO
PRINT N'Creating primary key [PK_CustomerStatistics] on [dbo].[SocialProfile_Statistics]'
GO
ALTER TABLE [dbo].[SocialProfile_Statistics] ADD CONSTRAINT [PK_CustomerStatistics] PRIMARY KEY CLUSTERED  ([SocialStatisticsId])
GO
PRINT N'Creating [dbo].[SocialProfile_Instagram_TargetingInformation]'
GO
CREATE TABLE [dbo].[SocialProfile_Instagram_TargetingInformation]
(
[TargetingInformationId] [int] NOT NULL IDENTITY(1, 1),
[SocialProfileId] [int] NULL,
[IsSystem] [bit] NULL,
[FollowOn] [bit] NULL,
[LikeOn] [bit] NULL,
[UnFollowOn] [bit] NULL,
[StoryViewerOn] [bit] NULL,
[ContactMembersOn] [bit] NULL,
[HashTagsToEngage] [nvarchar] (255) NULL,
[HashTagsToNotEngage] [nvarchar] (255) NULL,
[LocationsToEngage] [nvarchar] (255) NULL,
[GenderEngagmentPref] [int] NULL,
[IncludeBusinessAccounts] [bit] NULL,
[MonOper] [bit] NULL,
[TueOper] [bit] NULL,
[WedOper] [bit] NULL,
[ThuOper] [bit] NULL,
[FriOper] [bit] NULL,
[SatOper] [bit] NULL,
[SunOper] [bit] NULL,
[ExecutionIntervals] [nvarchar] (500) NULL,
[RandomizeIntervalsDaily] [bit] NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NULL,
[UpdatedOn] [datetime] NOT NULL,
[UpdatedBy] [nvarchar] (50) NULL,
[SocialAccAs] [int] NULL,
[FollNewPerDayLim] [int] NULL,
[FollDailyIncreaseLim] [int] NULL,
[FollMaxPerDayLim] [int] NULL,
[UnFollNewPerDayLim] [int] NULL,
[UnFollDailyIncreaseLim] [int] NULL,
[UnFollMaxPerDayLim] [int] NULL,
[LikePerDayLim] [int] NULL,
[LikeDailyIncreaseLim] [int] NULL,
[LikeMaxPerDayLim] [int] NULL,
[ViewStoriesPerDayLim] [int] NULL,
[ViewStoriesDailyIncreaseLim] [int] NULL,
[ViewStoriesMaxPerDayLim] [int] NULL,
[CommentPerDayLim] [int] NULL,
[CommentDailyIncreaseLim] [int] NULL,
[CommentMaxPerDayLim] [int] NULL,
[DMPerDayLim] [int] NULL,
[DMDailyIncreaseLim] [int] NULL,
[DMMaxPerDayLim] [int] NULL,
[FollUserProfileImage] [bit] NULL,
[FollUserMinPosts] [int] NULL,
[FollUserPostsLastXDays] [int] NULL,
[FollUseBlackList] [bit] NULL,
[FollCheckPostsCapBlackList] [bit] NULL,
[FollDoNotFollowPrivateUser] [bit] NULL,
[FollSkipBizAcct] [bit] NULL,
[FollDoNotFollowUsernamewithdigits] [int] NULL,
[FollOnlyFollowGender] [int] NULL,
[FollUserLangs] [nvarchar] (500) NULL,
[FollowLikeCommentHashtagsKeywordw] [bit] NULL,
[FollowLikeCommentHashtagsKeywordwValue] [int] NULL,
[FollowLikeViewCommentTopPosts] [bit] NULL,
[FollowLikeViewCommentTopPostsValue] [int] NULL,
[AfterFollLikeuserPosts] [bit] NULL,
[AfterFollLikeuserPostsWaitSecsFrom] [int] NULL,
[AfterFollLikeuserPostsWaitSecsTo] [int] NULL,
[AfterFollCommentUserPosts] [bit] NULL,
[AfterFollViewUserStory] [bit] NULL,
[AfterFollMuteUser] [bit] NULL,
[FollEngageDaily] [bit] NULL,
[FollEngageDailyfollCountFrmUnFollowList] [int] NULL,
[FollEngageLikeRecentPost] [bit] NULL,
[FollEngageEnableLikeCommAfterPostLike] [bit] NULL,
[FollEngageSendDMAfterLike] [bit] NULL,
[FollEngageViewUserStoryAfterLike] [bit] NULL,
[UnFollFollowersAfterMinDays] [bit] NULL,
[UnFollFollowersAfterMinDaysVal] [int] NULL,
[UnFollDoNotUnFollowLikersOfPosts] [bit] NULL,
[UnFollDoNotUnFollowLikersOfPostsCount] [int] NULL,
[UnFollDoNotUnFollowCommThatCommented] [bit] NULL,
[UnFollDoNotUnFollowCommThatCommentedCount] [int] NULL,
[UnFollUseWhiteList] [bit] NULL,
[StorVwOnlyPostXMin] [bit] NULL,
[StorVwOnlyPostXMinVal] [int] NULL,
[StorVwLikeUsrPostRcntPost] [bit] NULL,
[StorVwLikeUsrPostRcntPostVal] [int] NULL,
[StorVwReply] [bit] NULL,
[StorVwSendDMAfterLike] [bit] NULL,
[CommUsrRecentPosts] [bit] NULL,
[CommUsrRecentPostsVal] [int] NULL,
[CommUsrRecentPostsTypes] [int] NULL,
[CommFltrPostsByLikeCount] [bit] NULL,
[CommDelCommAfterXDays] [bit] NULL,
[CommDelCommAfterXDaysCount] [int] NULL,
[CommLine1] [nvarchar] (500) NULL,
[CommLine2] [nvarchar] (500) NULL,
[CommLine3] [nvarchar] (500) NULL,
[CommLine4] [nvarchar] (500) NULL,
[CommLine5] [nvarchar] (500) NULL,
[DMShowUnReadMsg] [bit] NULL,
[DMShowPendingReq] [bit] NULL,
[LEXLikeUsrMostRecentPosts] [bit] NULL,
[LEXLikeUsrMostRecentPostsCount] [int] NULL,
[LEXUseBlackList] [bit] NULL,
[LEXChkPostCaptionsforBlackList] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_TargetingInformation] on [dbo].[SocialProfile_Instagram_TargetingInformation]'
GO
ALTER TABLE [dbo].[SocialProfile_Instagram_TargetingInformation] ADD CONSTRAINT [PK_SG2_TargetingInformation] PRIMARY KEY CLUSTERED  ([TargetingInformationId])
GO
PRINT N'Creating [dbo].[SocialProfile]'
GO
CREATE TABLE [dbo].[SocialProfile]
(
[SocialProfileId] [int] NOT NULL IDENTITY(1, 1),
[CustomerId] [int] NULL,
[SocialProfileTypeId] [int] NULL,
[StatusId] [int] NULL,
[StripeCustomerId] [int] NULL,
[SocialUsername] [nvarchar] (50) NULL,
[SocialPassword] [nvarchar] (50) NULL,
[SocialProfileName] [nvarchar] (50) NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NULL,
[UpdatedOn] [datetime] NOT NULL,
[UpdatedBy] [nvarchar] (50) NULL,
[verificationCode] [nvarchar] (50) NULL,
[IsArchived] [bit] NULL CONSTRAINT [DF_IsArchived] DEFAULT ((0)),
[DeviceIMEI] [nvarchar] (100) NULL,
[DeviceStatus] [int] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_CustomerProfile] on [dbo].[SocialProfile]'
GO
ALTER TABLE [dbo].[SocialProfile] ADD CONSTRAINT [PK_SG2_CustomerProfile] PRIMARY KEY CLUSTERED  ([SocialProfileId])
GO
PRINT N'Creating [dbo].[Customer_ContactDetail]'
GO
CREATE TABLE [dbo].[Customer_ContactDetail]
(
[ContactDetailsId] [int] NOT NULL IDENTITY(1, 1),
[CustomerId] [int] NOT NULL,
[JobTitle] [nvarchar] (50) NULL,
[MobileNumber] [nvarchar] (50) NULL,
[PhoneNumber] [nvarchar] (50) NULL,
[AddressLine1] [nvarchar] (255) NULL,
[AddressLine2] [nvarchar] (255) NULL,
[City] [nvarchar] (50) NULL,
[Sate] [nvarchar] (50) NULL,
[Country] [nvarchar] (50) NULL,
[PostalCode] [nvarchar] (50) NULL,
[GUID] [nvarchar] (36) NULL,
[PhoneCode] [nvarchar] (5) NULL,
[ScheduleCallDate] [datetime] NULL,
[Notes] [nvarchar] (max) NULL
)
GO
PRINT N'Creating primary key [PK_SG2_ContactDetails] on [dbo].[Customer_ContactDetail]'
GO
ALTER TABLE [dbo].[Customer_ContactDetail] ADD CONSTRAINT [PK_SG2_ContactDetails] PRIMARY KEY CLUSTERED  ([ContactDetailsId])
GO
PRINT N'Creating [dbo].[Customer]'
GO
CREATE TABLE [dbo].[Customer]
(
[CustomerId] [int] NOT NULL IDENTITY(1, 1),
[GUID] [nvarchar] (36) NOT NULL,
[FirstName] [nvarchar] (20) NOT NULL,
[SurName] [nvarchar] (20) NULL,
[EmailAddress] [nvarchar] (50) NOT NULL,
[Password] [nvarchar] (64) NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NOT NULL,
[UpdatedOn] [datetime] NOT NULL,
[UpdatedBy] [nvarchar] (50) NOT NULL,
[StatusId] [smallint] NOT NULL,
[LastLoginDate] [datetime] NOT NULL,
[LoginAttempts] [tinyint] NOT NULL,
[LastLoginIP] [nvarchar] (20) NOT NULL,
[Tocken] [nvarchar] (128) NOT NULL,
[StripeCustomerId] [nvarchar] (max) NULL,
[UserName] [nvarchar] (50) NULL,
[Source] [nvarchar] (50) NULL,
[Register] [nvarchar] (50) NULL,
[ResponsibleTeamMemberId] [int] NULL,
[AvailableToEveryOne] [bit] NULL,
[Comment] [nvarchar] (max) NULL,
[CancelledDate] [datetime] NULL,
[IsOptedEducationalEmailSeries] [bit] NULL,
[IsOptedMarketingEmail] [bit] NULL,
[Title] [nvarchar] (10) NULL
)
GO
PRINT N'Creating primary key [PK_SG2_Customer] on [dbo].[Customer]'
GO
ALTER TABLE [dbo].[Customer] ADD CONSTRAINT [PK_SG2_Customer] PRIMARY KEY CLUSTERED  ([CustomerId])
GO
PRINT N'Creating [dbo].[SG2_Delete_Customer_All]'
GO
  
  
CREATE Procedure [dbo].[SG2_Delete_Customer_All]  
@riCustomerId int,  
@riSocialProfileId Int  
  
   
As    
Begin  
  
IF @riSocialProfileId =0   
BEGIN   
 -- Searches for Customers based on given parameters    

        
DELETE TI FROM [dbo].[SocialProfile_Instagram_TargetingInformation] TI Inner join [dbo].[SocialProfile]  SP  
     ON TI.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
DELETE SST  
 FROM [dbo].[SocialProfile_Statistics] SST Inner join [dbo].[SocialProfile]  SP  
     ON SST.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
  
DELETE SS  
FROM [dbo].[SocialProfile_Payments] SS Inner join [dbo].[SocialProfile]  SP  
     ON SS.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
DELETE SPSH  
FROM [dbo].[SocialProfile_StatusHistory] SPSH Inner join [dbo].[SocialProfile]  SP  
     ON SPSH.SocialProfileId=SP.[SocialProfileId]  
     Inner join [dbo].[Customer] C ON C.CustomerId=SP.CustomerId  
      Where C.CustomerId=@riCustomerId  
  
  
DELETE FROM [dbo].[SocialProfile]   
 Where [CustomerId]=@riCustomerId  
  
DELETE FROM  [dbo].[Customer_ContactDetail]  Where [CustomerId]=@riCustomerId  
DELETE FROM [dbo].[Customer] Where [CustomerId]=@riCustomerId   
END   
  
ELSE  
  
BEGIN   
  

        
DELETE TI FROM [dbo].[SocialProfile_Instagram_TargetingInformation] TI  
      Where TI.SocialProfileId=@riSocialProfileId  
DELETE SST  
 FROM [dbo].[SocialProfile_Statistics] SST   
      Where SST.SocialProfileId=@riSocialProfileId  
  
DELETE SS  
FROM [dbo].[SocialProfile_Payments] SS  
      Where SS.SocialProfileId=@riSocialProfileId  
DELETE SPSH  
FROM [dbo].[SocialProfile_StatusHistory] SPSH   
      Where SPSH.SocialProfileId=@riSocialProfileId  
  
  
DELETE FROM [dbo].[SocialProfile]   
 Where SocialProfileId=@riSocialProfileId  
  
END  
  
SELECT 1  
       
End  
  
GO
PRINT N'Creating [dbo].[PaymentPlan]'
GO
CREATE TABLE [dbo].[PaymentPlan]
(
[PaymentPlanId] [int] NOT NULL IDENTITY(1, 1),
[NoOfLikes] [int] NULL,
[DisplayPrice] [float] NULL,
[PlanName] [nvarchar] (250) NOT NULL,
[PlanShortDescription] [nvarchar] (250) NOT NULL,
[IsParentPlan] [bit] NULL,
[StripePlanId] [nvarchar] (50) NULL,
[StripePlanPrice] [float] NULL,
[NoOfLikesDuration] [int] NULL,
[StatusId] [int] NULL,
[CreatedOn] [datetime] NULL,
[CreatedBy] [nvarchar] (50) NULL,
[UpdatedOn] [datetime] NULL,
[UpdatedBy] [nvarchar] (50) NULL,
[SortOrder] [smallint] NULL,
[SocialPlatform] [int] NULL,
[IsDefault] [bit] NULL
)
GO
PRINT N'Creating primary key [PK_PlanInformation] on [dbo].[PaymentPlan]'
GO
ALTER TABLE [dbo].[PaymentPlan] ADD CONSTRAINT [PK_PlanInformation] PRIMARY KEY CLUSTERED  ([PaymentPlanId])
GO
PRINT N'Creating [dbo].[EnumerationValue]'
GO
CREATE TABLE [dbo].[EnumerationValue]
(
[EnumerationValueId] [smallint] NOT NULL,
[EnumerationId] [smallint] NOT NULL,
[Name] [nvarchar] (50) NOT NULL,
[Description] [nvarchar] (255) NOT NULL,
[IsDefault] [bit] NOT NULL,
[SequenceNo] [int] NOT NULL,
[IsVisible] [bit] NULL
)
GO
PRINT N'Creating [dbo].[SG2_usp_Customer_GetSocialProfileById]'
GO
  
CREATE Procedure [dbo].[SG2_usp_Customer_GetSocialProfileById]  
  @riSocialProfileId int  
  
As  
Begin  

  

  
   
 Select  
	 [TargetingInformationId]
      ,CTI.[SocialProfileId]
      ,[IsSystem]
      ,[FollowOn]
      ,[LikeOn]
      ,[UnFollowOn]
      ,[StoryViewerOn]
      ,[ContactMembersOn]
      ,[HashTagsToEngage]
      ,[HashTagsToNotEngage]
      ,[LocationsToEngage]
      ,[GenderEngagmentPref]
      ,[IncludeBusinessAccounts]
      ,[MonOper]
      ,[TueOper]
      ,[WedOper]
      ,[ThuOper]
      ,[FriOper]
      ,[SatOper]
      ,[SunOper]
      ,[ExecutionIntervals]
      ,[RandomizeIntervalsDaily]
      ,CTI.[CreatedOn]
      ,CTI.[CreatedBy]
      ,CTI.[UpdatedOn]
      ,CTI.[UpdatedBy]
    
      ,[FollNewPerDayLim]
      ,[FollDailyIncreaseLim]
      ,[FollMaxPerDayLim]
      ,[UnFollNewPerDayLim]
      ,[UnFollDailyIncreaseLim]
      ,[UnFollMaxPerDayLim]
      ,[LikePerDayLim]
      ,[LikeDailyIncreaseLim]
      ,[LikeMaxPerDayLim]
      ,[ViewStoriesPerDayLim]
      ,[ViewStoriesDailyIncreaseLim]
      ,[ViewStoriesMaxPerDayLim]
      ,[CommentPerDayLim]
      ,[CommentDailyIncreaseLim]
      ,[CommentMaxPerDayLim]
      ,[DMPerDayLim]
      ,[DMDailyIncreaseLim]
      ,[DMMaxPerDayLim]
      ,[FollUserProfileImage]
      ,[FollUserMinPosts]
      ,[FollUserPostsLastXDays]
      ,[FollUseBlackList]
      ,[FollCheckPostsCapBlackList]
      ,[FollDoNotFollowPrivateUser]
      ,[FollSkipBizAcct]
      ,[FollDoNotFollowUsernamewithdigits]
      ,[FollOnlyFollowGender]
      ,[FollUserLangs]
      ,[AfterFollLikeuserPosts]
      ,[AfterFollLikeuserPostsWaitSecsFrom]
      ,[AfterFollLikeuserPostsWaitSecsTo]
      ,[AfterFollCommentUserPosts]
      ,[AfterFollViewUserStory]
      ,[AfterFollMuteUser]
      ,[FollEngageDaily]
      ,[FollEngageDailyfollCountFrmUnFollowList]
      ,[FollEngageLikeRecentPost]
      ,[FollEngageEnableLikeCommAfterPostLike]
      ,[FollEngageSendDMAfterLike]
      ,[FollEngageViewUserStoryAfterLike]
      ,[UnFollFollowersAfterMinDays]
      ,[UnFollFollowersAfterMinDaysVal]
      ,[UnFollDoNotUnFollowLikersOfPosts]
      ,[UnFollDoNotUnFollowLikersOfPostsCount]
      ,[UnFollDoNotUnFollowCommThatCommented]
      ,[UnFollDoNotUnFollowCommThatCommentedCount]
      ,[UnFollUseWhiteList]
      ,[StorVwOnlyPostXMin]
      ,[StorVwOnlyPostXMinVal]
      ,[StorVwLikeUsrPostRcntPost]
      ,[StorVwLikeUsrPostRcntPostVal]
      ,[StorVwReply]
      ,[StorVwSendDMAfterLike]
      ,[CommUsrRecentPosts]
      ,[CommUsrRecentPostsVal]
      ,[CommUsrRecentPostsTypes]
      ,[CommFltrPostsByLikeCount]
      ,[CommDelCommAfterXDays]
      ,[CommDelCommAfterXDaysCount]
      ,[DMShowUnReadMsg]
      ,[DMShowPendingReq]
      ,[LEXLikeUsrMostRecentPosts]
      ,[LEXLikeUsrMostRecentPostsCount]
      ,[LEXUseBlackList]
      ,[LEXChkPostCaptionsforBlackList]


    
    ,SP.SocialProfileName  
    ,SP.SocialUsername  
    ,SP.[SocialPassword]  

    ,Sp.SocialProfileTypeId  
    ,EV4.Name as SocialProfileType  
    ,EV.Name as [ProfileStatus]  
    ,SP.StatusId as ProfileStatusId  


    ,pp.paymentPlanId  
    ,pp.PlanName  
    ,pp.StripePlanId  
    ,Subc.SubscriptionId  
    ,Subc.StartDate as SubscriptionStartDate  
    ,Subc.EndDate As SubscriptionEndDate  
    ,Subc.StatusId as SubscriptionStatusId  
    ,EV3.Name as SubscriptionStatus  

    ,Subc.[StripeSubscriptionId]  

    ,CTI.SocialAccAS  
    ,Subc.StripeInvoiceId  

      
    ,  

  SP.IsArchived  
 FROM  [dbo].[SocialProfile_Instagram_TargetingInformation] CTI  
  INNER JOIN [dbo].[SocialProfile] SP   
   ON CTI.[SocialProfileId]=SP.[SocialProfileId]  
  INNER JOIN [dbo].[Customer] C   
   ON C.[CustomerId]=SP.[CustomerId]  
  Left join EnumerationValue EV   
   ON EV.EnumerationValueId = SP.StatusId  

  Left join EnumerationValue EV4  
ON EV.EnumerationValueId = SP.socialprofiletypeid  
  Left join [dbo].[SocialProfile_Payments] Subc  
   on Subc.socialprofileid = SP.socialprofileid and subc.statusid = 25  
  Left Join [dbo].[PaymentPlan] PP  
   on Subc.paymentplanid = pp.paymentplanid  
  Left join EnumerationValue EV3  
   ON EV3.EnumerationValueId = subc.statusid  

 
 where CTI.SocialProfileId = @riSocialProfileId  
  
End  
  



	


GO
PRINT N'Creating [dbo].[SG2_usp_Customer_GetSocialProfilesByCustomerId]'
GO
-- =============================================  
-- Author:  <Author,,Name>  
-- Create date: <Create Date,,>  
-- Description: <Description,,>  
-- =============================================  
CREATE PROCEDURE [dbo].[SG2_usp_Customer_GetSocialProfilesByCustomerId]   
 @CustomerId int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  
 SELECT   
  SP.SocialProfileId,  
  SP.CustomerId,  
  SP.SocialProfileTypeId,  
  SP.StatusId,  
  SP.StripeCustomerId,  
  SP.SocialUsername,  
  SP.SocialPassword,  
  SP.SocialProfileName as ProfileName,  
  EV.[Description] StatusName,  
  EV2.[DESCRIPTION] SocialProfileTypeName,  
  
  CASE   
  WHEN SS.SubscriptionId IS Null  
  THEN 'Expired'  
  ELSE   
  'Active'  
  End SubscriptionStatus,  
  SS.[Name] as SubscriptionName  
 FROM [dbo].[SocialProfile] SP  
 LEFT JOIN [dbo].[SocialProfile_Payments] SS ON SP.[SocialProfileId]=SS.[SocialProfileId]  
               AND SS.StatusId=25  
 INNER JOIN [dbo].[EnumerationValue] EV  
  ON EV.[EnumerationValueId] = SP.STATUSID  

 LEFT JOIN [dbo].[EnumerationValue] EV2  
  ON EV2.[EnumerationValueId] = SP.SOCIALPROFILETYPEID  
 
 WHERE SP.CUSTOMERID = @CustomerId;  
END  
GO
PRINT N'Creating [dbo].[SG2_usp_Customers_Get]'
GO

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

GO
PRINT N'Creating [dbo].[SG2_usp_Customer_ProfileUpdate]'
GO
  
CREATE Procedure [dbo].[SG2_usp_Customer_ProfileUpdate]  
  @iCustomerId int,  
  @rvcUserName Nvarchar(50),  
  @rvcFirstName Nvarchar(50),  
  @rvcSurName Nvarchar(50),  
  @rvcPhoneNumber Nvarchar(64),  
  @rvcPhoneCode nvarchar(5)  
As    
Begin  
  
 Update  [dbo].[Customer]   
 set   
  FirstName=@rvcFirstName,  
  SurName=@rvcSurName,    
  UserName=@rvcUserName   
 where CustomerId=@iCustomerId;  
  
If not exists(Select 1 From dbo.Customer_ContactDetail where CustomerId = @iCustomerId)   
 Begin  
  Insert into dbo.Customer_ContactDetail  
  (  
   [CustomerId]         
   ,[PhoneNumber]          
   ,PhoneCode  
   ,[GUID]  
  )  
  Values (  
   @iCustomerId,  
   @rvcPhoneNumber,  
   @rvcPhoneCode,  
   NEWID()  
  )  
 END  
 Else   
 begin  
  Update dbo.Customer_ContactDetail  
   set   
    PhoneNumber = @rvcPhoneNumber,  
    PhoneCode = @rvcPhoneCode   
   where   
    CustomerId=@iCustomerId  
 End;  
  
exec [dbo].[SG2_usp_Customers_Get] @iCustomerId  
           
End  
  
GO
PRINT N'Creating [dbo].[SG2_usp_Customer_SignUp]'
GO
CREATE Procedure [dbo].[SG2_usp_Customer_SignUp] --'','','','','','','',1
  @rvcFirstName Nvarchar(50),
  @rvcSurName Nvarchar(50),
  @rvcEmailAddress Nvarchar(64),
  @rvcPassword Nvarchar(64),  
  @rvcCreatedBy  Nvarchar(64),
  @rvcGUID   Nvarchar(50),
  @rvcLastLoginIP Nvarchar(20),
  @rvcStatusId Int
As  
Begin

	Declare @iCustomerId int
	Declare @iSocialProfileId int
	Declare @rvcEmail Nvarchar(64)
	IF Not Exists(SELECT 1 FROM [dbo].[Customer] where [EmailAddress]=@rvcEmailAddress )
	BEGIN
	INSERT INTO [dbo].[Customer]
           ([GUID]
           ,[FirstName]
           ,[SurName]
           ,[EmailAddress]
           ,[Password]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy]
           ,[StatusId]
           ,[LastLoginDate]
           ,[LoginAttempts]
           ,[LastLoginIP]
           ,[Tocken]
		   
		   )
     VALUES
           ( @rvcGUID
           ,@rvcFirstName
           ,@rvcSurName
           ,@rvcEmailAddress
           ,@rvcPassword
           ,GETDATE()
           ,@rvcCreatedBy
           ,GETDATE()
           ,@rvcCreatedBy
           ,5--@rvcStatusId
           ,Getdate()
           ,0
           ,@rvcLastLoginIP
           ,0
		   )

	SELECT @iCustomerId= @@IDENTITY

	Begin
		Insert into dbo.Customer_ContactDetail 
		(
			[CustomerId]   
			,[GUID]
		)
		Values (
			@iCustomerId,
			NEWID()
		)
	END

	Insert into [dbo].[SocialProfile]
	(
		[CustomerId],
		[SocialProfileTypeId],
		[StatusId],
		[SocialProfileName],
		[CreatedOn],
		[CreatedBy],
		[UpdatedOn],
		[UpdatedBy]
	)
	SELECT 
		@iCustomerId,
		null,
		19,
		'Social Profile 1',
		getdate(),
		@rvcEmailAddress,
		getdate(),
		@rvcEmailAddress

	SELECT @iSocialProfileId= @@IDENTITY


INSERT INTO [dbo].[SocialProfile_Instagram_TargetingInformation]
           ([SocialProfileId]
           ,[IsSystem]
           ,[FollowOn]
           ,[LikeOn]
           ,[UnFollowOn]
           ,[StoryViewerOn]
           ,[ContactMembersOn]
           ,[HashTagsToEngage]
           ,[HashTagsToNotEngage]
           ,[LocationsToEngage]
           ,[GenderEngagmentPref]
           ,[IncludeBusinessAccounts]
           ,[MonOper]
           ,[TueOper]
           ,[WedOper]
           ,[ThuOper]
           ,[FriOper]
           ,[SatOper]
           ,[SunOper]
           ,[ExecutionIntervals]
           ,[RandomizeIntervalsDaily]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy]
           ,[SocialAccAs]
           ,[FollNewPerDayLim]
           ,[FollDailyIncreaseLim]
           ,[FollMaxPerDayLim]
           ,[UnFollNewPerDayLim]
           ,[UnFollDailyIncreaseLim]
           ,[UnFollMaxPerDayLim]
           ,[LikePerDayLim]
           ,[LikeDailyIncreaseLim]
           ,[LikeMaxPerDayLim]
           ,[ViewStoriesPerDayLim]
           ,[ViewStoriesDailyIncreaseLim]
           ,[ViewStoriesMaxPerDayLim]
           ,[CommentPerDayLim]
           ,[CommentDailyIncreaseLim]
           ,[CommentMaxPerDayLim]
           ,[DMPerDayLim]
           ,[DMDailyIncreaseLim]
           ,[DMMaxPerDayLim]
           ,[FollUserProfileImage]
           ,[FollUserMinPosts]
           ,[FollUserPostsLastXDays]
           ,[FollUseBlackList]
           ,[FollCheckPostsCapBlackList]
           ,[FollDoNotFollowPrivateUser]
           ,[FollSkipBizAcct]
           ,[FollDoNotFollowUsernamewithdigits]
           ,[FollOnlyFollowGender]
           ,[FollUserLangs]
           ,[AfterFollLikeuserPosts]
           ,[AfterFollLikeuserPostsWaitSecsFrom]
           ,[AfterFollLikeuserPostsWaitSecsTo]
           ,[AfterFollCommentUserPosts]
           ,[AfterFollViewUserStory]
           ,[AfterFollMuteUser]
           ,[FollEngageDaily]
           ,[FollEngageDailyfollCountFrmUnFollowList]
           ,[FollEngageLikeRecentPost]
           ,[FollEngageEnableLikeCommAfterPostLike]
           ,[FollEngageSendDMAfterLike]
           ,[FollEngageViewUserStoryAfterLike]
           ,[UnFollFollowersAfterMinDays]
           ,[UnFollFollowersAfterMinDaysVal]
           ,[UnFollDoNotUnFollowLikersOfPosts]
           ,[UnFollDoNotUnFollowLikersOfPostsCount]
           ,[UnFollDoNotUnFollowCommThatCommented]
           ,[UnFollDoNotUnFollowCommThatCommentedCount]
           ,[UnFollUseWhiteList]
           ,[StorVwOnlyPostXMin]
           ,[StorVwOnlyPostXMinVal]
           ,[StorVwLikeUsrPostRcntPost]
           ,[StorVwLikeUsrPostRcntPostVal]
           ,[StorVwReply]
           ,[StorVwSendDMAfterLike]
           ,[CommUsrRecentPosts]
           ,[CommUsrRecentPostsVal]
           ,[CommUsrRecentPostsTypes]
           ,[CommFltrPostsByLikeCount]
           ,[CommDelCommAfterXDays]
           ,[CommDelCommAfterXDaysCount]
           ,[DMShowUnReadMsg]
           ,[DMShowPendingReq]
           ,[LEXLikeUsrMostRecentPosts]
           ,[LEXLikeUsrMostRecentPostsCount]
           ,[LEXUseBlackList]
           ,[LEXChkPostCaptionsforBlackList])

SELECT 
      @iSocialProfileId
      ,0
      ,[FollowOn]
      ,[LikeOn]
      ,[UnFollowOn]
      ,[StoryViewerOn]
      ,[ContactMembersOn]
      ,[HashTagsToEngage]
      ,[HashTagsToNotEngage]
      ,[LocationsToEngage]
      ,[GenderEngagmentPref]
      ,[IncludeBusinessAccounts]
      ,[MonOper]
      ,[TueOper]
      ,[WedOper]
      ,[ThuOper]
      ,[FriOper]
      ,[SatOper]
      ,[SunOper]
      ,[ExecutionIntervals]
      ,[RandomizeIntervalsDaily]
      ,GETDATE()
      ,@rvcEmailAddress
      ,GETDATE()
      ,@rvcEmailAddress
      ,[SocialAccAs]
      ,[FollNewPerDayLim]
      ,[FollDailyIncreaseLim]
      ,[FollMaxPerDayLim]
      ,[UnFollNewPerDayLim]
      ,[UnFollDailyIncreaseLim]
      ,[UnFollMaxPerDayLim]
      ,[LikePerDayLim]
      ,[LikeDailyIncreaseLim]
      ,[LikeMaxPerDayLim]
      ,[ViewStoriesPerDayLim]
      ,[ViewStoriesDailyIncreaseLim]
      ,[ViewStoriesMaxPerDayLim]
      ,[CommentPerDayLim]
      ,[CommentDailyIncreaseLim]
      ,[CommentMaxPerDayLim]
      ,[DMPerDayLim]
      ,[DMDailyIncreaseLim]
      ,[DMMaxPerDayLim]
      ,[FollUserProfileImage]
      ,[FollUserMinPosts]
      ,[FollUserPostsLastXDays]
      ,[FollUseBlackList]
      ,[FollCheckPostsCapBlackList]
      ,[FollDoNotFollowPrivateUser]
      ,[FollSkipBizAcct]
      ,[FollDoNotFollowUsernamewithdigits]
      ,[FollOnlyFollowGender]
      ,[FollUserLangs]
      ,[AfterFollLikeuserPosts]
      ,[AfterFollLikeuserPostsWaitSecsFrom]
      ,[AfterFollLikeuserPostsWaitSecsTo]
      ,[AfterFollCommentUserPosts]
      ,[AfterFollViewUserStory]
      ,[AfterFollMuteUser]
      ,[FollEngageDaily]
      ,[FollEngageDailyfollCountFrmUnFollowList]
      ,[FollEngageLikeRecentPost]
      ,[FollEngageEnableLikeCommAfterPostLike]
      ,[FollEngageSendDMAfterLike]
      ,[FollEngageViewUserStoryAfterLike]
      ,[UnFollFollowersAfterMinDays]
      ,[UnFollFollowersAfterMinDaysVal]
      ,[UnFollDoNotUnFollowLikersOfPosts]
      ,[UnFollDoNotUnFollowLikersOfPostsCount]
      ,[UnFollDoNotUnFollowCommThatCommented]
      ,[UnFollDoNotUnFollowCommThatCommentedCount]
      ,[UnFollUseWhiteList]
      ,[StorVwOnlyPostXMin]
      ,[StorVwOnlyPostXMinVal]
      ,[StorVwLikeUsrPostRcntPost]
      ,[StorVwLikeUsrPostRcntPostVal]
      ,[StorVwReply]
      ,[StorVwSendDMAfterLike]
      ,[CommUsrRecentPosts]
      ,[CommUsrRecentPostsVal]
      ,[CommUsrRecentPostsTypes]
      ,[CommFltrPostsByLikeCount]
      ,[CommDelCommAfterXDays]
      ,[CommDelCommAfterXDaysCount]
      ,[DMShowUnReadMsg]
      ,[DMShowPendingReq]
      ,[LEXLikeUsrMostRecentPosts]
      ,[LEXLikeUsrMostRecentPostsCount]
      ,[LEXUseBlackList]
      ,[LEXChkPostCaptionsforBlackList]
  FROM [dbo].[SocialProfile_Instagram_TargetingInformation]
  where IsSystem = 1




	--values(
	--	@iSocialProfileId,
	--	'Dogs,Cats',
	--	'Snake',
	--	'New York,London',
	--	'Moon',
	--	1,
	--	3,
	--	1,
	--	'Fitness',
	--	'',
	--	getdate(),
	--	@rvcEmailAddress,
	--	getdate(),
	--	@rvcEmailAddress,
	--	1,
	--	2
	--)

     END

exec [dbo].[SG2_usp_Customers_Get] @iCustomerId

End


GO
PRINT N'Creating [dbo].[SG2_usp_Customer_SignUpCustomerWithPreference]'
GO
  
CREATE Procedure [dbo].[SG2_usp_Customer_SignUpCustomerWithPreference]  
 @rvcFirstName   Nvarchar(50),  
    @rvcLastName   Nvarchar(50),  
    @rvcEmailAddress  Nvarchar(64),  
    @rvcPassword   Nvarchar(64),  
 @rvcGUID    Nvarchar(50),  
 @rvcLastLoginIP   Nvarchar(20),  
    @rvcPreference1   nvarchar(255) ,  
 @rvcPreference2   nvarchar(255) ,  
 @rvcPreference3   nvarchar(255) ,  
 @rvcPreference4   nvarchar(255) ,  
 @iPreference5   int,  
 @iPreference6   int,  
 @rvcCity          int= null,  
 @rvcStatusId   Int=1  
As    
   
  -- Searches for Customers based on given parameters    
 Declare @iCustomerId int,@iSocialProfileId int  
 If not exists(Select 1 From dbo.[Customer] where CustomerId= @iCustomerId )   
 Begin  
  
  INSERT INTO [dbo].[Customer]  
           ([GUID]  
           ,[FirstName]  
           ,[SurName]  
           ,[EmailAddress]  
           ,[Password]  
           ,[CreatedOn]  
           ,[CreatedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy]  
           ,[StatusId]  
           ,[LastLoginDate]  
           ,[LoginAttempts]  
           ,[LastLoginIP]  
           ,[Tocken]  
     --,JVBoxStatusId  
     )  
     VALUES  
           ( @rvcGUID  
           ,@rvcFirstName  
           ,@rvcLastName  
           ,@rvcEmailAddress  
           ,@rvcPassword  
           ,GETDATE()  
           ,''  
           ,GETDATE()  
           ,''  
           ,5--@rvcStatusId  
           ,Getdate()  
           ,0  
           ,@rvcLastLoginIP  
           ,0  
     --,11  
     )  
  
  SELECT @iCustomerId= @@IDENTITY  
  
  Begin  
   Insert into dbo.Customer_ContactDetail   
   (  
    [CustomerId],     
    [GUID]  
   )  
   Values (  
    @iCustomerId,  
    NEWID()  
   )  
  END  
  
 END  
 Begin  
  If Not exists (Select 1 From [dbo].[SocialProfile] where [CustomerId]= @iCustomerId)  
  BEGIN  
   INSERT INTO [dbo].[SocialProfile]  
           ([CustomerId]  
           ,[SocialProfileTypeId]  
         
           ,[StatusId]  
           ,[StripeCustomerId]  
           ,[SocialUsername]  
           ,[SocialPassword]  
           ,[SocialProfileName]  
         
           ,[CreatedOn]  
           ,[CreatedBy]  
           ,[UpdatedOn]  
           ,[UpdatedBy])  
     VALUES  
           (@iCustomerId  
           ,30  
         
           ,5  
           ,null  
           ,null  
           ,null  
           ,'Social Profile 1'  
         
           ,getdate()  
           ,@rvcEmailAddress  
           ,getdate()  
           ,@rvcEmailAddress  
     )  
  
   SELECT @iSocialProfileId= @@IDENTITY  
  END  
  ELSE  
  BEGIN   
   SELECT TOP 1 @iSocialProfileId=[SocialProfileId]   
    From [dbo].[SocialProfile] where [CustomerId]= @iCustomerId order by CreatedOn desc  
  END  
 --If not exists(Select 1 From [dbo].[SocialProfile_Instagram_TargetingInformation] where [SocialProfileId]= @iSocialProfileId )   
 --Begin  
 -- INSERT INTO [dbo].[SocialProfile_Instagram_TargetingInformation]  
 --          ([SocialProfileId]  
 --          ,[Preference1]  
 --          ,[Preference2]  
 --          ,[Preference3]  
 --          ,[Preference4]  
 --          ,[Preference5]  
 --          ,[Preference6]  
 --          --,[Preference7]  
 --          --,[Preference8]  
 --          --,[Preference9]  
 --          --,[Preference10]  
 --          --,[InstaUser]  
 --          --,[InstaPassword]  
 --    ,CreatedOn,  
 --  UpdatedOn  
 --  -- City  
 --    )  
 --   VALUES  
 --   (@iSocialProfileId ,  
 --    @rvcPreference1 ,  
 --    @rvcPreference2 ,  
 --    @rvcPreference3,   
 --    @rvcPreference4 ,  
 --    @iPreference5 ,  
 --    @iPreference6 ,  
 --    --@iPreference7 ,  
 --    --@rvcPreference8,   
 --    --@rvcPreference9 ,  
 --    --@rvcPreference10,  
 --    --@rvcInstaUser ,  
 --    --@rvcInstaPassword,  
 --    getdate(),  
 --    GETDATE()  
       
  
 --   )  
 --END  
  
 SELECT TOP 1  
  GUID,  
  C.CustomerId,  
  C.[FirstName],  
  [EmailAddress],  
  [SurName],  
  [Password],  
  C.StripeCustomerId,  
  SP.[SocialProfileId]  
 From [dbo].[Customer]  C  
  inner join [dbo].[SocialProfile] SP ON C.[CustomerId]=SP.[CustomerId]  
   AND SP.[SocialProfileId]=@iSocialProfileId  
 WHERE C.CustomerId= @iCustomerId  
  
End  
  
GO
PRINT N'Creating [dbo].[SystemUser]'
GO
CREATE TABLE [dbo].[SystemUser]
(
[SystemUserId] [int] NOT NULL IDENTITY(1, 1),
[Title] [nvarchar] (5) NULL,
[FirstName] [nvarchar] (50) NOT NULL,
[LastName] [nvarchar] (50) NOT NULL,
[Email] [nvarchar] (50) NOT NULL,
[SystemRoleId] [smallint] NOT NULL,
[Password] [nvarchar] (50) NOT NULL,
[StatusId] [smallint] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NOT NULL,
[ModifiedOn] [datetime] NOT NULL,
[ModifiedBy] [nvarchar] (50) NOT NULL,
[HostUser] [bit] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemUsers] on [dbo].[SystemUser]'
GO
ALTER TABLE [dbo].[SystemUser] ADD CONSTRAINT [PK_SG2_SystemUsers] PRIMARY KEY CLUSTERED  ([SystemUserId])
GO
PRINT N'Creating [dbo].[SystemRole]'
GO
CREATE TABLE [dbo].[SystemRole]
(
[RoleId] [smallint] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (50) NOT NULL,
[StatusId] [smallint] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NOT NULL,
[ModifiedOn] [datetime] NOT NULL,
[ModifiedBy] [nvarchar] (50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemRoles] on [dbo].[SystemRole]'
GO
ALTER TABLE [dbo].[SystemRole] ADD CONSTRAINT [PK_SG2_SystemRoles] PRIMARY KEY CLUSTERED  ([RoleId])
GO
PRINT N'Creating [dbo].[SG2_usp_Get_AllUser]'
GO
  
CREATE Procedure [dbo].[SG2_usp_Get_AllUser]  
   
As    
Begin  
  
SELECT Distinct [SystemUserId] as UserId,FirstName as UserName,SR.[Name] as RoleName  FROM [dbo].[SystemUser] SU  
        LEFT JOIN SystemRole SR ON SU.SystemRoleId=SR.RoleId  
    
End  
  
GO
PRINT N'Creating [dbo].[SG2_usp_Get_CustomerOrderHistory]'
GO

CREATE Procedure [dbo].[SG2_usp_Get_CustomerOrderHistory] --1,1,1,1
  @riCustomerId int,
  @riSocialProfileId int,
  @riPageNumber Int=1,
  @riPageSize varchar(8)=50

As
Begin
--declare @riCustomerId int = 3;

Declare @iFirstRow Int    
 Declare @iLastRow Int 

 Declare @xmlSearchCriteria Xml
 
 
 --Set @xmlSearchCriteria = dbo.udf_UTL_UpdateWildcardInput(@rsSearchCriteria)

 Set @iFirstRow  = (@riPageNumber-1) * @riPageSize + 1    
 Set @iLastRow = @riPageSize + @iFirstRow - 1 

 Declare  @tbResult table(
 RowNumber     int ,
 SubscrpName     nvarchar(250),
 StartDate datetime,
 EndDate   datetime,
 Price     decimal(18,2),
 [Status]    nvarchar(20),
 
 SocialProfileStatus	nvarchar(50),
 UserName				nvarchar(250),
 Email					nvarchar(250),
 SProfileName			nvarchar(250),
 SProfileUsrName		nvarchar(250),
 TotalRecord int 
 ) 


	;with cte as (
	SELECT		StartDate as StartDate,
				EndDate as EndDate,
				S.Name as SubscrpName,
				Price as Price,
				SP.[SocialUsername],
				SP.[SocialProfileName],
				Isnull(C.[FirstName],'') + Isnull(C.[SurName],'') as UserName,
				C.[EmailAddress] as Email,
				
				EV2.[Name] as SocialProfileStatus,
				EV3.[Name] as SubscriptionStatus,
				 ROW_NUMBER() OVER (PARTITION BY S.[SocialProfileId] ORDER BY S.StartDate desc) AS RankId
	
		 FROM [dbo].[SocialProfile_Payments] S
		 Inner Join [dbo].[SocialProfile] SP ON SP.[SocialProfileId]=S.[SocialProfileId]
												AND SP.SocialProfileId= @riSocialProfileId	
		 inner Join [dbo].[Customer] C on C.[CustomerId]=SP.[CustomerId]	
	
		 Left join 	[dbo].[EnumerationValue]	EV2 on EV2.[EnumerationValueId]	=	SP.	[StatusId]
		 Left join 	[dbo].[EnumerationValue]	EV3 on EV3.[EnumerationValueId]	=	S.	[StatusId]
		where SP.[CustomerId]=@riCustomerId

)

		Insert Into @tbResult(
		RowNumber  ,
		SubscrpName,
		StartDate,
		EndDate, 
		[Status]  , 
		Price,
		TotalRecord,
		
		SocialProfileStatus,
		UserName		,	
		Email	,			
		SProfileName,		
		SProfileUsrName	
		)

		Select 
		RankId, 
		SubscrpName ,
		StartDate,
		EndDate,
		SubscriptionStatus,
		 Price,
		(Select Count(1) From cte) TotalRecord,

		SocialProfileStatus,
		Username,
		Email,
		SocialProfileName,
		SocialUsername
		From cte 

	 SELECT StartDate,
			EndDate, 
			SubscrpName,
			Case When [Status] = 'Unsubscribe' Then 'Expired' Else [Status] End [Status], 
			TotalRecord,
			Price,
			
			SocialProfileStatus,
			UserName,	
			Email,			
			SProfileName,		
			SProfileUsrName
	 From @tbResult 
	 Where RowNumber Between @iFirstRow And @iLastRow
 
 Return @@Error 
	
End

GO
PRINT N'Creating [dbo].[SG2_usp_Get_Customer_Instagram_TargetingInformation]'
GO
  
CREATE Procedure [dbo].[SG2_usp_Get_Customer_Instagram_TargetingInformation]  
  @riSocialProfileId int  
  
As  
Begin  
   

SELECT [TargetingInformationId]
      ,CTI.[SocialProfileId]
      ,[IsSystem]
      ,[FollowOn]
      ,[LikeOn]
      ,[UnFollowOn]
      ,[StoryViewerOn]
      ,[ContactMembersOn]
      ,[HashTagsToEngage]
      ,[HashTagsToNotEngage]
      ,[LocationsToEngage]
      ,[GenderEngagmentPref]
      ,[IncludeBusinessAccounts]
      ,[MonOper]
      ,[TueOper]
      ,[WedOper]
      ,[ThuOper]
      ,[FriOper]
      ,[SatOper]
      ,[SunOper]
      ,[ExecutionIntervals]
      ,[RandomizeIntervalsDaily]
      ,CTI.[CreatedOn]
      ,CTI.[CreatedBy]
      ,CTI.[UpdatedOn]
      ,CTI.[UpdatedBy]
      ,[SocialAccAs]
      ,[FollNewPerDayLim]
      ,[FollDailyIncreaseLim]
      ,[FollMaxPerDayLim]
      ,[UnFollNewPerDayLim]
      ,[UnFollDailyIncreaseLim]
      ,[UnFollMaxPerDayLim]
      ,[LikePerDayLim]
      ,[LikeDailyIncreaseLim]
      ,[LikeMaxPerDayLim]
      ,[ViewStoriesPerDayLim]
      ,[ViewStoriesDailyIncreaseLim]
      ,[ViewStoriesMaxPerDayLim]
      ,[CommentPerDayLim]
      ,[CommentDailyIncreaseLim]
      ,[CommentMaxPerDayLim]
      ,[DMPerDayLim]
      ,[DMDailyIncreaseLim]
      ,[DMMaxPerDayLim]
      ,[FollUserProfileImage]
      ,[FollUserMinPosts]
      ,[FollUserPostsLastXDays]
      ,[FollUseBlackList]
      ,[FollCheckPostsCapBlackList]
      ,[FollDoNotFollowPrivateUser]
      ,[FollSkipBizAcct]
      ,[FollDoNotFollowUsernamewithdigits]
      ,[FollOnlyFollowGender]
      ,[FollUserLangs]
      ,[AfterFollLikeuserPosts]
      ,[AfterFollLikeuserPostsWaitSecsFrom]
      ,[AfterFollLikeuserPostsWaitSecsTo]
      ,[AfterFollCommentUserPosts]
      ,[AfterFollViewUserStory]
      ,[AfterFollMuteUser]
      ,[FollEngageDaily]
      ,[FollEngageDailyfollCountFrmUnFollowList]
      ,[FollEngageLikeRecentPost]
      ,[FollEngageEnableLikeCommAfterPostLike]
      ,[FollEngageSendDMAfterLike]
      ,[FollEngageViewUserStoryAfterLike]
      ,[UnFollFollowersAfterMinDays]
      ,[UnFollFollowersAfterMinDaysVal]
      ,[UnFollDoNotUnFollowLikersOfPosts]
      ,[UnFollDoNotUnFollowLikersOfPostsCount]
      ,[UnFollDoNotUnFollowCommThatCommented]
      ,[UnFollDoNotUnFollowCommThatCommentedCount]
      ,[UnFollUseWhiteList]
      ,[StorVwOnlyPostXMin]
      ,[StorVwOnlyPostXMinVal]
      ,[StorVwLikeUsrPostRcntPost]
      ,[StorVwLikeUsrPostRcntPostVal]
      ,[StorVwReply]
      ,[StorVwSendDMAfterLike]
      ,[CommUsrRecentPosts]
      ,[CommUsrRecentPostsVal]
      ,[CommUsrRecentPostsTypes]
      ,[CommFltrPostsByLikeCount]
      ,[CommDelCommAfterXDays]
      ,[CommDelCommAfterXDaysCount]
      ,[DMShowUnReadMsg]
      ,[DMShowPendingReq]
      ,[LEXLikeUsrMostRecentPosts]
      ,[LEXLikeUsrMostRecentPostsCount]
      ,[LEXUseBlackList]
      ,[LEXChkPostCaptionsforBlackList]


      ,SP.SocialUsername as SocialUsername  
      ,SP.[SocialPassword]  
   ,C.CustomerId,  
   SP.SocialProfileName as SocialProfileName,  
   CTI.SocialProfileId,  

   C.[EmailAddress] as Email,  
   Isnull(C.[FirstName],'') + Isnull(C.[SurName],'') as UserName,  

    (SELECT COUNT(CustomerId) from [dbo].[SocialProfile] where CustomerId=C.CustomerId) as NoOfProfile,  
  
    SocialAccAs,  
    SP.VerificationCode as VerificationCode,  
  
    C.Comment,  
    ISNULL(SP.IsArchived,0) as IsArchived  
     FROM  [dbo].[SocialProfile_Instagram_TargetingInformation] CTI  
   INNER JOIN [dbo].[SocialProfile] SP ON CTI.[SocialProfileId]=SP.[SocialProfileId]  
 --AND SP.SocialProfileId=@riSocialProfileId  
   INNER JOIN [dbo].[Customer] C ON C.[CustomerId]=SP.[CustomerId]  

       where CTI.SocialProfileId=@riSocialProfileId  
End  
  
GO
PRINT N'Creating [dbo].[Enumeration]'
GO
CREATE TABLE [dbo].[Enumeration]
(
[EnumerationId] [smallint] NOT NULL,
[Name] [nvarchar] (50) NOT NULL,
[Description] [nvarchar] (255) NOT NULL
)
GO
PRINT N'Creating [dbo].[SG2_usp_Get_EnumerationValue]'
GO

CREATE Procedure [dbo].[SG2_usp_Get_EnumerationValue]

As
Begin
 

 Select E.[Name] as Enumeration , EV.EnumerationValueId , EV.[Name]  
 
 
  FROM  Enumeration E inner join EnumerationValue EV
						ON E.EnumerationId=EV.EnumerationId
  WHERE EV.IsVisible = 1
  order by Enumeration, SequenceNo 
End


GO
PRINT N'Creating [dbo].[SG2_usp_Get_SocialProfile_PaymentPlan]'
GO
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
GO
PRINT N'Creating [dbo].[SG2_usp_Get_SpecificCustomerDetail]'
GO
  
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
  
GO
PRINT N'Creating [dbo].[SG2_usp_GetUserDetailsForbackOffice]'
GO
CREATE Procedure [dbo].[SG2_usp_GetUserDetailsForbackOffice]  
  @rsSearchCrite Nvarchar(MAX),  
  @riPageNumber Int,  
  @riPageSize varchar(8),  
  @riStatusId int=null,  
  @riProductId varchar(250)=null,  
  @riJVStatus varchar(250)=null,  
  @riSubscription int=null  
  
As  
Begin  
   
 -- Searches for Products based on given parameters    
 Declare @iFirstRow Int      
 Declare @iLastRow Int   
  
 Declare @xmlSearchCriteria Xml  
   
 IF @riStatusId=''  
 SET @riStatusId=NULL  
  
 IF @riJVStatus=''  
 SET @riJVStatus=NULL  
 --Set @xmlSearchCriteria = dbo.udf_UTL_UpdateWildcardInput(@rsSearchCriteria)  
  
 Set @iFirstRow  = (@riPageNumber-1) * @riPageSize + 1      
 Set @iLastRow = @riPageSize + @iFirstRow - 1   
   
 Declare  @tbResult table(  
 RowNumber     int ,  
 InstaName     nvarchar(250),  
 UserName      nvarchar(250),  
 CustomerId    int,  
 Products      nvarchar(250),  
 ProxyIPNumber nvarchar(15),  
 BoxName       nvarchar(250),  
 [Status]      nvarchar(100),  
 JVBoxStatus nvarchar(100),  
 SocialProfileName  nvarchar(250),  
 SocialProfileId   int,  
 CustomerEmail    nvarchar(250)  
  
 )   
  
 ;With CTE As  
 (   
  -- Get all Product information to create index  
    
  Select Distinct  
   SP.[SocialUsername] as SocialAccountName,  
   SP.[SocialProfileName] as SocialProfileName,  
   SP.SocialProfileId,  
   ISNULL(Customer.FirstName,'') + ' ' + ISNULL(Customer.SurName,'')  as [Name],  
   Customer.CustomerId as CustomerId,  
   SubS.Name as Products,  
  
   EV.[Name] as [Status],  
   Customer.updatedon,  
  
   customer.[EmailAddress] as EmailAddress ,  
            ROW_NUMBER() OVER (PARTITION BY SP.[SocialProfileId] ORDER BY SubS.StartDate desc) AS RankId  
  From dbo.Customer Customer With (Nolock)   
  Inner join [dbo].[SocialProfile] SP ON SP.CustomerId=Customer.CustomerId  

 left join [dbo].[SocialProfile_Payments] SubS   
  ON SubS.[SocialProfileId]=SP.[SocialProfileId]   
    --AND SubS.StatusId=25  
 left join [dbo].[SocialProfile_Instagram_TargetingInformation] TI   
  On TI.[SocialProfileId]=SP.[SocialProfileId]  
 Left join dbo.EnumerationValue EV   
  ON EV.EnumerationValueId=SP.StatusId --And EV.EnumerationId=4  
 Where (  
   ((@riStatusId is null) or Customer.StatusId = @riStatusId)  
  
   AND   
     (((@riProductId is null) or @riProductId ='' ) or  SubS.[StripePlanId] = @riProductId)  
  AND ((@riSubscription is null) or SubS.StatusId= @riSubscription)  
  And (   
    (@rsSearchCrite is null or @rsSearchCrite = '')   
    or (SP.[SocialProfileName] like '%' +@rsSearchCrite +'%'   
    or Customer.UserName like '%' +@rsSearchCrite +'%'  
     or SubS.Name like '%' +@rsSearchCrite +'%'   
   
     or EV.Name like '%' +@rsSearchCrite +'%'   
     or Customer.FirstName like '%' +@rsSearchCrite +'%'  
     or Customer.SurName like '%' +@rsSearchCrite +'%'  
     or Customer.[EmailAddress] like '%' +@rsSearchCrite +'%'  
     or SP.[SocialUsername] like '%' +@rsSearchCrite +'%'   
       
      )  
   )  
 )  
       )  
    
Insert into @tbResult(  
RowNumber  ,    
InstaName  ,    
UserName   ,    
CustomerId ,    
Products   ,    

[Status]   ,  

SocialProfileName,  
SocialProfileId,  
CustomerEmail  
  
)  
SELECT Distinct  
 ROW_NUMBER() Over (    
           Order By UpdatedOn desc  
            ) As RowNumber,  
SocialAccountName,[Name],CustomerId,Products,[Status],SocialProfileName,  
SocialProfileId,EmailAddress  
 FROM CTE  where RankId=1  
  
 Select UserName,    
  InstaName,  
  CustomerId,  
        Products,  
     ProxyIPNumber,  
     BoxName,  
     [Status],  
  JVBoxStatus,  
  SocialProfileName,  
  SocialProfileId,  
  CustomerEmail,  
     (Select Count(1) From @tbResult) TotalRecord  
 From @tbResult   
 Where RowNumber Between @iFirstRow And @iLastRow  
   
 Return @@Error   
End  
  
GO
PRINT N'Creating [dbo].[LikeyAccount]'
GO
CREATE TABLE [dbo].[LikeyAccount]
(
[LikeyAccountId] [int] NOT NULL IDENTITY(1, 1),
[InstaUserName] [nvarchar] (50) NOT NULL,
[InstaPassword] [nvarchar] (50) NULL,
[Country] [nvarchar] (50) NULL,
[City] [nvarchar] (50) NOT NULL,
[Gender] [int] NOT NULL,
[HashTag] [nvarchar] (50) NOT NULL,
[StatusId] [int] NOT NULL
)
GO
PRINT N'Creating primary key [PK_SG2_LikeyAccount] on [dbo].[LikeyAccount]'
GO
ALTER TABLE [dbo].[LikeyAccount] ADD CONSTRAINT [PK_SG2_LikeyAccount] PRIMARY KEY CLUSTERED  ([LikeyAccountId])
GO
PRINT N'Creating [dbo].[SG2_usp_LikeyAccount_GetAll]'
GO
  
CREATE Procedure [dbo].[SG2_usp_LikeyAccount_GetAll]  
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
 SELECT       LikeyAccountId,  
     InstaUserName,  
      EV.Name StatusName,  
      (Select Count(1) From [dbo].LikeyAccount) TotalRecord,  
       ROW_NUMBER() OVER (ORDER BY  LikeyAccountId desc) AS RankId  
   FROM [dbo].LikeyAccount LA  
      INNER JOIN [dbo].EnumerationValue EV ON LA.StatusId= EV.EnumerationValueId  
   INNER JOIN [dbo].Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'General'  
   Where (  
   ((@riStatusId is null) or LA.StatusId = @riStatusId)  
  And (   
    (@rsSearchCrite is null or @rsSearchCrite = '')   
    or (LA.InstaUserName like '%' +@rsSearchCrite +'%' or LA.Country like '%' +@rsSearchCrite +'%' or EV.Name like '%' +@rsSearchCrite +'%' )  
   )  
 )  
 )  
  
 SELECT LikeyAccountId,  
     InstaUserName,  
     StatusName,  
     TotalRecord  
 FROM cte where RankId Between @iFirstRow And @iLastRow  
  
  
 Return @@Error   
End  
GO
PRINT N'Creating [dbo].[SG2_usp_Login_Customers]'
GO
CREATE Procedure [dbo].[SG2_usp_Login_Customers]  
  @rvcEmailAddress Nvarchar(64),  
  @rvcPassword Nvarchar(64),    
  @rvcCreatedBy  Nvarchar(64),  
  @rvcLastLoginIP Nvarchar(20),  
  @rvcStatusId Int=1  
  
   
As    
Begin  
  
 -- Searches for Customers based on given parameters    
  
 declare @customerid int = null;  
  
 SELECT TOP 1 @customerid = customerid FROM  [dbo].[Customer] where EmailAddress=@rvcEmailAddress and Password=@rvcPassword;-- and StatusId=@rvcStatusId;  
   
 exec [dbo].[SG2_usp_Customers_Get] @customerid  
   
      
End  
  
GO
PRINT N'Creating [dbo].[SG2_usp_PlanInformation_GetAll]'
GO
  
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
GO
PRINT N'Creating [dbo].[SG2_usp_Report_GetMostUsedProductData]'
GO
  
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
   FROM [dbo].[SocialProfile_Payments] t  
   WHERE T.StartDate BETWEEN @dtFromDate AND @dtToDate  
  
   SELECT S.Name as PlanName, COUNT(S.PaymentPlanID) PlanSold, @iTotalPlan as TotalPlanSold  
   FROM [dbo].[SocialProfile_Payments] S   
   WHERE S.StartDate BETWEEN @dtFromDate AND @dtToDate  
   GROUP BY S.Name  
  
  
End  
  
GO
PRINT N'Creating [dbo].[SocialProfile_Notification]'
GO
CREATE TABLE [dbo].[SocialProfile_Notification]
(
[Id] [bigint] NOT NULL IDENTITY(1, 1),
[Notification] [nvarchar] (250) NOT NULL,
[StatusId] [smallint] NOT NULL,
[SocialProfileId] [int] NOT NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NOT NULL,
[UpdateOn] [datetime] NOT NULL,
[UpdatedBy] [nvarchar] (50) NOT NULL,
[Mode] [nvarchar] (20) NOT NULL CONSTRAINT [DF__SG2_Social__Mode__6FE99F9F] DEFAULT ('Auto')
)
GO
PRINT N'Creating primary key [PK_SG2_SocialProfile_Notification] on [dbo].[SocialProfile_Notification]'
GO
ALTER TABLE [dbo].[SocialProfile_Notification] ADD CONSTRAINT [PK_SG2_SocialProfile_Notification] PRIMARY KEY CLUSTERED  ([Id])
GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_GetNotificationsByStatus]'
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_GetNotificationsByStatus]
	@StatusId smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
		nt.[Id], 
		nt.[Notification], 
		nt.StatusId, 
		nt.SocialProfileId,
		coalesce(sp.SocialUsername, [EmailAddress]) SocialUsername,
		nt.CreatedOn,
		nt.CreatedBy,
		nt.UpdateOn,
		nt.Updatedby,
		nt.Mode
	from [dbo].[SocialProfile_Notification] nt
		inner join [dbo].[SocialProfile] sp
			on nt.socialprofileid = sp.socialprofileid
		Inner Join [dbo].[Customer] c
			on c.customerid = sp.customerid
	where nt.StatusId  in (51,52)
		order by createdOn desc
END
GO
PRINT N'Creating [dbo].[udf_utl_CSVtoTwelveColumns]'
GO
CREATE FUNCTION [dbo].[udf_utl_CSVtoTwelveColumns] (@CSVList varchar(8000))
RETURNS @tbl TABLE (
  AccountName VARCHAR(max),
  SatDate VARCHAR(max),
  FollowersGain VARCHAR(max), 
  Followers VARCHAR(max),
  Followings VARCHAR(max),
  Joiner VARCHAR(max),
  Ujoiner VARCHAR(max),
  Follow VARCHAR(max),
  Unfollow VARCHAR(max),
  ContactMessages VARCHAR(max),
  ContactFriends VARCHAR(max),
  [Re(pin/tweet/blog)] VARCHAR(max),
  [Like] VARCHAR(max),
  Comment VARCHAR(max),
  Engagement VARCHAR(max),
  Repost VARCHAR(max),
  LikeComments VARCHAR(max),
  StoryViewer VARCHAR(max),
  BlockedFollowers VARCHAR(max)
  ) AS

BEGIN
 
 Declare @Table Table(Col1 varchar(Max),Col2 varchar(100))
 declare @vcColumnName varchar(max)

  IF RIGHT(@CSVList, 1) <> ','
    SELECT @CSVList = @CSVList + ','

    DECLARE @Pos    BIGINT,
            @OldPos BIGINT
    SELECT  @Pos    = 1,
            @OldPos = 1
 
 Declare @iIndex int = 1;
 
    WHILE   @Pos < LEN(@CSVList)
        BEGIN
            SELECT  @Pos = CHARINDEX(',', @CSVList, @OldPos)
            INSERT INTO @Table
            SELECT  LTRIM(RTRIM(SUBSTRING(@CSVList, @OldPos, @Pos - @OldPos))) Col001, 
     'Column' + Cast(@iIndex as varchar)

            SELECT  @OldPos = @Pos + 1
            Select @iIndex = @iIndex + 1;
        END
        
 Insert Into @tbl( AccountName, SatDate, FollowersGain , Followers, Followings, Joiner, Ujoiner,
  Follow, Unfollow, ContactMessages, ContactFriends, [Re(pin/tweet/blog)],[Like],Comment,Engagement,
  Repost,LikeComments,StoryViewer,BlockedFollowers )
 Select Column1, Column2 , Column3 , Column4, Column5, Column6, Column7, Column8, Column9, Column10,
  Column11, Column12,Column13,Column14,Column15,Column16,Column17,Column18,Column19
 From
 (
   select Col1, Col2
   from @Table
 ) d
 pivot
 (
   max(Col1)
   for Col2 in (Column1, Column2 , Column3 , Column4, Column5, Column6, Column7, Column8, Column9, Column10,
  Column11, Column12,Column13,Column14,Column15,Column16,Column17,Column18,Column19)
 ) piv;
 
 Return

END
GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_SaveStatistics]'
GO


CREATE Procedure [dbo].[SG2_usp_SocialProfile_SaveStatistics]
@nvStatisticsDataJson NVARCHAR(MAX) = '["AccountName,Date,FollowersGain,Followers,Followings,Joiner,Ujoiner,Follow,Unfollow,ContactMessages,ContactFriends,Re(pin/tweet/blog),Like,Comment,Engagement,Repost,LikeComments,StoryViewer,BlockedFollowers","Like Exchange PVA 01,6/15/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","Like Exchange PVA 01,6/14/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","OmarChaudhry,6/15/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","OmarChaudhry,6/14/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","Hassan Jamil,6/15/2019,0,39,51(0.76),0,0,0,0,0,0,0,0,0,0,0,0,0,0","Hassan Jamil,6/14/2019,0,39,50(0.78),0,0,0,0,0,0,0,0,0,0,0,0,0,0"]'
As  
BEGIN

--DECLARE @snvStatisticsDataJson NVARCHAR(MAX) = '["AccountName,Date,FollowersGain,Followers,Followings,Joiner,Ujoiner,Follow,Unfollow,ContactMessages,ContactFriends,Re(pin/tweet/blog),Like,Comment,Engagement,Repost,LikeComments,StoryViewer,BlockedFollowers","Like Exchange PVA 01,6/15/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","Like Exchange PVA 01,6/14/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","OmarChaudhry,6/15/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","OmarChaudhry,6/14/2019,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0","Hassan Jamil,6/15/2019,0,39,51(0.76),0,0,0,0,0,0,0,0,0,0,0,0,0,0","Hassan Jamil,6/14/2019,0,39,50(0.78),0,0,0,0,0,0,0,0,0,0,0,0,0,0"]'

DECLARE @iID INT, @nvStatisticValue nvarchar(max)


SELECT *
INTO  #tblStatisticData  FROM OPENJSON(@nvStatisticsDataJson)


--Select * from #tblStatisticData


SELECT 
Top 1 
@iID = tblSD.[Key],
@nvStatisticValue = tblSD.Value
FROM #tblStatisticData tblSD 
Where tblSD.[Key] <> 0

WHILE @@rowcount <> 0
BEGIN


INSERT INTO [dbo].[SG2_SocialProfile_Statistics](
 [SocialProfileId], [Username], [Date], [FollowersGain], [Followers], [Followings], [Joiner], [Unjoiner], [Follow], [Unfollow], [ContactMassage], [ContactFriends], [REPinTweetBlog], [Bump], [Like], [Comment], [Engagement], [Repost], [LikeComments], [StoryViewer], [BlockedFollowers], [CreatedDate], [UpdateDate])


Select  1,AccountName,SatDate, [FollowersGain], [Followers],  (CASE WHEN  CHARINDEX('(', [Followings]) > 0 THEN ISNULL(left([Followings], charindex('(', [Followings])-1),0) ELSE [Followings] END ),
 [Joiner], Ujoiner, [Follow], [Unfollow], ContactMessages, [ContactFriends], [Re(pin/tweet/blog)], null, [Like], [Comment], [Engagement], [Repost], [LikeComments], [StoryViewer], [BlockedFollowers],GetDATE(),GETDATE()
from udf_utl_CSVtoTwelveColumns(@nvStatisticValue)

  --select left('51(0.76)', charindex('(', '51(0.76)')-1)


DELETE FROM  #tblStatisticData WHERE  [Key] = @iID
SELECT 
Top 1 
@iID = tblSD.[Key],
@nvStatisticValue = tblSD.Value
FROM #tblStatisticData tblSD 
Where tblSD.[Key] <> 0

END

drop table #tblStatisticData




End

GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_Payments_Save]'
GO
  
CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_Payments_Save]  
(  
 @riSocialProfileId     INT,  
 @riStripeSubscriptionId    NVARCHAR(255),  
 @rvcDescription      NVARCHAR(250),  
 @rvcName       NVARCHAR(255),  
 @riPrice       Decimal(18,2),  
 @riStripePlanId      NVARCHAR(255),  
 @rvcSubscriptionType    NVARCHAR(255),  
 @rdtStartDate      datetime,  
 @rdtEndDate       datetime,  
 @riStatusId       Int,  
 @riPaymentPlanId     Int,  
 @rvcStripeInvoiceId     NVARCHAR(255)  
)  
   
AS    
   
   
BEGIN  
   declare @subId int;  
  
      Update [dbo].[SocialProfile_Payments]  
   Set StatusId=27  
   where [SocialProfileId]=@riSocialProfileId  
   AND StatusId not in (26,27)  
  
    
   INSERT INTO [dbo].[SocialProfile_Payments]  
           ([Name]  
           ,[Description]  
           ,[SubscriptionType]  
           ,[Price]  
           ,[StartDate]  
           ,[EndDate]  
           ,[SocialProfileId]  
           ,[StripeSubscriptionId]  
           ,[StatusId]  
           ,[StripePlanId]  
           ,[PaymentPlanId]  
     ,StripeInvoiceId  
     )  
      VALUES  
      (  
       @rvcName,  
       @rvcDescription,  
       @rvcSubscriptionType,  
       @riPrice,  
       @rdtStartDate,  
       @rdtEndDate,  
       @riSocialProfileId,  
       @riStripeSubscriptionId,  
       @riStatusId,  
       @riStripePlanId,  
       (select top 1 [PlanId] from [dbo].[SocialProfile_PaymentPlan] where [StripePlanId] = @riStripePlanId),  
      @rvcStripeInvoiceId  
       )  
    
  
 select @subId = @@identity  
   
 SELECT [SubscriptionId]  
      ,[Name]  
      ,[Description]  
      ,[SubscriptionType]  
      ,[Price]  
      ,[StartDate]  
      ,[EndDate]  
      ,[SocialProfileId]  
      ,[StripeSubscriptionId]  
      ,[StatusId]  
      ,[StripePlanId]  
      ,[PaymentPlanId]  
   ,StripeInvoiceId  
  FROM [dbo].[SocialProfile_Payments]  
 where [SubscriptionId] = @subId  
    
END  
GO
PRINT N'Creating [dbo].[SystemConfig]'
GO
CREATE TABLE [dbo].[SystemConfig]
(
[ConfigId] [smallint] NOT NULL IDENTITY(1, 1),
[ConfigKey] [nvarchar] (50) NOT NULL,
[ConfigValue] [nvarchar] (250) NOT NULL,
[ConfigValue2] [nvarchar] (250) NULL,
[CreatedOn] [datetime] NOT NULL,
[CreatedBy] [nvarchar] (50) NOT NULL,
[ModifiedOn] [datetime] NOT NULL,
[ModifiedBy] [nvarchar] (50) NOT NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemConfig] on [dbo].[SystemConfig]'
GO
ALTER TABLE [dbo].[SystemConfig] ADD CONSTRAINT [PK_SG2_SystemConfig] PRIMARY KEY CLUSTERED  ([ConfigId])
GO
PRINT N'Creating [dbo].[SG2_usp_SystemConfig_GetAll]'
GO

CREATE Procedure [dbo].[SG2_usp_SystemConfig_GetAll]
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

Select   
        [ConfigId],
		[ConfigKey], 
		[ConfigValue] as ConfigValue1,
		[ConfigValue2] as ConfigValue2,
     (Select Count(1) From [dbo].[SystemConfig]) TotalRecord
from [dbo].[SystemConfig] SC
Where  ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (SC.ConfigKey like '%' +@rsSearchCrite +'%' or SC.ConfigValue like '%' +@rsSearchCrite +'%' or SC.ConfigValue2 like '%' +@rsSearchCrite +'%' )
			)


 Return @@Error 
End
GO
PRINT N'Creating [dbo].[SG2_usp_SystemUser_GetAll]'
GO
  
CREATE Procedure [dbo].[SG2_usp_SystemUser_GetAll]  
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
  
Select     
         SU.SystemUserId,  
   SU.FirstName + ' ' +  SU.LastName As FullName,  
   SR.Name As RoleName,  
   EV.Name As StatusName,  
   SU.Email,  
     (Select Count(1) From [dbo].[SystemUser]) TotalRecord  
from [dbo].SystemUser SU  
    INNER JOIN SystemRole SR ON SU.SystemRoleID = SR.RoleId  
    INNER JOIN [dbo].EnumerationValue EV ON SU.StatusId= EV.EnumerationValueId  
    INNER JOIN [dbo].Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'SystemUser'  
Where SU.HostUser = 0  
 AND (   
   (@riStatusId is null) or SU.StatusId = @riStatusId)  
   AND ((@rsSearchCrite is null or @rsSearchCrite = '')   
    or (SU.FirstName like '%' +@rsSearchCrite +'%'   
    or SU.LastName like '%' +@rsSearchCrite +'%'   
      ))  
  
  
 Return @@Error   
End  
GO
PRINT N'Creating [dbo].[SG2_usp_SystemUser_Login]'
GO
CREATE Procedure [dbo].[SG2_usp_SystemUser_Login]  
  @rvcEmailAddress Nvarchar(64),  
  @rvcPassword Nvarchar(64)  
  
   
As    
Begin  
  
 -- Searches for Customers based on given parameters    
  
SELECT [SystemUserId], [Title], [FirstName],   
  [LastName], [Email], [SystemRoleId],   
  [Password], SU.[StatusId], SU.[CreatedOn],  
   SU.[CreatedBy], SU.[ModifiedOn], SU.[ModifiedBy], SU.[HostUser],  
   SR.Name AS RoleName  
FROM  [dbo].SystemUser SU  
   INNER JOIN [dbo].[SystemRole] SR ON SU.SystemRoleId = SR.RoleId  
WHERE Email = @rvcEmailAddress AND PASSWORD= @rvcPassword   
   
   
      
End  
  
GO
PRINT N'Creating [dbo].[SocialProfile_Actions]'
GO
CREATE TABLE [dbo].[SocialProfile_Actions]
(
[SPSHId] [bigint] NOT NULL IDENTITY(1, 1),
[SocialProfileId] [int] NULL,
[ActionID] [int] NULL,
[TargetProfile] [nvarchar] (100) NULL,
[ActionDateTime] [datetime] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SocialProfile_StatusHistory] on [dbo].[SocialProfile_Actions]'
GO
ALTER TABLE [dbo].[SocialProfile_Actions] ADD CONSTRAINT [PK_SG2_SocialProfile_StatusHistory] PRIMARY KEY CLUSTERED  ([SPSHId])
GO
PRINT N'Creating [dbo].[StringSplit]'
GO
CREATE FUNCTION [dbo].[StringSplit]
(
	@String  VARCHAR(MAX), @Separator CHAR(1)
)
RETURNS @RESULT TABLE(Value VARCHAR(MAX))
AS
BEGIN      
 DECLARE @SeparatorPosition INT = CHARINDEX(@Separator, @String ),
		@Value VARCHAR(MAX), @StartPosition INT = 1

 IF @SeparatorPosition = 0	
  BEGIN	
   INSERT INTO @RESULT VALUES(@String)
   RETURN
  END
	
 SET @String = @String + @Separator
 WHILE @SeparatorPosition > 0
  BEGIN
   SET @Value = SUBSTRING(@String , @StartPosition, @SeparatorPosition- @StartPosition)

   IF( @Value <> ''  ) 
    INSERT INTO @RESULT VALUES(@Value)
  
   SET @StartPosition = @SeparatorPosition + 1
   SET @SeparatorPosition = CHARINDEX(@Separator, @String , @StartPosition)
  END     
	
 RETURN
END
GO
PRINT N'Creating [dbo].[Customer_Title]'
GO
CREATE TABLE [dbo].[Customer_Title]
(
[PkTitleId] [int] NOT NULL IDENTITY(1, 1),
[TitleName] [varchar] (50) NULL
)
GO
PRINT N'Creating primary key [PK_SG2_Title] on [dbo].[Customer_Title]'
GO
ALTER TABLE [dbo].[Customer_Title] ADD CONSTRAINT [PK_SG2_Title] PRIMARY KEY CLUSTERED  ([PkTitleId])
GO
PRINT N'Creating [dbo].[SystemCity]'
GO
CREATE TABLE [dbo].[SystemCity]
(
[CityId] [int] NOT NULL IDENTITY(1, 1),
[CountryId] [smallint] NOT NULL,
[StateId] [smallint] NULL,
[Name] [nvarchar] (50) NOT NULL,
[Code] [nvarchar] (5) NULL,
[StatusId] [smallint] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemCity] on [dbo].[SystemCity]'
GO
ALTER TABLE [dbo].[SystemCity] ADD CONSTRAINT [PK_SG2_SystemCity] PRIMARY KEY CLUSTERED  ([CityId])
GO
PRINT N'Creating [dbo].[SystemCountry]'
GO
CREATE TABLE [dbo].[SystemCountry]
(
[CountryId] [smallint] NOT NULL IDENTITY(1, 1),
[Name] [nvarchar] (50) NOT NULL,
[Code] [nvarchar] (5) NULL,
[PhoneCode] [nvarchar] (5) NULL,
[StatusId] [smallint] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemCountry] on [dbo].[SystemCountry]'
GO
ALTER TABLE [dbo].[SystemCountry] ADD CONSTRAINT [PK_SG2_SystemCountry] PRIMARY KEY CLUSTERED  ([CountryId])
GO
PRINT N'Creating [dbo].[SystemState]'
GO
CREATE TABLE [dbo].[SystemState]
(
[StateId] [smallint] NOT NULL IDENTITY(1, 1),
[CountryId] [smallint] NOT NULL,
[Name] [nvarchar] (50) NULL,
[Code] [nvarchar] (5) NULL,
[StatusId] [smallint] NULL
)
GO
PRINT N'Creating primary key [PK_SG2_SystemStates] on [dbo].[SystemState]'
GO
ALTER TABLE [dbo].[SystemState] ADD CONSTRAINT [PK_SG2_SystemStates] PRIMARY KEY CLUSTERED  ([StateId])
GO
PRINT N'Creating [dbo].[SG2_Delete_Customer]'
GO


CREATE Procedure [dbo].[SG2_Delete_Customer]
@riCustomerId int,
@riSocialProfileId Int

 
As  
Begin

 -- Searches for Customers based on given parameters  
DELETE FROM [dbo].[SG2_SocialProfile_ProxyMapping] where [SocialProfileId]=@riSocialProfileId


UPDATE [dbo].[SG2_SocialProfile] set 

JVBoxStatusId=Case when JVBoxStatusId IS not null then 35 ELSE NULL END,
[JVBoxId]=Case when JVBoxStatusId is null then NULL else JVBoxId  END
,[StatusId]=18
 where [SocialProfileId]=@riSocialProfileId
 AND [CustomerId]=@riCustomerId

 return 1
     
End

GO
PRINT N'Creating [dbo].[SG2_Get_AllCustomers]'
GO

CREATE Procedure [dbo].[SG2_Get_AllCustomers]


 
As  
Begin

 -- Searches for Customers based on given parameters  

SELECT Distinct CustomerId, ISnull(FirstName,'') + ' ' + Isnull(SurName,'') as CustomerName  FROM SG2_Customer
 
 
    
End

GO
PRINT N'Creating [dbo].[SG2_usp_AssignJVBoxToCustomer]'
GO
CREATE Procedure [dbo].[SG2_usp_AssignJVBoxToCustomer]
  @riCustomerId Int,
  @riProfileId int

As  
Begin

	DECLARE @JVBoxId int

	SELECT 
			@JVBoxId=JVBoxId FROM [dbo].[SG2_SocialProfile] 
		where 
			CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

	DECLARE @JVBoxes table(JVBoxId int, MaxCount int, AssignedCount int, JVBoxStatus int)

	if (
		(@JVBoxId is null) 
		and 
		exists(SELECT 1 from [dbo].[SG2_SocialProfile_Payments] where [SocialProfileId]= @riProfileId and StatusId=25))
	Begin 
		INSERT INTO @JVBoxes(JVBoxId,MaxCount,AssignedCount,JVBoxStatus)
		SELECT JVBox.JVBoxId,MaxLimit, Count(C.JVBoxId),JVBox.StatusId FROM SG2_JVBox JVBox  
		Left join [dbo].[SG2_SocialProfile] C 
		On JVBox.JVBoxId=C.JVBoxId
		where  JVBox.StatusId=19
		AND ISNULL(JVBox.QueueStatusId,45) <> 44
		Group by JVBox.JVBoxId,MaxLimit,JVBox.StatusId

		IF Exists(Select 1 from @JVBoxes where AssignedCount<MaxCount and JVBoxStatus=19)
		Begin
			SELECT TOP 1 @JVBoxId=JVBoxId from @JVBoxes where AssignedCount<MaxCount order by JVBoxId asc

			Update [dbo].[SG2_SocialProfile] set JVBoxId=@JVBoxId
			where CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

		END
	END

	Select 
		CustomerId, SP.JVBoxId as JVBoxId ,JV.BoxName as JVBoxName 
		FROM [dbo].[SG2_SocialProfile] SP
			Inner JOIN [dbo].[SG2_JVBox] JV ON SP.JVBoxId=JV.JVBoxId
	 where 
		CustomerId=@riCustomerId and [SocialProfileId]=@riProfileId

End
GO
PRINT N'Creating [dbo].[SG2_usp_Customer_AssignedNearestProxyIP]'
GO
CREATE PROCEDURE [dbo].[SG2_usp_Customer_AssignedNearestProxyIP]
(
	@riCustomer				INT,
	@rfCustomerLatitude		FLOAT,
	@rfCustomerLongitude	FLOAT,
	@riSocialProfileId     INT
)
 
AS  
BEGIN

	 DECLARE @tblAvailableProxyIPs TABLE
	(
		ProxyId INT,
		ProxyIPNumber NVARCHAR(15),
		Latitude FLOAT,
		longitudes FLOAT,
		Distance  FLOAT,
		FreeSlots Int
	)

	DECLARE  @MinDistance FLOAT, @riProxyMappingId INT

	INSERT INTO @tblAvailableProxyIPs
	SELECT P.ProxyId,P.ProxyIPNumber,
 LEFT(P.GeoPoints, charindex(',', P.GeoPoints) - 1) Latitude,
REPLACE(RIGHT(P.GeoPoints, charindex(',', P.GeoPoints) ),',','') longitudes,
 ( 3960 * acos( cos( radians( @rfCustomerLatitude ) ) *
  cos( radians( LEFT(P.GeoPoints, charindex(',', P.GeoPoints) - 1) ) ) * cos( radians(  REPLACE(RIGHT(P.GeoPoints, charindex(',', P.GeoPoints) ),',','') ) - radians( @rfCustomerLongitude ) ) +
  sin( radians( @rfCustomerLatitude ) ) * sin( radians(  LEFT(P.GeoPoints, charindex(',', P.GeoPoints) - 1)  ) ) ) ) AS Distance,
((SELECT COUNT(ProxyId) FROM [dbo].[SG2_SocialProfile_ProxyMapping] WITH(NOLOCK) WHERE ProxyId = P.ProxyId ) - 3) FreeSlots
FROM [dbo].[SG2_Proxy] P

	--- All unavilable IPs
	DELETE FROM @tblAvailableProxyIPs WHERE FreeSlots = 0

DELETE IProxy FROM
@tblAvailableProxyIPs IProxy inner join [dbo].[SG2_SocialProfile_BadProxy] BP 
ON IProxy.ProxyId=BP.ProxyId AND BP.[SocialProfileId]=@riSocialProfileId

	-- Calculate Min Distance 
	SELECT @MinDistance = MIN(distance) FROM @tblAvailableProxyIPs
	
	If not exists(SELECT 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping] where [SocialProfileId]=@riSocialProfileId )
		Begin
			-- Insert Customer IP Mapping 
			INSERT INTO [dbo].[SG2_SocialProfile_ProxyMapping]([ProxyId],[SocialProfileId])
			SELECT ProxyId ,@riSocialProfileId
				FROM @tblAvailableProxyIPs
			WHERE Distance <= @MinDistance

			SET @riProxyMappingId = SCOPE_IDENTITY() 
		END
	ELSE 
		BEGIN
			SELECT @riProxyMappingId=ProxyMappingId 
				FROM [dbo].[SG2_SocialProfile_ProxyMapping] 
			WHERE 
				SocialProfileId = @riSocialProfileId
		END
	
	SELECT PM.*, PRX.[ProxyIPNumber], PRX.[ProxyPort], PRX.[ProxyIPName] 
		FROM 
			[dbo].[SG2_SocialProfile_ProxyMapping] PM
		INNER JOIN 
			SG2_Proxy as PRX
		ON PRX.[ProxyId] = PM.[ProxyId]
	WHERE 
		ProxyMappingId = @riProxyMappingId

End
GO
PRINT N'Creating [dbo].[SG2_usp_Customer_GetPendingSocialProfiles]'
GO

CREATE Procedure [dbo].[SG2_usp_Customer_GetPendingSocialProfiles]
@riStatusId int

As
Begin	
	Select
		[TargetingInformationId]
	   ,[Preference1]
       ,[Preference2]
       ,[Preference3]
       ,[Preference4]
       ,[Preference5]
       ,[Preference6]
       ,[Preference7]
       ,[Preference8]
       ,[Preference9]
	   ,[Preference10]
	   ,SP.SocialProfileId
	   ,SP.SocialProfileName
	   ,SP.SocialUsername
	   ,JV.ExchangeName
	   ,JV.JVBoxId as JVServerId
	   
	FROM  [dbo].[SG2_SocialProfile_TargetingInformation] CTI
		INNER JOIN [dbo].[SG2_SocialProfile] SP 
			ON CTI.[SocialProfileId]=SP.[SocialProfileId]
		INNER JOIN [dbo].[SG2_Customer] C 
			ON C.[CustomerId]=SP.[CustomerId]
			Left Join [dbo].[SG2_JVBox] JV ON JV.JVBoxId=SP.JVBoxId
			Where CTI.QueueStatus=@riStatusId


		
End

GO
PRINT N'Creating [dbo].[SG2_usp_Customer_SavePreference]'
GO


CREATE Procedure [dbo].[SG2_usp_Customer_SavePreference]
	@iSocialProfileId   int=null,
    @rvcPreference1		nvarchar(255) ,
	@rvcPreference2		nvarchar(255) ,
	@rvcPreference3		nvarchar(255) ,
	@rvcPreference4		nvarchar(255) ,
	@iPreference5		int,
	@iPreference6		int,
	@iPreference7		int,
	@rvcPreference8		nvarchar(255) ,
	@rvcPreference9		nvarchar(255) ,
	@rvcPreference10	Int,
	@rvcInstaUser		nvarchar(255) ,
	@rvcInstaPassword	nvarchar(255),
	@rvcCity            int= null,
	@rvcSocialProfileName nvarchar(255),
	@iCustomerId        int,
	@riStatusQueueId smallint= null,
	@riAITargeting   int=2,
	@riSocialAccAs int=2
As  
 
 
Begin

If(@iSocialProfileId IS NULL)
Begin
	Insert into [dbo].[SG2_SocialProfile]
	(
		[CustomerId],
		[SocialProfileTypeId],
		[StatusId],
		[SocialProfileName],
		[CreatedOn],
		[CreatedBy],
		[UpdatedOn],
		[UpdatedBy]
	)
	SELECT 
		@iCustomerId,
		null,
		19,
		@rvcSocialProfileName,
		getdate(),
		(Select Concat(C.Firstname, c.Surname) From SG2_Customer C Where C.CustomerId = @iCustomerid),
		getdate(),
		(Select Concat(C.Firstname, c.Surname) From SG2_Customer C Where C.CustomerId = @iCustomerid)

	SELECT @iSocialProfileId = @@IDENTITY
End

If not exists(Select 1 From [dbo].[SG2_SocialProfile_TargetingInformation] where [SocialProfileId]=@iSocialProfileId  ) 
	Begin
		INSERT INTO [dbo].[SG2_SocialProfile_TargetingInformation]
           ([SocialProfileId]
           ,[Preference1]
           ,[Preference2]
           ,[Preference3]
           ,[Preference4]
           ,[Preference5]
           ,[Preference6]
           ,[Preference7]
           ,[Preference8]
           ,[Preference9]
		   ,CreatedOn,
			UpdatedOn,
			[QueueStatus],
			Preference10,
			SocialAccAs
		   )
		VALUES
		 (
			@iSocialProfileId,
			@rvcPreference1,
			@rvcPreference2,
			@rvcPreference3,	
			@rvcPreference4,
			@iPreference5,
			@iPreference6,
			@iPreference7,
			@rvcPreference8,	
			@rvcPreference9,
			getdate(),
			GETDATE(),
			@riStatusQueueId,
			@riAITargeting,
			@riSocialAccAs
		)
	END
	Else
		Begin
			UPDATE [dbo].[SG2_SocialProfile_TargetingInformation]
			   SET [Preference1] =  @rvcPreference1,
				 [Preference2] =   @rvcPreference2,
				 [Preference3] =   @rvcPreference3,
				 [Preference4] =   @rvcPreference4,
				 [Preference5] =   @iPreference5,
				 [Preference6] =   @iPreference6,
				 [Preference7] =   @iPreference7,
				 [Preference8] =   @rvcPreference8,
				 [Preference9] =   @rvcPreference9,
				 Preference10  =   @riAITargeting,
				 [QueueStatus] =   @riStatusQueueId,
				 UpdatedOn=GETDATE(),
				 SocialAccAs=@riSocialAccAs
			 WHERE [SocialProfileId]=@iSocialProfileId
	END
if @rvcSocialProfileName is not null
Begin
	Update 
		[dbo].[SG2_SocialProfile]
		SET 
			[SocialProfileName] = @rvcSocialProfileName,
			--[SocialUsername]=@rvcInstaUser, 
		--	[SocialPassword]=@rvcInstaPassword,
		--	[SocialPrefferedCity]=@rvcCity,
			--[SocialPrefferedCountry]=@rvcPreference10, 
			UpdatedOn=GETDATE()
		Where [SocialProfileId]=@iSocialProfileId
END
		SELECT [SocialProfileId],
			   [Preference1]
			  ,[Preference2]
			  ,[Preference3]
			  ,[Preference4]
			  ,[Preference5]
			  ,[Preference6]
			  ,[Preference7]
			  ,[Preference8]
			  ,[Preference9]
			  ,[Preference10]
		  FROM [dbo].[SG2_SocialProfile_TargetingInformation] 
		  Where[SocialProfileId]=@iSocialProfileId
         
End

GO
PRINT N'Creating [dbo].[SG2_usp_Customer_ScheduleCall]'
GO


CREATE Procedure [dbo].[SG2_usp_Customer_ScheduleCall]
@riCustomerId int,
@rdtScheduleDate datetime,
@rvcTest Nvarchar(max)

 
As  
Begin

Update [dbo].[SG2_Customer_ContactDetail]
		Set [ScheduleCallDate]=@rdtScheduleDate,
     [Notes]=@rvcTest
	 Where [CustomerId]=@riCustomerId

Select 1

End

GO
PRINT N'Creating [dbo].[SG2_usp_Get_Product]'
GO

CREATE Procedure [dbo].[SG2_usp_Get_Product]
 
As  
Begin

SELECT PlanName as [Name],StripePlanId as StripeSubscriptionId
FROM [dbo].[SG2_SocialProfile_PaymentPlan] 

End

GO
PRINT N'Creating [dbo].[SG2_usp_Get_Title]'
GO

CREATE Procedure [dbo].[SG2_usp_Get_Title]
 
As  
Begin

SELECT Distinct [PkTitleId],[TitleName]   FROM  [dbo].[SG2_Customer_Title]
  
End

GO
PRINT N'Creating [dbo].[SG2_usp_LikeyAccount_Delete]'
GO

CREATE Procedure [dbo].[SG2_usp_LikeyAccount_Delete]
  @riLikeyAccountId Int
As  
Begin

   UPDATE SG2_LikeyAccount 
   SET StatusID = 18
   WHERE LikeyAccountId = @riLikeyAccountId
 
 Return 1;
    
End
GO
PRINT N'Creating [dbo].[SG2_usp_LikeyAccount_GetById]'
GO
CREATE Procedure [dbo].[SG2_usp_LikeyAccount_GetById]
  @riLikeyAccountId Int

As  
Begin

SELECT LikeyAccountId, 
	  InstaUserName, 
	  InstaPassword, 
	  Country, 
	  City, 
	  Gender, 
	  HashTag,
	  [StatusId]
FROM [dbo].SG2_LikeyAccount
Where LikeyAccountId = @riLikeyAccountId

End
GO
PRINT N'Creating [dbo].[SG2_usp_LikeyAccount_Save]'
GO
CREATE PROCEDURE [dbo].[SG2_usp_LikeyAccount_Save]
(
	@riLikeyAccountId			INT,
	@rvcInstaUserName			NVARCHAR(15),
	@rvcInstaPassword			NVARCHAR(50),
	@rvcCountry					NVARCHAR(50),
	@rvcCity					NVARCHAR(50),
	@rvcGender					smallint,
	@rvcHashTag					NVARCHAR(50),
	@riStatusId					INT = 3
)
 
AS  
 
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].SG2_LikeyAccount WHERE LikeyAccountId = @riLikeyAccountId ) 
		BEGIN
			INSERT INTO [dbo].SG2_LikeyAccount
					   (
		   				 InstaUserName,
						 InstaPassword,
						 Country,
						 City,
						 Gender,
						 HashTag,
						 StatusId
						)
					 VALUES
					 (
						 @rvcInstaUserName,
						 @rvcInstaPassword,
						 @rvcCountry,
						 @rvcCity,
						 @rvcGender,
						 @rvcHashTag,
						 @riStatusId
						 )
					 Select @riLikeyAccountId = SCOPE_IDENTITY() 
			END
	ELSE
		BEGIN
			UPDATE [dbo].SG2_LikeyAccount
			   SET
						InstaUserName = @rvcInstaUserName,
						InstaPassword = @rvcInstaPassword,
						Country = @rvcCountry,
						City = @rvcCity,
						Gender = @rvcGender,
						HashTag = @rvcHashTag,
						StatusId = @riStatusId
			 WHERE  LikeyAccountId= @riLikeyAccountId
		End
END
GO
PRINT N'Creating [dbo].[SG2_usp_PlanInformation_Delete]'
GO

CREATE Procedure [dbo].[SG2_usp_PlanInformation_Delete]
  @riPlanInformationId Int
As  
Begin
DECLARE @StripPlanId int
SELECT @StripPlanId=[StripePlanId]  FROM [dbo].[SG2_SocialProfile_PaymentPlan] where PlanId=@riPlanInformationId
if not exists(SELECT 1 from [dbo].[SG2_SocialProfile_Payments] where [StripeSubscriptionId]=@StripPlanId)
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
GO
PRINT N'Creating [dbo].[SG2_usp_PlanInformation_GetById]'
GO
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
GO
PRINT N'Creating [dbo].[SG2_usp_PlanInformation_Save]'
GO
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
GO
PRINT N'Creating [dbo].[SG2_usp_Report_GetReportData]'
GO

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
			From [dbo].[SG2_SocialProfile_Payments] S 
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

GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_BadProxy]'
GO
CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_BadProxy]
(
	@riProxyId					INT,
	@riSocailProfileID	        INT = null,
	@riStatusId					INT = 49
)
 
AS  
 
BEGIN

Declare @riBadProxyMappingId int
Declare @iMaxRetry int

INSERT INTO [dbo].[SG2_SocialProfile_BadProxy]
           (
		    [ProxyId],
		    [SocialProfileId],
		    [StatusId],
			[CityId]
			)
     VALUES
	 (     @riProxyId,
		   @riSocailProfileID,
		   @riStatusId	,
		   (SELECT [SocialPrefferedCity] FROM [dbo].[SG2_SocialProfile]	where [SocialProfileId]=  @riSocailProfileID )
	  )

		 Select @riBadProxyMappingId = SCOPE_IDENTITY() 
 
 Delete from [dbo].[SG2_SocialProfile_ProxyMapping] where [ProxyId]=@riProxyId
 and [SocialProfileId]=@riSocailProfileID

 SELECT @iMaxRetry=Count(*) from [dbo].[SG2_SocialProfile_BadProxy] where [ProxyId]=@riProxyId
 AND SocialProfileId=@riSocailProfileID

 Update [dbo].[SG2_Proxy] set [NoOfMaxRetry] = @iMaxRetry where [ProxyId]=@riProxyId

 If exists(Select 1 from [dbo].[SG2_Proxy] where [ProxyId]=@riProxyId and [NoOfMaxRetry]=3  )
 BEGIN 
 Update [dbo].[SG2_Proxy] set [StatusId]=@riStatusId where [ProxyId]=@riProxyId
 END
  
END

SELECT      ProxyId,
		    [SocialProfileId]
			FROM [dbo].[SG2_SocialProfile_BadProxy]  
			WHERE BadProxyMappingId= @riBadProxyMappingId
					
         
GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers]'
GO

CREATE Procedure [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers]
  @riSocialProfileId Int,
  @dtFromDate Date,
  @dtToDate   Date

As
Begin	

	SELECT  
	 [SocialProfileId],[Username], FollowersGain FollowersGain, followers followers, 
	 Followings Followings, FollowingsRatio FollowingsRatio,
	 followers/24 AVGFollowers,
	 [Like],Comment,Engagement,LikeComments,
	 [Date]
	FROM [dbo].[SG2_SocialProfile_Statistics]
	WHERE [SocialProfileId] = @riSocialProfileId
	 AND CAST([Date] as DATE) BETWEEN @dtFromDate and @dtToDate

	-- GROUP BY [SocialProfileId], [Username],[Date]
End

GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers_Old]'
GO

CREATE Procedure [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers_Old]
  @riSocialProfileId Int,
  @dtFromDate Date,
  @dtToDate   Date,
  @nvChartType Nvarchar(255)

As
Begin	
   DEclare @iTodayDays int

SELECT @iTodayDays = DATEDIFF(day, @dtFromDate, @dtToDate);
	SELECT  
	 [SocialProfileId],[Username], SUM(FollowersGain) FollowersGain, SUM(followers) followers, SUM(Followings) Followings, SUM(FollowingsRatio) FollowingsRatio,
	 (SUM(followers)/1) AVGFollowers,
	 DATENAME(weekday,[Date]) WeekDays,
	 DATENAME(month,[Date])  Monthly
	FROM [dbo].[SG2_SocialProfile_Statistics]
	WHERE [SocialProfileId] = @riSocialProfileId
	 AND [Date] BETWEEN @dtFromDate and @dtToDate

	 GROUP BY [SocialProfileId], [Username],[Date]
End

GO
PRINT N'Creating [dbo].[SG2_usp_SocialProfile_Statistics_GetStatistics]'
GO

CREATE Procedure [dbo].[SG2_usp_SocialProfile_Statistics_GetStatistics]
  @riSocialProfileId Int

As
Begin	

   DEclare @iTodayDays BIGINT,
           @iTotalFollowers BIGINT,
		   @iTotalFollowersGain BIGINT,
		   @iTotalFollowings BIGINT,
		   @iTotalFollowingsRatio BIGINT,
		   @iTotalLike BIGINT,
		   @iTotalLikeComment BIGINT,
		   @iTotalComment BIGINT,
		    @iTotalEngagement BIGINT

	SELECT  TOP 1
	   @iTotalFollowers = followers,
	   @iTotalFollowersGain = FollowersGain, 
	   @iTotalFollowings = Followings, 
	   @iTotalFollowingsRatio = FollowingsRatio,
		@iTotalLike  = [Like],
		@iTotalLikeComment  = LikeComments,
		@iTotalComment  = Comment,
		@iTotalEngagement  = Engagement
	FROM [dbo].[SG2_SocialProfile_Statistics]
	WHERE [SocialProfileId] = @riSocialProfileId
	ORDER BY DATE DESC

	SELECT  
	 @iTotalFollowers TotalFollowers,
	 @iTotalFollowersGain TotalFollowersGain,
	 @iTotalFollowings TotalFollowings,
	 @iTotalFollowingsRatio TotalFollowingsRatio,
	 @iTotalLike TotalLike ,
	@iTotalLikeComment TotalLikeComment,
	@iTotalComment   TotalComment,
	@iTotalEngagement   TotalEngagement

	-- GROUP BY [SocialProfileId], [Username],[Date]
End

GO
PRINT N'Creating [dbo].[SG2_usp_SystemConfig_GetById]'
GO

CREATE Procedure [dbo].[SG2_usp_SystemConfig_GetById]
  @riConfigId smallint

As  
Begin

   SELECT    [ConfigId],
			 [ConfigKey], 
			 [ConfigValue], 
			 [ConfigValue2], 
			 [CreatedOn], 
			 [CreatedBy], 
			 [ModifiedOn], 
			 [ModifiedBy]
  FROM [dbo].[SG2_SystemConfig]
  WHERE [ConfigId] = @riConfigId

    
End
GO
PRINT N'Creating [dbo].[SG2_usp_SystemConfig_Save]'
GO
CREATE PROCEDURE [dbo].[SG2_usp_SystemConfig_Save]
(
	@riConfigId			smallint,
	@rvcConfigValue		nvarchar(250),
	@rvcConfigValue2	nvarchar(250),
	@rdtModifiedOn		datetime,
	@rvcModifiedBy		nvarchar(50)
)
 
AS  
 
 
BEGIN
	
		UPDATE  [dbo].[SG2_SystemConfig]
		   SET
					[ConfigValue] = @rvcConfigValue,
					[ConfigValue2] = @rvcConfigValue2,
					[ModifiedOn] = @rdtModifiedOn,
					[ModifiedBy] = @rvcModifiedBy
		 WHERE ConfigId = @riConfigId
					    
End
GO
PRINT N'Creating [dbo].[SG2_usp_SystemRole_AllRole]'
GO

CREATE Procedure [dbo].[SG2_usp_SystemRole_AllRole]

As  
Begin


  Select [RoleId],[Name] From [dbo].[SG2_SystemRole] Where StatusId = 19 and RoleId  <> 1 -- Super Admin

 Return @@Error 
End
GO
PRINT N'Creating [dbo].[SG2_usp_SystemUser_GetById]'
GO

CREATE Procedure [dbo].[SG2_usp_SystemUser_GetById]
  @riSystemUserId INT

As  
Begin

   SELECT   [SystemUserId], 
			Title,
			[FirstName], 
			[LastName], 
			[Email], 
			[SystemRoleId], 
			[Password], 
			[StatusId], 
			[CreatedOn], 
			[CreatedBy], 
			[ModifiedOn], 
			[ModifiedBy], 
			[HostUser] 
  FROM [dbo].SG2_SystemUser
  WHERE [SystemUserId] = @riSystemUserId

    
End
GO
PRINT N'Creating [dbo].[SG2_usp_SystemUser_Save]'
GO
CREATE PROCEDURE [dbo].[SG2_usp_SystemUser_Save]
(
	@riSystemUserId		int,
	@rvcTitle			nvarchar(5),
	@rvcFirstName		nvarchar(50),
	@rvcLastName		nvarchar(50),
	@rvcEmail			nvarchar(50),
	@riSystemRoleId		Int,
	@rvcPassword		nvarchar(50),
	@riStatusId			Smallint,
	@rdtCreatedOn		datetime,
	@rvcCreatedBy		nvarchar(50),
	@rdtModifiedOn		datetime,
	@rvcModifiedBy		nvarchar(50)
)
 
AS  
 
 
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].SG2_SystemUser WHERE SystemUserId = @riSystemUserId ) 
		BEGIN
		INSERT INTO [dbo].SG2_SystemUser
				   (
						Title,
						[FirstName], 
						[LastName],
						[Email], 
						[SystemRoleId], 
						[Password], 
						[StatusId], 
						[CreatedOn], 
						[CreatedBy], 
						[ModifiedOn], 
						[ModifiedBy],
						HostUser
						
					)
					VALUES
					(
						@rvcTitle,
						@rvcFirstName,
						@rvcLastName,
						@rvcEmail,
						@riSystemRoleId,
						@rvcPassword,
						@riStatusId,
						@rdtCreatedOn,
						@rvcCreatedBy,
						@rdtModifiedOn,
						@rvcModifiedBy,
						0
					)
		END
	ELSE
		BEGIN
		UPDATE  [dbo].SG2_SystemUser
		   SET
					Title = @rvcTitle,
					FirstName	= @rvcFirstName,
					LastName	= @rvcLastName,
					Email		= @rvcEmail,
					SystemRoleId= @riSystemRoleId,
					StatusId	= @riStatusId,
					ModifiedOn	= @rdtModifiedOn,
					ModifiedBy	= @rvcModifiedBy
		 WHERE SystemUserId = @riSystemUserId
					
	END    
End
GO
PRINT N'Adding foreign keys to [dbo].[Customer_ContactDetail]'
GO
ALTER TABLE [dbo].[Customer_ContactDetail] ADD CONSTRAINT [FK_SG2_ContactDetails_SG2_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
GO
PRINT N'Adding foreign keys to [dbo].[SocialProfile]'
GO
ALTER TABLE [dbo].[SocialProfile] ADD CONSTRAINT [FK_SG2_SocialProfile_SG2_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
GO
PRINT N'Adding foreign keys to [dbo].[SocialProfile_Payments]'
GO
ALTER TABLE [dbo].[SocialProfile_Payments] ADD CONSTRAINT [FK_SocialProfile_Payments_SocialProfile_PaymentPlan] FOREIGN KEY ([PaymentPlanId]) REFERENCES [dbo].[PaymentPlan] ([PaymentPlanId])
GO
ALTER TABLE [dbo].[SocialProfile_Payments] ADD CONSTRAINT [FK_SG2_SocialProfile_Payments_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId])
GO
PRINT N'Adding foreign keys to [dbo].[SocialProfile_Statistics]'
GO
ALTER TABLE [dbo].[SocialProfile_Statistics] ADD CONSTRAINT [FK_SG2_SocialProfile_Statistics_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId])
GO
PRINT N'Adding foreign keys to [dbo].[SocialProfile_Actions]'
GO
ALTER TABLE [dbo].[SocialProfile_Actions] ADD CONSTRAINT [FK_SG2_SocialProfile_StatusHistory_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId])
GO
PRINT N'Adding foreign keys to [dbo].[SocialProfile_Instagram_TargetingInformation]'
GO
ALTER TABLE [dbo].[SocialProfile_Instagram_TargetingInformation] ADD CONSTRAINT [FK_SG2_SocialProfile_TargetingInformation_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId])
GO
PRINT N'Adding foreign keys to [dbo].[SystemUser]'
GO
ALTER TABLE [dbo].[SystemUser] ADD CONSTRAINT [FK_SG2_SystemUsers_SG2_SystemRoles] FOREIGN KEY ([SystemRoleId]) REFERENCES [dbo].[SystemRole] ([RoleId])
GO
