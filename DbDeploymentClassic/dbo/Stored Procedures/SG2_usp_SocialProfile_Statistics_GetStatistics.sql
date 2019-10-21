
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

