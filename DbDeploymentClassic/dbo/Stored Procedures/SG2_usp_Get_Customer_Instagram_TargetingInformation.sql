  
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
  