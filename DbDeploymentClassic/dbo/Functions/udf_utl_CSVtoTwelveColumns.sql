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
