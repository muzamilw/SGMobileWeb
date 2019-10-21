  
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
  Left join [dbo].[SocialProfile_Subscription] Subc  
   on Subc.socialprofileid = SP.socialprofileid and subc.statusid = 25  
  Left Join [dbo].[PaymentPlan] PP  
   on Subc.paymentplanid = pp.paymentplanid  
  Left join EnumerationValue EV3  
   ON EV3.EnumerationValueId = subc.statusid  

 
 where CTI.SocialProfileId = @riSocialProfileId  
  
End  
  



	


