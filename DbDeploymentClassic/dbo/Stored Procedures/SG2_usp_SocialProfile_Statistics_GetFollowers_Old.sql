
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

