
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

