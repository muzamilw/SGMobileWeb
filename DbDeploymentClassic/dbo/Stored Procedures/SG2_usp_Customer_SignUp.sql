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


