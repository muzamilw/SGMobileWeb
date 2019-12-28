

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

