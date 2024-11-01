/****** Object:  Database [DB_129180_socialgrowth]    Script Date: 10/5/2019 1:50:30 PM ******/
CREATE DATABASE [DB_129180_socialgrowth]  (EDITION = 'Standard', SERVICE_OBJECTIVE = 'S2', MAXSIZE = 250 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS;
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ARITHABORT OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET  MULTI_USER 
GO
ALTER DATABASE [DB_129180_socialgrowth] SET ENCRYPTION ON
GO
ALTER DATABASE [DB_129180_socialgrowth] SET QUERY_STORE = ON
GO
ALTER DATABASE [DB_129180_socialgrowth] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
/****** Object:  UserDefinedFunction [dbo].[StringSplit]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  UserDefinedFunction [dbo].[udf_utl_CSVtoTwelveColumns]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[SG2_Customer]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[GUID] [nvarchar](36) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[SurName] [nvarchar](20) NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](64) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LoginAttempts] [tinyint] NOT NULL,
	[LastLoginIP] [nvarchar](20) NOT NULL,
	[Tocken] [nvarchar](128) NOT NULL,
	[StripeCustomerId] [nvarchar](max) NULL,
	[UserName] [nvarchar](50) NULL,
	[Source] [nvarchar](50) NULL,
	[Register] [nvarchar](50) NULL,
	[ResponsibleTeamMemberId] [int] NULL,
	[AvailableToEveryOne] [bit] NULL,
	[Comment] [nvarchar](max) NULL,
	[CancelledDate] [datetime] NULL,
	[IsOptedEducationalEmailSeries] [bit] NULL,
	[IsOptedMarketingEmail] [bit] NULL,
	[Title] [nvarchar](10) NULL,
 CONSTRAINT [PK_SG2_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_Customer_ContactDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_Customer_ContactDetail](
	[ContactDetailsId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[JobTitle] [nvarchar](50) NULL,
	[MobileNumber] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[AddressLine1] [nvarchar](255) NULL,
	[AddressLine2] [nvarchar](255) NULL,
	[City] [nvarchar](50) NULL,
	[Sate] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[PostalCode] [nvarchar](50) NULL,
	[GUID] [nvarchar](36) NULL,
	[PhoneCode] [nvarchar](5) NULL,
	[ScheduleCallDate] [datetime] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_SG2_ContactDetails] PRIMARY KEY CLUSTERED 
(
	[ContactDetailsId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_Customer_Title]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_Customer_Title](
	[PkTitleId] [int] IDENTITY(1,1) NOT NULL,
	[TitleName] [varchar](50) NULL,
 CONSTRAINT [PK_SG2_Title] PRIMARY KEY CLUSTERED 
(
	[PkTitleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_Enumeration]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_Enumeration](
	[EnumerationId] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_SG2_Enumeration] PRIMARY KEY CLUSTERED 
(
	[EnumerationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_EnumerationValue]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_EnumerationValue](
	[EnumerationValueId] [smallint] IDENTITY(1,1) NOT NULL,
	[EnumerationId] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[SequenceNo] [int] NOT NULL,
	[IsVisible] [bit] NULL,
 CONSTRAINT [PK_SG2_EnumerationValue] PRIMARY KEY CLUSTERED 
(
	[EnumerationValueId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_JVBox]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_JVBox](
	[JVBoxId] [int] IDENTITY(1,1) NOT NULL,
	[BoxName] [nvarchar](100) NOT NULL,
	[AdminName] [nvarchar](50) NOT NULL,
	[AdminPassword] [nvarchar](50) NOT NULL,
	[BoxManagedBy] [nvarchar](50) NOT NULL,
	[SupportPhone] [nvarchar](50) NOT NULL,
	[SupportEmail] [nvarchar](50) NOT NULL,
	[HostedBy] [nvarchar](50) NOT NULL,
	[HostingPhone] [nvarchar](50) NOT NULL,
	[HostingWebsite] [nvarchar](50) NOT NULL,
	[HostingAccount] [nvarchar](50) NOT NULL,
	[HostingPassword] [nvarchar](50) NOT NULL,
	[HostingPriceInfo] [nvarchar](max) NULL,
	[StatusId] [int] NOT NULL,
	[MaxLimit] [int] NULL,
	[JVBoxType] [int] NULL,
	[ExchangeName] [nvarchar](250) NULL,
	[QueueStatusId] [smallint] NULL,
	[PRCExecProfileId] [nvarchar](50) NULL,
	[ServerRunningStatusId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NULL,
	[Updateby] [nvarchar](50) NULL,
 CONSTRAINT [PK_SG2_JVBoxes] PRIMARY KEY CLUSTERED 
(
	[JVBoxId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_LikeyAccount]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_LikeyAccount](
	[LikeyAccountId] [int] IDENTITY(1,1) NOT NULL,
	[InstaUserName] [nvarchar](50) NOT NULL,
	[InstaPassword] [nvarchar](50) NULL,
	[Country] [nvarchar](50) NULL,
	[City] [nvarchar](50) NOT NULL,
	[Gender] [int] NOT NULL,
	[HashTag] [nvarchar](50) NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_SG2_LikeyAccount] PRIMARY KEY CLUSTERED 
(
	[LikeyAccountId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_Proxy]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_Proxy](
	[ProxyId] [int] IDENTITY(1,1) NOT NULL,
	[ProxyIPNumber] [nvarchar](15) NOT NULL,
	[ProxyIPName] [nvarchar](50) NOT NULL,
	[BaseCity] [nvarchar](50) NULL,
	[BaseCountry] [nvarchar](50) NULL,
	[GeoPoints] [nvarchar](150) NULL,
	[StatusId] [int] NOT NULL,
	[VPSSId] [int] NULL,
	[ProxyPort] [nvarchar](50) NULL,
	[NoOfMaxRetry] [int] NULL,
 CONSTRAINT [PK_SG2_ProxyMapping] PRIMARY KEY CLUSTERED 
(
	[ProxyId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_QueueAudit]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_QueueAudit](
	[QueueAuditId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [nvarchar](32) NOT NULL,
	[QueueType] [smallint] NULL,
	[QueueStatus] [smallint] NULL,
	[QueueData] [nvarchar](max) NULL,
	[ErrorDescription] [nvarchar](max) NULL,
	[ProfileId] [int] NULL,
	[JVBoxData] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NULL,
	[NoOfAttempts] [int] NULL,
	[QueueAction] [nvarchar](50) NULL,
	[JVServerId] [int] NULL,
 CONSTRAINT [PK_SG2_QueueAudit] PRIMARY KEY CLUSTERED 
(
	[QueueAuditId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_QueueAuditDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_QueueAuditDetail](
	[QueueAuditDetailId] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [nvarchar](32) NULL,
	[StepName] [nvarchar](50) NULL,
	[StepDetail] [nvarchar](max) NULL,
	[StepStatus] [int] NULL,
	[StepError] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [varchar](50) NULL,
	[Base64Image] [varbinary](max) NULL,
 CONSTRAINT [PK_SG2_QueueAuditDetail] PRIMARY KEY CLUSTERED 
(
	[QueueAuditDetailId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_QueueAuditEnumeration]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_QueueAuditEnumeration](
	[EnumerationId] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_SG2_QueueAuditEnumeration] PRIMARY KEY CLUSTERED 
(
	[EnumerationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_QueueAuditEnumerationValue]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_QueueAuditEnumerationValue](
	[EnumerationValueId] [smallint] IDENTITY(1,1) NOT NULL,
	[EnumerationId] [smallint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[SequenceNo] [int] NOT NULL,
	[IsVisible] [bit] NULL,
 CONSTRAINT [PK_SG2_QueueAuditEnumerationValue] PRIMARY KEY CLUSTERED 
(
	[EnumerationValueId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile](
	[SocialProfileId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[SocialProfileTypeId] [int] NULL,
	[JVBoxId] [int] NULL,
	[JVBoxStatusId] [int] NULL,
	[StatusId] [int] NULL,
	[StripeCustomerId] [int] NULL,
	[SocialUsername] [nvarchar](50) NULL,
	[SocialPassword] [nvarchar](50) NULL,
	[SocialProfileName] [nvarchar](50) NULL,
	[SocialPrefferedCity] [int] NULL,
	[SocialPrefferedCountry] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[JVAttempts] [smallint] NULL,
	[JVAttemptsBlockedTill] [datetime] NULL,
	[JVAttemptStatus] [smallint] NULL,
	[verificationCode] [nvarchar](50) NULL,
	[IsArchived] [bit] NULL,
 CONSTRAINT [PK_SG2_CustomerProfile] PRIMARY KEY CLUSTERED 
(
	[SocialProfileId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_BadProxy]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_BadProxy](
	[BadProxyMappingId] [int] IDENTITY(1,1) NOT NULL,
	[ProxyId] [int] NULL,
	[SocialProfileId] [int] NULL,
	[StatusId] [int] NULL,
	[CityId] [int] NULL,
 CONSTRAINT [PK_SG2_SocialProfile_BadProxy] PRIMARY KEY CLUSTERED 
(
	[BadProxyMappingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_Notification]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_Notification](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Notification] [nvarchar](250) NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[SocialProfileId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[UpdateOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NOT NULL,
	[Mode] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_SG2_SocialProfile_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_PaymentPlan]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_PaymentPlan](
	[PlanId] [int] IDENTITY(1,1) NOT NULL,
	[NoOfLikes] [int] NULL,
	[DisplayPrice] [float] NULL,
	[PlanName] [nvarchar](250) NOT NULL,
	[PlanShortDescription] [nvarchar](250) NOT NULL,
	[PlanTypeId] [int] NULL,
	[StripePlanId] [nvarchar](50) NULL,
	[StripePlanPrice] [float] NULL,
	[NoOfLikesDuration] [int] NULL,
	[StatusId] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[SortOrder] [smallint] NULL,
	[SocialPlanTypeId] [int] NULL,
	[IsDefault] [bit] NULL,
 CONSTRAINT [PK_PlanInformation] PRIMARY KEY CLUSTERED 
(
	[PlanId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_ProxyMapping]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_ProxyMapping](
	[ProxyMappingId] [int] IDENTITY(1,1) NOT NULL,
	[ProxyId] [int] NOT NULL,
	[SocialProfileId] [int] NOT NULL,
 CONSTRAINT [PK_SG2_Customer_ProxyMapping] PRIMARY KEY CLUSTERED 
(
	[ProxyMappingId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_Statistics]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_Statistics](
	[SocialStatisticsId] [bigint] IDENTITY(1,1) NOT NULL,
	[SocialProfileId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Date] [datetime] NOT NULL,
	[FollowersGain] [bigint] NULL,
	[Followers] [bigint] NULL,
	[Followings] [bigint] NULL,
	[FollowingsRatio] [decimal](18, 2) NULL,
	[Joiner] [bigint] NULL,
	[Unjoiner] [bigint] NULL,
	[Follow] [bigint] NULL,
	[Unfollow] [bigint] NULL,
	[ContactMassage] [bigint] NULL,
	[ContactFriends] [bigint] NULL,
	[REPinTweetBlog] [bigint] NULL,
	[Bump] [bigint] NULL,
	[Like] [bigint] NULL,
	[Comment] [bigint] NULL,
	[Engagement] [bigint] NULL,
	[Repost] [bigint] NULL,
	[LikeComments] [bigint] NULL,
	[StoryViewer] [bigint] NULL,
	[BlockedFollowers] [bigint] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NULL,
 CONSTRAINT [PK_CustomerStatistics] PRIMARY KEY CLUSTERED 
(
	[SocialStatisticsId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_StatusHistory]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_StatusHistory](
	[SPSHId] [int] IDENTITY(1,1) NOT NULL,
	[SocialProfileId] [int] NULL,
	[JVBoxId] [int] NULL,
	[JVBoxStatusId] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SG2_SocialProfile_StatusHistory] PRIMARY KEY CLUSTERED 
(
	[SPSHId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_Subscription]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_Subscription](
	[SubscriptionId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[SubscriptionType] [nvarchar](255) NULL,
	[Price] [decimal](18, 2) NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[SocialProfileId] [int] NOT NULL,
	[StripeSubscriptionId] [nvarchar](255) NULL,
	[StatusId] [int] NULL,
	[StripePlanId] [nvarchar](255) NULL,
	[PaymentPlanId] [int] NULL,
	[StripeInvoiceId] [nvarchar](255) NULL,
 CONSTRAINT [PK_SG2_Subscription] PRIMARY KEY CLUSTERED 
(
	[SubscriptionId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SocialProfile_TargetingInformation]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SocialProfile_TargetingInformation](
	[TargetingInformationId] [int] IDENTITY(1,1) NOT NULL,
	[SocialProfileId] [int] NOT NULL,
	[Preference1] [nvarchar](255) NULL,
	[Preference2] [nvarchar](255) NULL,
	[Preference3] [nvarchar](255) NULL,
	[Preference4] [nvarchar](255) NULL,
	[Preference5] [int] NULL,
	[Preference6] [int] NULL,
	[Preference7] [int] NULL,
	[Preference8] [nvarchar](255) NULL,
	[Preference9] [nvarchar](255) NULL,
	[Preference10] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[UpdatedBy] [nvarchar](50) NULL,
	[QueueStatus] [smallint] NULL,
	[JVNoOfLikes] [int] NULL,
	[JVLikeyStatus] [smallint] NULL,
	[SocialAccAs] [int] NULL,
 CONSTRAINT [PK_SG2_TargetingInformation] PRIMARY KEY CLUSTERED 
(
	[TargetingInformationId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemCity]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemCity](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[CountryId] [smallint] NOT NULL,
	[StateId] [smallint] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[StatusId] [smallint] NULL,
 CONSTRAINT [PK_SG2_SystemCity] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemConfig]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemConfig](
	[ConfigId] [smallint] IDENTITY(1,1) NOT NULL,
	[ConfigKey] [nvarchar](50) NOT NULL,
	[ConfigValue] [nvarchar](250) NOT NULL,
	[ConfigValue2] [nvarchar](250) NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SG2_SystemConfig] PRIMARY KEY CLUSTERED 
(
	[ConfigId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemCountry]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemCountry](
	[CountryId] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[PhoneCode] [nvarchar](5) NULL,
	[StatusId] [smallint] NULL,
 CONSTRAINT [PK_SG2_SystemCountry] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemRole]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemRole](
	[RoleId] [smallint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SG2_SystemRoles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemState]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemState](
	[StateId] [smallint] IDENTITY(1,1) NOT NULL,
	[CountryId] [smallint] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Code] [nvarchar](5) NULL,
	[StatusId] [smallint] NULL,
 CONSTRAINT [PK_SG2_SystemStates] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_SystemUser]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_SystemUser](
	[SystemUserId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](5) NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[SystemRoleId] [smallint] NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[StatusId] [smallint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[HostUser] [bit] NOT NULL,
 CONSTRAINT [PK_SG2_SystemUsers] PRIMARY KEY CLUSTERED 
(
	[SystemUserId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SG2_VPSSupplier]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SG2_VPSSupplier](
	[VPSSId] [int] IDENTITY(1,1) NOT NULL,
	[IssuingISPName] [nvarchar](50) NULL,
	[IssuingISPPhone] [nvarchar](50) NULL,
	[IssuingISPWebsite] [nvarchar](50) NULL,
	[IssuingISPAccount] [nvarchar](50) NULL,
	[IssuingISPPassword] [nvarchar](50) NULL,
	[IssuingISPMemo] [nvarchar](50) NULL,
	[StatusId] [int] NOT NULL,
	[IPManageBy] [nvarchar](50) NULL,
	[SupportPhone] [nvarchar](50) NULL,
	[SupportEmail] [nvarchar](50) NULL,
 CONSTRAINT [PK_SG2_VPSSupplier] PRIMARY KEY CLUSTERED 
(
	[VPSSId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SG2_Customer] ON 

INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (1, N'73d7bf5f-2e5f-4e81-8422-f5fe69a88613', N'Hassan', NULL, N'hassanjamil.bwp@gmail.com', N'123', CAST(N'2019-09-18T07:12:03.387' AS DateTime), N'Hassan', CAST(N'2019-09-18T07:12:03.387' AS DateTime), N'Hassan', 5, CAST(N'2019-09-18T07:12:03.387' AS DateTime), 0, N'119.63.135.222', N'0', N'cus_FpcbaQF9vrdtVf', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'2')
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (2, N'55f80537-6200-4dcc-b020-3613bc2b604c', N'Lucas', NULL, N'lucas.eyre@gmail.com', N'test1234', CAST(N'2019-09-18T09:40:32.340' AS DateTime), N'Lucas', CAST(N'2019-09-18T09:40:32.340' AS DateTime), N'Lucas', 5, CAST(N'2019-09-18T09:40:32.340' AS DateTime), 0, N'5.90.237.11', N'0', N'cus_Fpf4jI01deuVSX', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (4, N'5480b71b-cd6c-459d-bf62-8780ff06cdbd', N'Anthony Niehaus', NULL, N'anthony@einfinity.email', N'tm{uHmAKW75,1I1D', CAST(N'2019-09-19T18:24:28.577' AS DateTime), N'Anthony Niehaus', CAST(N'2019-09-19T18:24:28.577' AS DateTime), N'Anthony Niehaus', 1, CAST(N'2019-09-19T18:24:28.577' AS DateTime), 0, N'216.49.115.71', N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (6, N'edd7a747-bf82-4631-9350-3e49594e24a1', N'Muhammad', NULL, N'abdullahbcy@gmail.com', N'Abdullah10', CAST(N'2019-09-23T07:34:21.740' AS DateTime), N'Muhammad', CAST(N'2019-09-23T07:34:21.740' AS DateTime), N'Muhammad', 5, CAST(N'2019-09-23T07:34:21.740' AS DateTime), 0, N'39.50.109.225', N'0', N'cus_FrV73w2Sb7Y9OW', NULL, NULL, NULL, NULL, NULL, N'
OC (23 Sep, 2019 07:39): Invalid.', NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (7, N'7256a139-298b-478b-86d2-50064f844aeb', N'Sam Bayne ', NULL, N'Sam@atlantafinehomes.com', N'Sambayne#1', CAST(N'2019-09-23T21:58:53.000' AS DateTime), N'Sam Bayne ', CAST(N'2019-09-23T21:58:53.000' AS DateTime), N'Sam Bayne ', 5, CAST(N'2019-09-23T21:58:53.000' AS DateTime), 0, N'107.133.224.17', N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (8, N'28ceee23-6fcb-426c-8e02-d5c5f1927e9a', N'Omar', NULL, N'omar.c@me.com', N'123', CAST(N'2019-09-24T06:42:19.293' AS DateTime), N'Omar', CAST(N'2019-09-24T06:42:19.293' AS DateTime), N'Omar', 5, CAST(N'2019-09-24T06:42:19.293' AS DateTime), 0, N'39.50.98.194', N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (9, N'36ec79c4-3816-4a4f-8e28-14da65d67f60', N'Lucas', NULL, N'hello@thebaysocial.com', N'test1234', CAST(N'2019-09-24T09:01:14.683' AS DateTime), N'Lucas', CAST(N'2019-09-24T09:01:14.683' AS DateTime), N'Lucas', 1, CAST(N'2019-09-24T09:01:14.683' AS DateTime), 0, N'5.90.137.88', N'0', N'cus_Frti6dmW4eVtTO', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (12, N'a4f3575f-78db-44d4-ac40-4ca6223e2930', N'waheed', NULL, N'w.sardar000@gmial.com', N'123456789', CAST(N'2019-09-28T07:15:53.077' AS DateTime), N'waheed', CAST(N'2019-09-28T07:15:53.077' AS DateTime), N'waheed', 5, CAST(N'2019-09-28T07:15:53.077' AS DateTime), 0, N'39.50.36.62', N'0', N'cus_FtMvh419sQJuG9', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (15, N'8f9150d5-7393-4760-9b74-8f329c3aa03c', N'kashif133', NULL, N'Kashif1235mkh@yahoo.com', N'123', CAST(N'2019-09-30T20:43:08.787' AS DateTime), N'kashif133', CAST(N'2019-09-30T20:43:08.787' AS DateTime), N'kashif133', 5, CAST(N'2019-09-30T20:43:08.787' AS DateTime), 0, N'37.111.135.60', N'0', N'cus_FuKOHtNVwibq8W', NULL, NULL, NULL, NULL, NULL, N'
OC (01 Oct, 2019 16:12): Hi 
OC (01 Oct, 2019 16:12): Please verify this profile', NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (17, N'b6797bd8-017b-4384-8386-4de06215d783', N'Lucas', NULL, N'sga2acc@gmail.com', N'test1234', CAST(N'2019-10-01T07:45:34.323' AS DateTime), N'Lucas', CAST(N'2019-10-01T07:45:34.323' AS DateTime), N'Lucas', 5, CAST(N'2019-10-01T07:45:34.323' AS DateTime), 0, N'5.90.96.227', N'0', N'cus_FuV7C1lgRDHxLU', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (20, N'e489e35e-152f-4d07-8391-888cd71eb9b5', N'kashif1334', NULL, N'Kashifmkh1235@yahoo.com', N'123', CAST(N'2019-10-01T17:53:07.303' AS DateTime), N'kashif1334', CAST(N'2019-10-01T17:53:07.303' AS DateTime), N'kashif1334', 5, CAST(N'2019-10-01T17:53:07.303' AS DateTime), 0, N'103.7.79.53', N'0', N'cus_Fuet38wvmXzDkC', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (28, N'53de089a-6878-4796-9e03-c1eaa15cb1f4', N'Syed Saqib', NULL, N'sraza@crovtech.com', N'123', CAST(N'2019-10-02T18:59:26.030' AS DateTime), N'Syed Saqib', CAST(N'2019-10-02T18:59:26.030' AS DateTime), N'Syed Saqib', 5, CAST(N'2019-10-02T18:59:26.030' AS DateTime), 0, N'119.160.101.131', N'0', N'cus_Fv39A7QpRSWJ8f', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (29, N'd37d8def-792b-40df-9d2b-e43b22f17ff6', N'kashif133', NULL, N'Kashifmkh@yahoo.com', N'123', CAST(N'2019-10-02T19:00:28.247' AS DateTime), N'kashif133', CAST(N'2019-10-02T19:00:28.247' AS DateTime), N'kashif133', 5, CAST(N'2019-10-02T19:00:28.247' AS DateTime), 0, N'103.7.79.107', N'0', N'cus_Fv3BVyTajv88Gi', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (32, N'f3830899-7fd0-470c-8973-5ec83da1259f', N'hjamil@crovtech.com', NULL, N'hjamil@crovtech.com', N'123', CAST(N'2019-10-02T20:01:30.293' AS DateTime), N'hjamil@crovtech.com', CAST(N'2019-10-02T20:01:30.293' AS DateTime), N'hjamil@crovtech.com', 5, CAST(N'2019-10-02T20:01:30.293' AS DateTime), 0, N'103.7.79.107', N'0', N'cus_Fv49YDxNaovrba', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (33, N'e3045b21-4464-4385-97cc-e90d4b16f5c2', N'Lucas', NULL, N'lucas.eyre123@gmail.com', N'test1234', CAST(N'2019-10-02T20:19:35.233' AS DateTime), N'Lucas', CAST(N'2019-10-02T20:19:35.233' AS DateTime), N'Lucas', 5, CAST(N'2019-10-02T20:19:35.233' AS DateTime), 0, N'5.90.236.119', N'0', N'cus_Fv4SmSNurWgExx', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (34, N'c9deeefc-4da8-40dd-b8a3-90fb09837632', N'Lucas', NULL, N'lucas.eyre321@gmail.com', N'test', CAST(N'2019-10-02T20:26:04.560' AS DateTime), N'Lucas', CAST(N'2019-10-02T20:26:04.560' AS DateTime), N'Lucas', 5, CAST(N'2019-10-02T20:26:04.560' AS DateTime), 0, N'5.90.236.119', N'0', N'cus_Fv4Y3ZZjahsVhB', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (36, N'd5304287-09f5-44bb-8037-5c2164bbdefc', N'Vanessa Ball', NULL, N'VanessaBall791pE48@yahoo.com', N'M9DkySFddH', CAST(N'2019-10-03T16:45:34.830' AS DateTime), N'Vanessa Ball', CAST(N'2019-10-03T16:45:34.830' AS DateTime), N'Vanessa Ball', 5, CAST(N'2019-10-03T16:45:34.830' AS DateTime), 0, N'103.255.4.16', N'0', N'cus_FvODgyy68pIkmT', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (37, N'625108b8-9a1f-4412-a2a7-eb44c7e2bbaf', N'Angela Scott', NULL, N'AngelaScott366gbxO@yahoo.com', N'TaIttqOaIt', CAST(N'2019-10-03T17:20:22.037' AS DateTime), N'Angela Scott', CAST(N'2019-10-03T17:20:22.037' AS DateTime), N'Angela Scott', 5, CAST(N'2019-10-03T17:20:22.037' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvOpSnwyeVsaOh', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (38, N'11c5bcfa-0a18-42aa-9599-4f4042bfeae4', N'Elizabeth Chapman', NULL, N'ElizabethChapman5331b2B@yahoo.com', N'yDNl1LQdDx', CAST(N'2019-10-03T17:53:14.150' AS DateTime), N'Elizabeth Chapman', CAST(N'2019-10-03T17:53:14.150' AS DateTime), N'Elizabeth Chapman', 5, CAST(N'2019-10-03T17:53:14.150' AS DateTime), 0, N'::1', N'0', N'cus_FvPNUjg96dRR6Q', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (39, N'c5e837e6-fc52-43a0-b06b-f8cbfd6c0100', N'Julia Blake', NULL, N'JuliaBlake189e3Yw@yahoo.com', N'nk3ZvIGMg0', CAST(N'2019-10-03T18:28:31.107' AS DateTime), N'Julia Blake', CAST(N'2019-10-03T18:28:31.107' AS DateTime), N'Julia Blake', 5, CAST(N'2019-10-03T18:28:31.107' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvPte2ct8JCt51', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (40, N'8ac9f1ca-b36f-48a8-9222-8b1697e961ff', N'Syed Saqib', NULL, N'sraza1@crovtech.com', N'123', CAST(N'2019-10-03T18:32:15.820' AS DateTime), N'Syed Saqib', CAST(N'2019-10-03T18:32:15.820' AS DateTime), N'Syed Saqib', 5, CAST(N'2019-10-03T18:32:15.820' AS DateTime), 0, N'::1', N'0', N'cus_FvPw3U7IXItt8x', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (41, N'ea9f2502-ddd6-47a0-a187-a079ad4af16c', N'Sonia Langdon', NULL, N'SoniaLangdon414Lufk@yahoo.com', N'r7MYaklSin', CAST(N'2019-10-03T19:10:41.280' AS DateTime), N'Sonia Langdon', CAST(N'2019-10-03T19:10:41.280' AS DateTime), N'Sonia Langdon', 5, CAST(N'2019-10-03T19:10:41.280' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvQZt3qy1SA6rh', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (42, N'b272b3fe-fca4-4372-9c19-3a09247508bd', N'bernicec', NULL, N'bernicec9r4@gmx.com', N'iT93l8y12', CAST(N'2019-10-03T20:37:08.180' AS DateTime), N'bernicec', CAST(N'2019-10-03T20:37:08.180' AS DateTime), N'bernicec', 5, CAST(N'2019-10-03T20:37:08.180' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvRxRJ9SlgO0tu', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (43, N'9143c50c-88f9-4029-8dcb-81e0bc8ff31d', N'dalilau0peeks', NULL, N'dalilau0peeks@gmx.com', N'fao2Hvr9hxs', CAST(N'2019-10-03T20:45:29.280' AS DateTime), N'dalilau0peeks', CAST(N'2019-10-03T20:45:29.280' AS DateTime), N'dalilau0peeks', 1, CAST(N'2019-10-03T20:45:29.280' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvS6RVmT2kiWXD', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (44, N'91dcb2b9-ee68-4f5b-b51d-c0b2fe5165cb', N'shakitab', NULL, N'shakitab37s@gmx.com', N'sshm2Cyv09v', CAST(N'2019-10-03T20:56:45.340' AS DateTime), N'shakitab', CAST(N'2019-10-03T20:56:45.340' AS DateTime), N'shakitab', 1, CAST(N'2019-10-03T20:56:45.340' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvSGP8lVfr9ha4', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (45, N'8c0a0386-81da-447d-aba0-8f1ad5f26ae7', N'alfredacebotv2s', NULL, N'alfredacebotv2s@gmx.com', N'uzmlHtod2', CAST(N'2019-10-03T21:03:40.003' AS DateTime), N'alfredacebotv2s', CAST(N'2019-10-03T21:03:40.003' AS DateTime), N'alfredacebotv2s', 1, CAST(N'2019-10-03T21:03:40.003' AS DateTime), 0, N'119.160.100.43', N'0', N'cus_FvSOoCHZYcX1HQ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (46, N'dccdd876-461a-4471-b460-e6b2cc237701', N'Delmy Biparuta', NULL, N'delmybiparuta@gmx.com', N'123123', CAST(N'2019-10-04T10:18:19.673' AS DateTime), N'Delmy Biparuta', CAST(N'2019-10-04T10:18:19.673' AS DateTime), N'Delmy Biparuta', 5, CAST(N'2019-10-04T10:18:19.673' AS DateTime), 0, N'39.50.108.215', N'0', N'cus_FvfDRba5tc1Rlm', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (47, N'89810b76-df92-4335-a21c-15a0841fc996', N'Lynda Acutera', NULL, N'lyndaacuterra@gmx.com', N'123123', CAST(N'2019-10-04T12:11:29.637' AS DateTime), N'Lynda Acutera', CAST(N'2019-10-04T12:11:29.637' AS DateTime), N'Lynda Acutera', 5, CAST(N'2019-10-04T12:11:29.637' AS DateTime), 0, N'39.50.97.154', N'0', N'cus_Fvh2FxKijELxRY', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (48, N'38b67aa8-9b32-4bfb-ad05-de7a0480297d', N'Domenic', NULL, N'domenicrwc@gmail.com', N'123123', CAST(N'2019-10-04T12:59:48.237' AS DateTime), N'Domenic', CAST(N'2019-10-04T12:59:48.237' AS DateTime), N'Domenic', 5, CAST(N'2019-10-04T12:59:48.237' AS DateTime), 0, N'39.50.97.154', N'0', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (49, N'600e3441-b66e-4ae0-b45a-01f0e7ba8aef', N'France', NULL, N'francejqs@gmx.com', N'123123', CAST(N'2019-10-04T13:01:49.900' AS DateTime), N'France', CAST(N'2019-10-04T13:01:49.900' AS DateTime), N'France', 5, CAST(N'2019-10-04T13:01:49.900' AS DateTime), 0, N'39.50.97.154', N'0', N'cus_FvhrG2POJR1kDn', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (50, N'22eb4e9f-df90-4ea2-8014-78bf29c8ca3f', N'deangelo692cka', NULL, N'deangelo692cka@gmx.com', N'qs00iqdY67', CAST(N'2019-10-04T19:11:22.550' AS DateTime), N'deangelo692cka', CAST(N'2019-10-04T19:11:22.550' AS DateTime), N'deangelo692cka', 5, CAST(N'2019-10-04T19:11:22.550' AS DateTime), 0, N'103.255.5.98', N'0', N'cus_FvnoX6bgN8AxKi', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (52, N'33870272-d959-45fe-a3c2-1a44dc5b6223', N'rue4jm5parez', NULL, N'rue4jm5parez@gmx.com', N'vmKcx2cm9', CAST(N'2019-10-04T23:06:33.563' AS DateTime), N'rue4jm5parez', CAST(N'2019-10-04T23:06:33.563' AS DateTime), N'rue4jm5parez', 5, CAST(N'2019-10-04T23:06:33.563' AS DateTime), 0, N'119.160.100.169', N'0', N'cus_Fvra648e5VIKmP', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (53, N'8fb4865d-552e-4e5d-8bd5-d05c6fca71ee', N'cammiesilker1j', NULL, N'cammiesilker1j@gmx.com', N'ef687Vc7x', CAST(N'2019-10-05T07:59:42.423' AS DateTime), N'cammiesilker1j', CAST(N'2019-10-05T07:59:42.423' AS DateTime), N'cammiesilker1j', 1, CAST(N'2019-10-05T07:59:42.423' AS DateTime), 0, N'119.160.100.169', N'0', N'cus_Fw0ChkMe6dSiaw', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer] ([CustomerId], [GUID], [FirstName], [SurName], [EmailAddress], [Password], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [StatusId], [LastLoginDate], [LoginAttempts], [LastLoginIP], [Tocken], [StripeCustomerId], [UserName], [Source], [Register], [ResponsibleTeamMemberId], [AvailableToEveryOne], [Comment], [CancelledDate], [IsOptedEducationalEmailSeries], [IsOptedMarketingEmail], [Title]) VALUES (54, N'1159545b-dba4-41eb-a1e8-d1d972843083', N'rebeccannemarqzg', NULL, N'rebeccannemarqzg@gmx.com', N'y193iyJh966', CAST(N'2019-10-05T08:23:23.587' AS DateTime), N'rebeccannemarqzg', CAST(N'2019-10-05T08:23:23.587' AS DateTime), N'rebeccannemarqzg', 1, CAST(N'2019-10-05T08:23:23.587' AS DateTime), 0, N'119.160.100.169', N'0', N'cus_Fw0bVE3ctq6UAJ', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SG2_Customer] OFF
SET IDENTITY_INSERT [dbo].[SG2_Customer_ContactDetail] ON 

INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'AB10B2DA-F092-4F1D-A6EF-6C0604EB41C9', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (2, 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'A72DE126-84A4-4266-9ADD-06097D4FE11D', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (4, 4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'CA1D59B6-22D3-4C64-B1D1-724767330AD8', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (6, 6, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'48212771-7B64-429C-B2F5-2D7F5C5BEBFC', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (7, 7, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'2DA2B99D-1809-4122-B836-A6F8E65AAF60', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (8, 8, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'10CC6C62-6135-4EF4-9ED2-83DDE7130580', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (9, 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'55FE12DD-5196-4F7F-947F-D61B83623C9A', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (12, 12, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'2E7A3B29-910D-4790-A6F9-44646FFD3CDE', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (15, 15, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'138D57A5-D1A3-429F-ADAC-ACAE71039EF0', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (17, 17, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'B4B5BEE8-E66D-494B-AEC7-92320930D273', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (20, 20, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'4C16ABFA-2F53-465C-A661-0E312BC71599', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (28, 28, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'DEF53FF4-3B7C-4703-91D7-8A346BFBEDC0', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (29, 29, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'0CFC441A-ABFA-4E9C-BAFF-AC903EDDA0EC', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (32, 32, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'93916DC0-6C6D-417E-A7A6-FB5E99B1638E', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (33, 33, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'59404206-7E6F-4B81-A859-B3E2329A7196', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (34, 34, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'3A667383-69DD-44CD-9938-7F2104556EBF', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (36, 36, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D76B4283-1692-4DF0-AB65-CEC4DC361D0E', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (37, 37, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'F05BA01F-B7A9-45AF-8A07-5A6DCB7CE70D', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (38, 38, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'FD1AE6B4-A8BB-4BC3-84DD-0D23319CDA2A', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (39, 39, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'F869A2B5-3EB8-4DBC-A59B-A1C83318695C', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (40, 40, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'08843A30-475B-4A7A-BBC2-50F0465A8232', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (41, 41, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'B2724647-6829-41A6-89E1-9F678D6C4233', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (42, 42, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'3C9B75E1-91FE-4725-AC47-D41B0B71573C', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (43, 43, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'884B6182-3FFD-442F-8569-208D616DF442', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (44, 44, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'1BD11144-4249-47E5-B324-D7CA6B78D2BF', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (45, 45, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'232C87FE-002B-4860-BA74-80713120088F', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (46, 46, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'E5ECDA03-255F-4A05-A0B2-D79EA77AF9DE', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (47, 47, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'00B1E8B2-6C36-4FD6-A77C-AA72B8B3DBD3', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (48, 48, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'F33A4494-6409-4CD9-9419-7D881B2BCDCC', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (49, 49, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'585538B4-2B91-4CB1-A2DD-46243CACFD35', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (50, 50, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'ABE521FA-2091-460B-8728-9AC1B9F4166C', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (52, 52, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'D7FB6F14-BFA2-4999-8208-F465D98D3197', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (53, 53, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'E7B36F12-179F-48FD-BD7F-BC9324D70F99', NULL, NULL, NULL)
INSERT [dbo].[SG2_Customer_ContactDetail] ([ContactDetailsId], [CustomerId], [JobTitle], [MobileNumber], [PhoneNumber], [AddressLine1], [AddressLine2], [City], [Sate], [Country], [PostalCode], [GUID], [PhoneCode], [ScheduleCallDate], [Notes]) VALUES (54, 54, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'619DEA03-6033-41F3-89EE-A98AE222F39F', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SG2_Customer_ContactDetail] OFF
SET IDENTITY_INSERT [dbo].[SG2_Customer_Title] ON 

INSERT [dbo].[SG2_Customer_Title] ([PkTitleId], [TitleName]) VALUES (1, N'Mrs')
INSERT [dbo].[SG2_Customer_Title] ([PkTitleId], [TitleName]) VALUES (2, N'Mr')
SET IDENTITY_INSERT [dbo].[SG2_Customer_Title] OFF
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (1, N'Customer', N'Customers Status')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (2, N'User', N'User Status')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (3, N'JV', N'JVStatus')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (4, N'General', N'General Status')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (5, N'SystemUser', N'System Users Status')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (6, N'Subscription', N'Payment Plan Subscription')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (7, N'JVServerType', N'Jarvee Server type')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (8, N'SocialMedia', N'SocialMedia')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (9, N'QueueStatus', N'Queue Status')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (10, N'QueueTypes', N'Queue Types')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (11, N'ProxyIP', N'Proxy IP')
INSERT [dbo].[SG2_Enumeration] ([EnumerationId], [Name], [Description]) VALUES (12, N'Notification', N'Notofication Status')
SET IDENTITY_INSERT [dbo].[SG2_EnumerationValue] ON 

INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (1, 1, N'Live', N'', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (2, 1, N'NewUnRegistered', N'', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (3, 1, N'Cancelled', N'', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (4, 1, N'Deleted', N'Deleted', 0, 4, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (5, 1, N'Active', N'Active', 0, 5, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (6, 1, N'InActive', N'InActive', 0, 6, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (7, 1, N'EmailverificationRequired', N'Email verification Required', 0, 7, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (8, 2, N'Active', N'Active', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (9, 2, N'InActive', N'InActive', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (10, 2, N'Deleted', N'Deleted', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (11, 3, N'Profile adding', N'Profile adding', 1, 1, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (12, 3, N'Password to be updated', N'Password to be updated', 0, 11, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (13, 3, N'Whitelist to be updated', N'Whitelist to be updated', 0, 12, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (14, 3, N'Targeting to be updated', N'Targetting to be updated', 0, 10, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (15, 3, N'Profile requires cancelling', N'Requires cancelling', 0, 13, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (16, 3, N'Canceled on MP', N'Canceled on MP', 0, 14, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (17, 3, N'Invalid credentials', N'Invalid credentials', 0, 5, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (18, 4, N'Deleted', N'Deleted', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (19, 4, N'Active', N'Active', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (20, 4, N'InActive', N'InActive', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (21, 5, N'Invited', N'Invite new user', 1, 1, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (22, 5, N'Active', N'Active User', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (23, 5, N'Suspend', N'Suspend User', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (24, 5, N'Delete', N'Delete User', 0, 4, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (25, 6, N'Active', N'Active Subscription', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (26, 6, N'canceled', N'canceled Subscription', 1, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (27, 6, N'Unsubscribe', N'Unsubscribe Subscription', 1, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (28, 7, N'Likey', N'Likey Jarvee Server', 0, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (29, 7, N'GrowthEngine', N'Growth Engine Jarvee Server', 1, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (30, 8, N'Instagram', N'Instagram', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (31, 3, N'Invalid credentials Re-Send', N'Invalid credentials Re-Send', 0, 6, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (32, 3, N'2FA Code Required', N'Two factor authentication', 0, 8, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (33, 3, N'API block', N'API block', 0, 9, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (34, 3, N'Cancelled on SG', N'Cancelled on SG', 0, 17, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (35, 3, N'Profile deleted', N'Profile deleted', 0, 18, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (36, 3, N'Email verification required', N'Email verification required', 0, 7, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (37, 3, N'Subscription cancellation in progress', N'Subscription cancellation in progress', 0, 15, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (38, 3, N'Like exchanged updated', N'Like exchange update', 0, 16, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (39, 3, N'Valid and not set up', N'Valid and not set up', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (40, 3, N'Valid and set up live', N'Valid and set up live', 0, 4, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (41, 3, N'Accounts loaded but not verified', N'Accounts loaded but not verified', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (42, 3, N'Accounts to be loaded', N'Accounts to be loaded', 0, 0, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (43, 9, N'Pending', N'Pending', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (44, 9, N'Inprogress', N'Inprogress', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (45, 9, N'Completed', N'Completed', 0, 3, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (46, 10, N'Regular', N'Regular', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (47, 10, N'RPC', N'RPC', 0, 2, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (48, 9, N'Error', N'Error', 0, 4, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (49, 11, N'BadProxy', N'Bad Proxy', 1, 1, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (50, 3, N'ProxyAndInternetIssue', N'Proxy And Internet Issue', 0, 19, 0)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (51, 12, N'Unread', N'Unread', 0, 4, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (52, 12, N'Read', N'Read', 0, 5, 1)
INSERT [dbo].[SG2_EnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (53, 12, N'Close', N'Close', 0, 6, 1)
SET IDENTITY_INSERT [dbo].[SG2_EnumerationValue] OFF
SET IDENTITY_INSERT [dbo].[SG2_JVBox] ON 

INSERT [dbo].[SG2_JVBox] ([JVBoxId], [BoxName], [AdminName], [AdminPassword], [BoxManagedBy], [SupportPhone], [SupportEmail], [HostedBy], [HostingPhone], [HostingWebsite], [HostingAccount], [HostingPassword], [HostingPriceInfo], [StatusId], [MaxLimit], [JVBoxType], [ExchangeName], [QueueStatusId], [PRCExecProfileId], [ServerRunningStatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [Updateby]) VALUES (1, N'SGA - Dev + Staging -95.217.99.133', N'Administrator', N'--', N'--', N'--', N'me@me.com', N'--', N'--', N'--', N'193.200.241.54', N'--', N'--', 20, 0, 29, N'167.86.104.68-Regular,167.86.104.68-RPC', NULL, NULL, 2, NULL, NULL, NULL, NULL)
INSERT [dbo].[SG2_JVBox] ([JVBoxId], [BoxName], [AdminName], [AdminPassword], [BoxManagedBy], [SupportPhone], [SupportEmail], [HostedBy], [HostingPhone], [HostingWebsite], [HostingAccount], [HostingPassword], [HostingPriceInfo], [StatusId], [MaxLimit], [JVBoxType], [ExchangeName], [QueueStatusId], [PRCExecProfileId], [ServerRunningStatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [Updateby]) VALUES (2, N'SGA1-95.217.99.130', N'--', N'--', N'--', N'--', N'me@mme.com', N'--', N'--', N'--', N'91.205.174.118', N'--', N'--', 19, 300, 29, N'5.189.172.25-Regular,5.189.172.25-RPC', 45, NULL, 2, NULL, NULL, CAST(N'2019-10-05T08:29:35.813' AS DateTime), N'MP Bot')
INSERT [dbo].[SG2_JVBox] ([JVBoxId], [BoxName], [AdminName], [AdminPassword], [BoxManagedBy], [SupportPhone], [SupportEmail], [HostedBy], [HostingPhone], [HostingWebsite], [HostingAccount], [HostingPassword], [HostingPriceInfo], [StatusId], [MaxLimit], [JVBoxType], [ExchangeName], [QueueStatusId], [PRCExecProfileId], [ServerRunningStatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [Updateby]) VALUES (3, N'SGA2-95.217.99.131', N'--', N'--', N'--', N'--', N'me@me.com', N'--', N'--', N'--', N'173.212.240.187', N'--', N'--', 19, 300, 29, N'173.212.240.187-Regular,173.212.240.187-RPC', 45, NULL, 1, NULL, NULL, CAST(N'2019-10-05T08:23:15.320' AS DateTime), N'MP Bot')
SET IDENTITY_INSERT [dbo].[SG2_JVBox] OFF
SET IDENTITY_INSERT [dbo].[SG2_LikeyAccount] ON 

INSERT [dbo].[SG2_LikeyAccount] ([LikeyAccountId], [InstaUserName], [InstaPassword], [Country], [City], [Gender], [HashTag], [StatusId]) VALUES (1, N'Alistair_MacLea', N'123', N'2', N'3', 1, N'#novelist fg', 19)
SET IDENTITY_INSERT [dbo].[SG2_LikeyAccount] OFF
SET IDENTITY_INSERT [dbo].[SG2_Proxy] ON 

INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (10, N'45.124.53.12', N'baysocial1,69np2nxxm7jb', N'6', N'3', N'-37.839610, 144.942280', 19, 5, N'3177', 1)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (11, N'103.73.65.65', N'baysocial1,69np2nxxm7jb', N'5', N'3', N'-33.8688197,151.2092955', 19, 10, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (12, N'27.100.36.71', N'baysocial1,69np2nxxm7jb', N'5', N'3', N'-33.8688197,151.2092955', 19, 10, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (13, N'112.213.32.139', N'baysocial1,69np2nxxm7jb', N'5', N'3', N'-33.8688197,151.2092955', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (14, N'103.1.186.91', N'baysocial1,69np2nxxm7jb', N'5', N'3', N'-33.8688197,151.2092955', 19, 8, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (15, N'172.98.73.238', N'baysocial1,69np2nxxm7jb', N'25', N'30', N'43.6532260,-79.3831843', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (16, N'172.98.73.239', N'baysocial1,69np2nxxm7jb', N'25', N'30', N'43.6532260,-79.3831843', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (17, N'172.98.73.47', N'baysocial1,69np2nxxm7jb', N'25', N'30', N'43.6532260,-79.3831843', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (18, N'172.98.73.45', N'baysocial1,69np2nxxm7jb', N'25', N'30', N'43.6532260,-79.3831843', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (19, N'192.111.132.42', N'baysocial1,69np2nxxm7jb', N'21', N'30', N'49.2827291,-123.1207375', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (20, N'192.111.132.99', N'baysocial1,69np2nxxm7jb', N'21', N'30', N'49.2827291,-123.1207375', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (21, N'88.202.182.183', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (22, N'146.185.21.194', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (23, N'88.202.190.52', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (24, N'31.24.224.201', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (25, N'88.202.179.249', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (26, N'88.202.190.75', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (27, N'88.202.185.210', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (28, N'89.33.6.17', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 11, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (29, N'31.24.230.71', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (30, N'45.58.57.60', N'baysocial1,69np2nxxm7jb', N'20', N'1', N'33.7489954,-84.3879824', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (31, N'198.8.93.114', N'baysocial1,69np2nxxm7jb', N'24', N'1', N'41.8781136,-87.6297982', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (32, N'198.8.93.6', N'baysocial1,69np2nxxm7jb', N'24', N'1', N'41.8781136,-87.6297982', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (33, N'162.211.64.253', N'baysocial1,69np2nxxm7jb', N'29', N'1', N'39.7392358,-104.9902510', 19, 12, N'3177', 1)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (34, N'173.244.195.11', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (35, N'104.128.228.164', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (36, N'104.128.238.160', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (37, N'107.181.170.62', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (38, N'172.98.94.155', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (39, N'107.181.170.40', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (40, N'107.173.57.202', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (41, N'172.98.77.199', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (42, N'199.187.208.112', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (43, N'192.111.142.91', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (44, N'172.98.77.129', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (45, N'162.220.51.146', N'baysocial1,69np2nxxm7jb', N'34', N'1', N'33.4483771,-112.0740373', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (46, N'209.95.52.182', N'baysocial1,69np2nxxm7jb', N'2', N'1', N'40.7127753,-74.0059728', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (47, N'103.11.64.27', N'baysocial1,69np2nxxm7jb', N'23', N'1', N'38.9071923,-77.0368707', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (48, N'103.11.64.29', N'baysocial1,69np2nxxm7jb', N'23', N'1', N'38.9071923,-77.0368707', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (49, N'103.11.64.50', N'baysocial1,69np2nxxm7jb', N'23', N'1', N'38.9071923,-77.0368707', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (50, N'199.187.208.37', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (51, N'172.98.77.91', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (52, N'199.187.208.59', N'baysocial1,69np2nxxm7jb', N'30', N'1', N'25.7616798,-80.1917902', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (53, N'107.181.170.59', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (54, N'172.98.94.155', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (55, N'172.98.94.164', N'baysocial1,69np2nxxm7jb', N'1', N'1', N'34.0522342,-118.2436849', 19, 9, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (56, N'162.220.51.70', N'baysocial1,69np2nxxm7jb', N'34', N'1', N'33.4483771,-112.0740373', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (57, N'162.211.64.56', N'baysocial1,69np2nxxm7jb', N'29', N'1', N'39.7392358,-104.9902510', 19, 12, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (58, N'88.202.190.62', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (59, N'88.202.185.209', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (60, N'88.202.188.24', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (61, N'88.202.185.212', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (62, N'88.202.190.61', N'baysocial1,69np2nxxm7jb', N'3', N'2', N'51.5073509,-0.1277583', 19, 7, N'3177', NULL)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (63, N'287.16.120.233', N'Baysocial1,69np2nxxm7jb', N'19', N'28', N'1.3520830,103.8198360', 19, 13, N'3177', 1)
INSERT [dbo].[SG2_Proxy] ([ProxyId], [ProxyIPNumber], [ProxyIPName], [BaseCity], [BaseCountry], [GeoPoints], [StatusId], [VPSSId], [ProxyPort], [NoOfMaxRetry]) VALUES (64, N'233.565.57.345', N'Baysocial1,69np2nxxm7jb', N'20', N'31', N'33.7489954,-84.3879824', 19, 14, N'3177', NULL)
SET IDENTITY_INSERT [dbo].[SG2_Proxy] OFF
INSERT [dbo].[SG2_QueueAuditEnumeration] ([EnumerationId], [Name], [Description]) VALUES (1, N'QueueTypes', N'Queue Types')
INSERT [dbo].[SG2_QueueAuditEnumeration] ([EnumerationId], [Name], [Description]) VALUES (2, N'QueueStatus', N'Queue Status')
SET IDENTITY_INSERT [dbo].[SG2_QueueAuditEnumerationValue] ON 

INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (1, 1, N'Regular', N'Regular', 0, 1, 1)
INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (2, 1, N'RPC', N'RPC', 1, 2, 1)
INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (3, 2, N'Error', N'Error', 0, 4, 1)
INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (4, 2, N'Inprogress', N'Inprogress', 0, 2, 1)
INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (5, 2, N'Pending', N'Pending', 1, 1, 1)
INSERT [dbo].[SG2_QueueAuditEnumerationValue] ([EnumerationValueId], [EnumerationId], [Name], [Description], [IsDefault], [SequenceNo], [IsVisible]) VALUES (6, 2, N'Processed', N'Processed', 0, 3, 1)
SET IDENTITY_INSERT [dbo].[SG2_QueueAuditEnumerationValue] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile] ON 

INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (1, 1, NULL, 3, 36, 19, NULL, N'abdullahbcy', N'Abdullah10', N'Social Profile 1', 6, 3, CAST(N'2019-09-18T07:12:03.393' AS DateTime), N'hassanjamil.bwp@gmail.com', CAST(N'2019-10-04T23:34:31.447' AS DateTime), N'hassanjamil.bwp@gmail.com', NULL, NULL, NULL, N'123456', 1)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (2, 2, NULL, 2, 33, 19, NULL, N'lucas_eyre', N'vuvMzryjax]e4M4', N'Social Profile 1', 1, 1, CAST(N'2019-09-18T09:40:32.357' AS DateTime), N'lucas.eyre@gmail.com', CAST(N'2019-10-02T20:08:42.080' AS DateTime), N'lucas.eyre@gmail.com', 0, CAST(N'2019-10-03T20:06:38.140' AS DateTime), NULL, NULL, 1)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (4, 4, NULL, NULL, NULL, 19, NULL, NULL, NULL, N'Social Profile 1', NULL, NULL, CAST(N'2019-09-19T18:24:28.590' AS DateTime), N'anthony@einfinity.email', CAST(N'2019-09-19T18:24:28.590' AS DateTime), N'anthony@einfinity.email', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (6, 6, NULL, 2, 42, 19, NULL, N'abdullahbcy', N'Abdullah10', N'Social Profile 1', 3, 2, CAST(N'2019-09-23T07:34:21.753' AS DateTime), N'abdullahbcy@gmail.com', CAST(N'2019-09-23T08:22:40.587' AS DateTime), N'abdullahbcy@gmail.com', NULL, NULL, NULL, N'412976', 1)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (7, 7, NULL, NULL, NULL, 19, NULL, NULL, NULL, N'Social Profile 1', NULL, NULL, CAST(N'2019-09-23T21:58:53.027' AS DateTime), N'Sam@atlantafinehomes.com', CAST(N'2019-09-29T00:46:46.457' AS DateTime), N'Sam@atlantafinehomes.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (8, 8, NULL, NULL, NULL, 19, NULL, NULL, NULL, N'Social Profile 1', NULL, NULL, CAST(N'2019-09-24T06:42:19.300' AS DateTime), N'omar.c@me.com', CAST(N'2019-09-24T06:42:19.300' AS DateTime), N'omar.c@me.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (9, 9, NULL, 2, 42, 19, NULL, N'bethany.hogarth', N'BZ3a5gg6', N'Social Profile 1', 30, 1, CAST(N'2019-09-24T09:01:14.703' AS DateTime), N'hello@thebaysocial.com', CAST(N'2019-10-01T21:27:39.027' AS DateTime), N'hello@thebaysocial.com', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (12, 12, NULL, NULL, 42, 19, NULL, NULL, NULL, N'Social Profile 1', NULL, NULL, CAST(N'2019-09-28T07:15:53.113' AS DateTime), N'w.sardar000@gmial.com', CAST(N'2019-09-28T07:18:16.600' AS DateTime), N'w.sardar000@gmial.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (15, 15, NULL, 2, 42, 19, NULL, N'@mkashif1235', N'123', N'Social Profile 1', 25, 30, CAST(N'2019-09-30T20:43:08.793' AS DateTime), N'Kashif1235mkh@yahoo.com', CAST(N'2019-10-01T21:27:24.890' AS DateTime), N'Admin', 0, CAST(N'2019-10-01T20:46:38.577' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (17, 17, NULL, 2, 42, 19, NULL, N'lucas_eyre', N'vuvMzryjax]e4M4', N'Social Profile 1', 6, 3, CAST(N'2019-10-01T07:45:34.330' AS DateTime), N'sga2acc@gmail.com', CAST(N'2019-10-01T21:25:38.073' AS DateTime), N'MP Bot', 0, CAST(N'2019-10-02T07:52:34.073' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (20, 20, NULL, 2, 42, 19, NULL, N'@mkashif12356', N'456', N'Social Profile 1', 4, 2, CAST(N'2019-10-01T17:53:07.313' AS DateTime), N'Kashifmkh1235@yahoo.com', CAST(N'2019-10-01T21:26:05.093' AS DateTime), N'MP Bot', 0, CAST(N'2019-10-02T18:03:01.547' AS DateTime), NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (28, 28, NULL, 2, 39, 19, NULL, N'oops_horizon', N'S@q1b2030', N'Social Profile 1', 1, 1, CAST(N'2019-10-02T18:59:26.040' AS DateTime), N'sraza@crovtech.com', CAST(N'2019-10-02T19:03:36.650' AS DateTime), N'sraza@crovtech.com', NULL, NULL, NULL, N'251039', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (29, 29, NULL, 2, 32, 19, NULL, N'muhammadkashifhanif', N'K@sh1f2020', N'Social Profile 1', 4, 2, CAST(N'2019-10-02T19:00:28.270' AS DateTime), N'Kashifmkh@yahoo.com', CAST(N'2019-10-04T22:34:21.267' AS DateTime), N'Kashifmkh@yahoo.com', NULL, NULL, NULL, N'567382', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (32, 32, NULL, 3, 39, 19, NULL, N'hassanjamilbwp', N'p244w0rd', N'Social Profile 1', 3, 2, CAST(N'2019-10-02T20:01:30.303' AS DateTime), N'hjamil@crovtech.com', CAST(N'2019-10-02T20:05:46.253' AS DateTime), N'hjamil@crovtech.com', NULL, NULL, NULL, N'283647', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (33, 33, NULL, 3, 36, 19, NULL, N'dianequinnfttu564', N'd!W89?as', N'Social Profile 1', 1, 1, CAST(N'2019-10-02T20:19:35.253' AS DateTime), N'lucas.eyre123@gmail.com', CAST(N'2019-10-02T20:22:59.987' AS DateTime), N'lucas.eyre123@gmail.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (34, 34, NULL, 3, 39, 19, NULL, N'beyazcavus', N'wZ1HTVmM', N'Social Profile 1', 30, 1, CAST(N'2019-10-02T20:26:04.633' AS DateTime), N'lucas.eyre321@gmail.com', CAST(N'2019-10-02T20:28:10.550' AS DateTime), N'lucas.eyre321@gmail.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (36, 36, NULL, 3, 36, 19, NULL, N'vanessaball791pe48', N'M9DkySFddH', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T16:45:34.837' AS DateTime), N'VanessaBall791pE48@yahoo.com', CAST(N'2019-10-03T17:15:09.927' AS DateTime), N'VanessaBall791pE48@yahoo.com', NULL, NULL, NULL, N'123456', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (37, 37, NULL, 3, 36, 19, NULL, N'angelascott366gbxo', N'TaIttqOaIt', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T17:20:22.043' AS DateTime), N'AngelaScott366gbxO@yahoo.com', CAST(N'2019-10-03T17:26:33.623' AS DateTime), N'AngelaScott366gbxO@yahoo.com', NULL, NULL, NULL, N'666125', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (38, 38, NULL, 3, 42, 19, NULL, N'elizabethchapman5331b2b', N'yDNl1LQdDx', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T17:53:14.173' AS DateTime), N'ElizabethChapman5331b2B@yahoo.com', CAST(N'2019-10-03T22:57:38.360' AS DateTime), N'ElizabethChapman5331b2B@yahoo.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (39, 39, NULL, 3, 36, 19, NULL, N'juliablake189e3yw', N'nk3ZvIGMg0', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T18:28:31.187' AS DateTime), N'JuliaBlake189e3Yw@yahoo.com', CAST(N'2019-10-03T19:52:55.553' AS DateTime), N'JuliaBlake189e3Yw@yahoo.com', NULL, NULL, NULL, N'1234555', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (40, 40, NULL, 3, 42, 19, NULL, N'oops_horizon123', N'123', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T18:32:15.827' AS DateTime), N'sraza1@crovtech.com', CAST(N'2019-10-03T23:33:30.267' AS DateTime), N'sraza1@crovtech.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (41, 41, NULL, 3, 36, 19, NULL, N'sonialangdon414lufk', N'r7MYaklSin', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T19:10:41.310' AS DateTime), N'SoniaLangdon414Lufk@yahoo.com', CAST(N'2019-10-03T20:23:13.650' AS DateTime), N'SoniaLangdon414Lufk@yahoo.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (42, 42, NULL, 3, 39, 19, NULL, N'darylswinburne', N'iT93l8y12', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T20:37:08.200' AS DateTime), N'bernicec9r4@gmx.com', CAST(N'2019-10-03T20:41:00.953' AS DateTime), N'bernicec9r4@gmx.com', NULL, NULL, NULL, N'538261', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (43, 43, NULL, 3, 36, 19, NULL, N'ashillivings', N'fao2Hvr9hxs', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T20:45:29.290' AS DateTime), N'dalilau0peeks@gmx.com', CAST(N'2019-10-03T20:53:33.623' AS DateTime), N'dalilau0peeks@gmx.com', NULL, NULL, NULL, N'510463', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (44, 44, NULL, 3, 36, 19, NULL, N'mannieferdy', N'sshm2Cyv09v', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T20:56:45.360' AS DateTime), N'shakitab37s@gmx.com', CAST(N'2019-10-03T20:58:27.847' AS DateTime), N'shakitab37s@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (45, 45, NULL, 3, 39, 19, NULL, N'burkpeak', N'uzmlHtod2', N'Social Profile 1', 1, 1, CAST(N'2019-10-03T21:03:40.027' AS DateTime), N'alfredacebotv2s@gmx.com', CAST(N'2019-10-03T21:07:09.527' AS DateTime), N'alfredacebotv2s@gmx.com', NULL, NULL, NULL, N'068197', 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (46, 46, NULL, 3, 36, 19, NULL, N'deliamunsey', N'c3ynx4mKs', N'Social Profile 1', 3, 2, CAST(N'2019-10-04T10:18:19.680' AS DateTime), N'delmybiparuta@gmx.com', CAST(N'2019-10-04T10:21:25.547' AS DateTime), N'delmybiparuta@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (47, 47, NULL, 3, 11, 19, NULL, N'shanehammerberger', N'tF9js37n1', N'Social Profile 1', 6, 3, CAST(N'2019-10-04T12:11:29.677' AS DateTime), N'lyndaacuterra@gmx.com', CAST(N'2019-10-04T12:47:48.140' AS DateTime), N'lyndaacuterra@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (48, 48, NULL, NULL, NULL, 19, NULL, NULL, NULL, N'Social Profile 1', NULL, NULL, CAST(N'2019-10-04T12:59:48.257' AS DateTime), N'domenicrwc@gmail.com', CAST(N'2019-10-04T12:59:48.257' AS DateTime), N'domenicrwc@gmail.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (49, 49, NULL, 3, 42, 19, NULL, N'mickipeltzer', N'ti18q0tqYa', N'Social Profile 1', 4, 2, CAST(N'2019-10-04T13:01:49.920' AS DateTime), N'francejqs@gmx.com', CAST(N'2019-10-04T13:03:28.770' AS DateTime), N'francejqs@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (50, 50, NULL, 3, 36, 19, NULL, N'georgiannabayfield', N'qs00iqdY67', N'Social Profile 1', 1, 1, CAST(N'2019-10-04T19:11:22.570' AS DateTime), N'deangelo692cka@gmx.com', CAST(N'2019-10-04T19:43:03.853' AS DateTime), N'deangelo692cka@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (52, 52, NULL, 2, 36, 19, NULL, N'ulrickformby', N'vmKcx2cm9', N'Social Profile 1', 1, 1, CAST(N'2019-10-04T23:06:33.573' AS DateTime), N'rue4jm5parez@gmx.com', CAST(N'2019-10-04T23:33:19.820' AS DateTime), N'rue4jm5parez@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (53, 53, NULL, 2, 36, 19, NULL, N'siusanperchard', N'ef687Vc7x', N'Social Profile 1', 1, 1, CAST(N'2019-10-05T07:59:42.440' AS DateTime), N'cammiesilker1j@gmx.com', CAST(N'2019-10-05T08:07:12.680' AS DateTime), N'cammiesilker1j@gmx.com', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[SG2_SocialProfile] ([SocialProfileId], [CustomerId], [SocialProfileTypeId], [JVBoxId], [JVBoxStatusId], [StatusId], [StripeCustomerId], [SocialUsername], [SocialPassword], [SocialProfileName], [SocialPrefferedCity], [SocialPrefferedCountry], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [JVAttempts], [JVAttemptsBlockedTill], [JVAttemptStatus], [verificationCode], [IsArchived]) VALUES (54, 54, NULL, 2, 33, 19, NULL, N'lonnardreason', N'y193iyJh966', N'Social Profile 1', 3, 2, CAST(N'2019-10-05T08:23:23.597' AS DateTime), N'rebeccannemarqzg@gmx.com', CAST(N'2019-10-05T08:29:34.917' AS DateTime), N'rebeccannemarqzg@gmx.com', NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_BadProxy] ON 

INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (1, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (2, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (3, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (4, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (5, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (6, 10, 2, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (7, 10, 1, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (8, 0, 1, 49, 6)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (9, 63, 54, 49, 3)
INSERT [dbo].[SG2_SocialProfile_BadProxy] ([BadProxyMappingId], [ProxyId], [SocialProfileId], [StatusId], [CityId]) VALUES (10, 33, 54, 49, 3)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_BadProxy] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_Notification] ON 

INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (15, N'Customer enter email verification code 123456', 51, 1, CAST(N'2019-10-05T08:18:14.490' AS DateTime), N'1', CAST(N'2019-10-05T08:18:14.490' AS DateTime), N'1', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (16, N'Customer successfull subscribe for plan LikeX500', 51, 54, CAST(N'2019-10-05T08:25:36.440' AS DateTime), N'54', CAST(N'2019-10-05T08:25:36.440' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (17, N'Customer update social profile.', 51, 54, CAST(N'2019-10-05T08:27:19.847' AS DateTime), N'54', CAST(N'2019-10-05T08:27:19.847' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (18, N'Customer MP Box is assigned -  SGA1-95.217.99.130', 51, 54, CAST(N'2019-10-05T08:27:19.860' AS DateTime), N'54', CAST(N'2019-10-05T08:27:19.860' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (19, N'Customer assigned IP : 287.16.120.233 Port : 3177', 51, 54, CAST(N'2019-10-05T08:27:20.160' AS DateTime), N'54', CAST(N'2019-10-05T08:27:20.160' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (20, N'Customer assigned IP : 162.211.64.56 Port : 3177', 51, 54, CAST(N'2019-10-05T08:28:14.807' AS DateTime), N'54', CAST(N'2019-10-05T08:28:14.807' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (21, N'Customer assigned IP : 162.211.64.56 Port : 3177', 51, 54, CAST(N'2019-10-05T08:28:55.480' AS DateTime), N'54', CAST(N'2019-10-05T08:28:55.480' AS DateTime), N'54', N'Auto')
INSERT [dbo].[SG2_SocialProfile_Notification] ([Id], [Notification], [StatusId], [SocialProfileId], [CreatedOn], [CreatedBy], [UpdateOn], [UpdatedBy], [Mode]) VALUES (22, N'The customer profile APIBlock.', 51, 54, CAST(N'2019-10-05T08:29:35.813' AS DateTime), N'54', CAST(N'2019-10-05T08:29:35.813' AS DateTime), N'54', N'Auto')
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_Notification] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ON 

INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (1, 0, 0, N'No Boost', N'No Boost', 28, N'plan_FgODkLxV4Iv7LI', 39, 3, 19, CAST(N'2019-08-24T15:45:50.107' AS DateTime), NULL, CAST(N'2019-08-24T15:45:50.107' AS DateTime), NULL, 1, 30, 1)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (2, 300, 19, N'LikeX300', N'LikeX300', 28, N'plan_FgOE1dPDRcUU6E', 58, 8, 19, CAST(N'2019-08-24T15:46:39.473' AS DateTime), NULL, CAST(N'2019-08-24T15:46:39.473' AS DateTime), NULL, 2, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (3, 500, 25, N'LikeX500', N'LikeX500', 28, N'plan_FgOE1RYjzLDTuM', 64, 8, 19, CAST(N'2019-08-24T15:47:05.890' AS DateTime), NULL, CAST(N'2019-08-24T15:47:05.890' AS DateTime), NULL, 3, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (4, 750, 29, N'LikeX750', N'LikeX500', 28, N'plan_FgOFfUHVgu5aw7', 68, 8, 19, CAST(N'2019-08-24T15:47:28.950' AS DateTime), NULL, CAST(N'2019-08-24T15:47:28.950' AS DateTime), NULL, 4, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (5, 1000, 35, N'LikeX1K', N'LikeX1K', 28, N'plan_FgOFBbulP2H7Et', 74, 7, 19, CAST(N'2019-08-24T15:47:49.847' AS DateTime), NULL, CAST(N'2019-08-24T15:47:49.847' AS DateTime), NULL, 5, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (6, 2000, 70, N'LikeX2K', N'LikeX2K', 28, N'plan_FgOFG90XfNLnEz', 109, 8, 19, CAST(N'2019-08-24T15:48:11.877' AS DateTime), NULL, CAST(N'2019-08-24T15:48:11.877' AS DateTime), NULL, 6, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (7, 3000, 99, N'LikeX3K', N'LikeX3K', 28, N'plan_FgOGjyzPJYlmIz', 138, 8, 20, CAST(N'2019-08-24T15:48:33.767' AS DateTime), NULL, CAST(N'2019-08-24T15:48:33.767' AS DateTime), NULL, 7, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (8, 5000, 160, N'LikeX5K', N'LikeX5K', 28, N'plan_FgOGoSWGfRZZ15', 199, 8, 20, CAST(N'2019-08-24T15:48:42.680' AS DateTime), NULL, CAST(N'2019-08-24T15:48:42.680' AS DateTime), NULL, 8, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (9, 10000, 320, N'LikeX10K', N'LikeX10K', 28, N'plan_FgOGSZWl25kDHw', 359, 8, 20, CAST(N'2019-08-24T15:48:50.510' AS DateTime), NULL, CAST(N'2019-08-24T15:48:50.510' AS DateTime), NULL, 9, 30, 0)
INSERT [dbo].[SG2_SocialProfile_PaymentPlan] ([PlanId], [NoOfLikes], [DisplayPrice], [PlanName], [PlanShortDescription], [PlanTypeId], [StripePlanId], [StripePlanPrice], [NoOfLikesDuration], [StatusId], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [SortOrder], [SocialPlanTypeId], [IsDefault]) VALUES (10, 0, 39, N'Social Growth E', N'Social Growth Engine', 29, N'plan_FgO7ZufIVGqxIg', 0, 2, 18, CAST(N'2019-08-24T15:41:37.060' AS DateTime), NULL, CAST(N'2019-08-24T15:41:37.060' AS DateTime), NULL, NULL, 30, 1)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_PaymentPlan] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ON 

INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (7, 10, 2)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (8, 62, 6)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (10, 41, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (11, 42, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (12, 43, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (13, 44, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (14, 50, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (15, 51, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (16, 52, 9)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (25, 15, 15)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (26, 16, 15)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (27, 17, 15)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (28, 18, 15)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (31, 10, 17)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (49, 21, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (50, 22, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (51, 23, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (52, 24, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (53, 25, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (54, 26, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (55, 27, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (56, 28, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (57, 29, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (58, 58, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (59, 59, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (60, 60, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (61, 61, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (62, 62, 20)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (123, 31, 28)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (124, 32, 28)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (165, 21, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (166, 22, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (167, 23, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (168, 24, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (169, 25, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (170, 26, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (171, 27, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (172, 28, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (173, 29, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (174, 58, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (175, 59, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (176, 60, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (177, 61, 32)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (178, 46, 33)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (179, 31, 34)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (180, 32, 34)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (181, 31, 36)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (182, 32, 36)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (183, 30, 37)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (184, 64, 37)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (185, 30, 38)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (186, 64, 38)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (187, 30, 39)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (188, 64, 39)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (189, 15, 40)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (190, 16, 40)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (191, 17, 40)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (192, 18, 40)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (193, 15, 41)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (194, 16, 41)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (195, 17, 41)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (196, 18, 41)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (197, 47, 42)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (198, 48, 42)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (199, 49, 42)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (200, 47, 43)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (201, 48, 43)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (202, 49, 43)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (203, 47, 44)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (204, 48, 44)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (205, 49, 44)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (206, 41, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (207, 42, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (208, 43, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (209, 44, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (210, 50, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (211, 51, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (212, 52, 45)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (213, 46, 46)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (214, 10, 47)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (215, 46, 49)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (216, 41, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (217, 42, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (218, 43, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (219, 44, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (220, 50, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (221, 51, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (222, 52, 50)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (223, 21, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (224, 22, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (225, 23, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (226, 24, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (227, 25, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (228, 26, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (229, 27, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (230, 28, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (231, 29, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (232, 58, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (233, 59, 29)
GO
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (234, 60, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (235, 61, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (236, 62, 29)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (243, 19, 52)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (244, 20, 52)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (245, 11, 1)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (246, 12, 1)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (247, 13, 1)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (248, 14, 1)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (249, 19, 53)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (250, 20, 53)
INSERT [dbo].[SG2_SocialProfile_ProxyMapping] ([ProxyMappingId], [ProxyId], [SocialProfileId]) VALUES (253, 57, 54)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_ProxyMapping] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_StatusHistory] ON 

INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (1, 1, 1, 42, CAST(N'2019-09-18T07:17:40.083' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (2, 1, 1, 11, CAST(N'2019-09-18T07:17:40.130' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (3, 1, 1, 50, CAST(N'2019-09-18T07:17:40.130' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (4, 1, 2, 42, CAST(N'2019-09-18T09:10:55.403' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (5, 1, 2, 11, CAST(N'2019-09-18T09:10:55.403' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (6, 1, 2, 50, CAST(N'2019-09-18T09:10:55.403' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (7, 2, 2, 42, CAST(N'2019-09-22T20:39:25.713' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (8, 2, 2, 50, CAST(N'2019-09-22T20:39:25.760' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (9, 2, 2, 11, CAST(N'2019-09-22T20:39:25.760' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (10, 2, 2, 50, CAST(N'2019-09-22T20:39:25.760' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (11, 2, 2, 42, CAST(N'2019-09-22T20:44:53.560' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (12, 2, 2, 11, CAST(N'2019-09-22T20:44:53.560' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (13, 2, 2, 50, CAST(N'2019-09-22T20:44:53.560' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (14, 2, 2, 50, CAST(N'2019-09-22T20:44:53.560' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (15, 2, 2, 42, CAST(N'2019-09-22T21:32:56.090' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (16, 2, 2, 11, CAST(N'2019-09-22T21:32:56.090' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (17, 6, 2, 42, CAST(N'2019-09-23T07:49:43.307' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (18, 6, 2, 11, CAST(N'2019-09-23T07:49:43.353' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (19, 6, 2, 17, CAST(N'2019-09-23T07:51:04.090' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (20, 6, 2, 31, CAST(N'2019-09-23T07:51:04.090' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (21, 6, 2, 36, CAST(N'2019-09-23T08:05:29.270' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (22, 6, 2, 36, CAST(N'2019-09-23T08:20:31.457' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (23, 6, 2, 36, CAST(N'2019-09-23T08:22:41.433' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (24, 1, 2, 42, CAST(N'2019-09-24T05:30:09.937' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (25, 1, 2, 11, CAST(N'2019-09-24T05:30:09.983' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (26, 1, 2, 42, CAST(N'2019-09-24T05:52:35.923' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (27, 1, 2, 11, CAST(N'2019-09-24T05:52:36.000' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (28, 1, 2, 36, CAST(N'2019-09-24T05:53:05.790' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (29, 1, 2, 42, CAST(N'2019-09-24T05:54:14.543' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (30, 1, 2, 11, CAST(N'2019-09-24T05:54:14.543' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (31, 1, 2, 36, CAST(N'2019-09-24T05:54:20.510' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (32, 1, 2, 36, CAST(N'2019-09-24T07:03:28.930' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (33, 1, 2, 50, CAST(N'2019-09-24T07:03:28.930' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (34, 2, 2, 50, CAST(N'2019-09-24T07:44:00.593' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (35, 9, 1, 42, CAST(N'2019-09-24T16:47:08.330' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (36, 9, 1, 11, CAST(N'2019-09-24T16:47:08.377' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (37, 9, 1, 11, CAST(N'2019-09-24T16:47:09.220' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (38, 9, 1, 42, CAST(N'2019-09-24T16:47:09.233' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (39, 9, 1, 11, CAST(N'2019-09-24T16:47:10.337' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (40, 9, 1, 42, CAST(N'2019-09-24T16:47:10.353' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (44, 15, 1, 42, CAST(N'2019-10-01T16:11:56.947' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (76, 28, 2, 42, CAST(N'2019-10-02T19:01:47.160' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (77, 28, 2, 11, CAST(N'2019-10-02T19:01:47.253' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (78, 28, 2, 17, CAST(N'2019-10-02T19:02:49.567' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (79, 28, 2, 31, CAST(N'2019-10-02T19:02:49.567' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (80, 28, 2, 32, CAST(N'2019-10-02T19:03:37.633' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (81, 29, 2, 42, CAST(N'2019-10-02T19:05:28.370' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (82, 29, 2, 11, CAST(N'2019-10-02T19:05:28.370' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (83, 29, 2, 32, CAST(N'2019-10-02T19:08:19.380' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (87, 32, 3, 42, CAST(N'2019-10-02T20:05:04.553' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (88, 32, 3, 11, CAST(N'2019-10-02T20:05:04.600' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (89, 32, 3, 32, CAST(N'2019-10-02T20:05:47.237' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (90, 2, 2, 42, CAST(N'2019-10-02T20:08:43.097' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (91, 2, 2, 11, CAST(N'2019-10-02T20:08:43.097' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (92, 33, 3, 42, CAST(N'2019-10-02T20:23:05.950' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (93, 33, 3, 11, CAST(N'2019-10-02T20:23:05.950' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (94, 34, 3, 42, CAST(N'2019-10-02T20:28:11.527' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (95, 34, 3, 11, CAST(N'2019-10-02T20:28:11.527' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (96, 36, 3, 42, CAST(N'2019-10-03T16:48:34.950' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (97, 36, 3, 42, CAST(N'2019-10-03T16:59:48.107' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (98, 36, 3, 11, CAST(N'2019-10-03T16:59:48.107' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (99, 36, 3, 36, CAST(N'2019-10-03T17:02:53.510' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (100, 36, 3, 36, CAST(N'2019-10-03T17:03:38.943' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (101, 36, 3, 36, CAST(N'2019-10-03T17:04:57.090' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (102, 36, 3, 36, CAST(N'2019-10-03T17:15:09.990' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (103, 37, 3, 42, CAST(N'2019-10-03T17:25:45.380' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (104, 37, 3, 11, CAST(N'2019-10-03T17:25:45.380' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (105, 37, 3, 36, CAST(N'2019-10-03T17:26:33.683' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (106, 39, 3, 42, CAST(N'2019-10-03T18:30:05.037' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (107, 39, 3, 42, CAST(N'2019-10-03T18:46:23.963' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (108, 39, 3, 42, CAST(N'2019-10-03T19:03:14.020' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (109, 39, 3, 11, CAST(N'2019-10-03T19:03:14.020' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (110, 39, 3, 17, CAST(N'2019-10-03T19:04:12.537' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (111, 39, 3, 31, CAST(N'2019-10-03T19:04:12.537' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (112, 39, 3, 17, CAST(N'2019-10-03T19:06:58.367' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (113, 39, 3, 31, CAST(N'2019-10-03T19:06:58.367' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (114, 41, 3, 42, CAST(N'2019-10-03T19:12:00.817' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (115, 41, 3, 42, CAST(N'2019-10-03T19:24:39.300' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (116, 39, 3, 36, CAST(N'2019-10-03T19:52:55.633' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (117, 41, 3, 42, CAST(N'2019-10-03T20:23:21.273' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (118, 41, 3, 11, CAST(N'2019-10-03T20:23:21.273' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (119, 42, 3, 42, CAST(N'2019-10-03T20:40:03.813' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (120, 42, 3, 11, CAST(N'2019-10-03T20:40:03.813' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (121, 42, 3, 36, CAST(N'2019-10-03T20:41:01.983' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (122, 43, 3, 42, CAST(N'2019-10-03T20:47:39.347' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (123, 43, 3, 11, CAST(N'2019-10-03T20:47:39.347' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (124, 43, 3, 36, CAST(N'2019-10-03T20:53:33.687' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (125, 44, 3, 42, CAST(N'2019-10-03T20:58:35.673' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (126, 44, 3, 11, CAST(N'2019-10-03T20:58:35.690' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (127, 45, 3, 42, CAST(N'2019-10-03T21:05:42.250' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (128, 45, 3, 11, CAST(N'2019-10-03T21:05:42.250' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (129, 45, 3, 36, CAST(N'2019-10-03T21:07:10.583' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (130, 46, 3, 42, CAST(N'2019-10-04T10:21:33.290' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (131, 46, 3, 11, CAST(N'2019-10-04T10:21:33.290' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (132, 47, 3, 42, CAST(N'2019-10-04T12:14:04.433' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (133, 47, 3, 42, CAST(N'2019-10-04T12:42:15.567' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (134, 50, 3, 42, CAST(N'2019-10-04T19:13:50.220' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (135, 50, 3, 42, CAST(N'2019-10-04T19:28:21.060' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (136, 50, 3, 42, CAST(N'2019-10-04T19:30:44.253' AS DateTime), NULL)
GO
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (137, 50, 3, 42, CAST(N'2019-10-04T19:35:50.133' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (138, 50, 3, 42, CAST(N'2019-10-04T19:39:06.833' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (139, 50, 3, 42, CAST(N'2019-10-04T19:43:12.427' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (140, 50, 3, 11, CAST(N'2019-10-04T19:43:12.427' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (141, 29, 2, 42, CAST(N'2019-10-04T22:34:21.360' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (143, 1, 2, 42, CAST(N'2019-10-04T22:36:12.887' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (144, 52, 2, 42, CAST(N'2019-10-04T23:33:27.723' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (145, 1, 3, 42, CAST(N'2019-10-04T23:34:39.423' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (146, 53, 2, 42, CAST(N'2019-10-05T08:07:20.733' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (147, 54, 2, 42, CAST(N'2019-10-05T08:29:35.893' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (148, 54, 2, 50, CAST(N'2019-10-05T08:29:35.893' AS DateTime), NULL)
INSERT [dbo].[SG2_SocialProfile_StatusHistory] ([SPSHId], [SocialProfileId], [JVBoxId], [JVBoxStatusId], [CreatedDate], [CreatedBy]) VALUES (149, 54, 2, 50, CAST(N'2019-10-05T08:29:35.893' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_StatusHistory] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_Subscription] ON 

INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (1, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-09-18T07:13:49.000' AS DateTime), CAST(N'2019-10-18T07:13:49.000' AS DateTime), 1, N'sub_Fpcbsor6TcqztM', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FJxMHEStpmImzZCuWoC4Phk')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (2, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-09-18T09:46:24.000' AS DateTime), CAST(N'2019-10-18T09:46:24.000' AS DateTime), 2, N'sub_Fpf4nkZ0wafX0T', 27, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FJzjwEStpmImzZCFMcSD2ni')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (3, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-09-22T21:31:58.000' AS DateTime), CAST(N'2019-10-22T21:31:58.000' AS DateTime), 2, N'sub_Fpf4nkZ0wafX0T', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FLcewEStpmImzZClEBmXbfQ')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (4, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-09-23T07:37:28.000' AS DateTime), CAST(N'2019-10-23T07:37:28.000' AS DateTime), 6, N'sub_FrV7edNsjiMJMr', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FLm6uEStpmImzZC8j5HZuXR')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (5, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-09-24T09:02:31.000' AS DateTime), CAST(N'2019-10-24T09:02:31.000' AS DateTime), 9, N'sub_FrtidrTy4DeLkA', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FM9ulEStpmImzZC4cWCp57i')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (9, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-09-28T07:18:11.000' AS DateTime), CAST(N'2019-10-28T07:18:11.000' AS DateTime), 12, N'sub_FtMvktphskiNao', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FNaBzEStpmImzZCm0cIi1RU')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (12, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-09-30T20:44:30.000' AS DateTime), CAST(N'2019-10-30T20:44:30.000' AS DateTime), 15, N'sub_FuKOCNmX5oX1Pd', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FOVjOEStpmImzZC40vG3PU3')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (14, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-10-01T07:49:40.000' AS DateTime), CAST(N'2019-11-01T07:49:40.000' AS DateTime), 17, N'sub_FuV7mQXbAYmlJh', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FOg76EStpmImzZCFftTOrxz')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (17, N'LikeX750', N'LikeX750', N'month', CAST(6800.00 AS Decimal(18, 2)), CAST(N'2019-10-01T17:55:24.000' AS DateTime), CAST(N'2019-11-01T17:55:24.000' AS DateTime), 20, N'sub_FuettB90SamnlC', 25, N'plan_FgOFfUHVgu5aw7', 4, N'in_1FOpZIEStpmImzZCVqmdGfmW')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (25, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-02T18:59:55.000' AS DateTime), CAST(N'2019-11-02T18:59:55.000' AS DateTime), 28, N'sub_Fv39LsvZ3oSBU6', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPD3HEStpmImzZC2wGKbSOS')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (26, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-02T19:01:39.000' AS DateTime), CAST(N'2019-11-02T19:01:39.000' AS DateTime), 29, N'sub_Fv3BKTACJ2svrA', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPD4yEStpmImzZCnSlt6vfn')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (29, N'LikeX750', N'LikeX750', N'month', CAST(6800.00 AS Decimal(18, 2)), CAST(N'2019-10-02T20:01:52.000' AS DateTime), CAST(N'2019-11-02T20:01:52.000' AS DateTime), 32, N'sub_Fv493LWSTOCED8', 25, N'plan_FgOFfUHVgu5aw7', 4, N'in_1FPE1EEStpmImzZCavC9uUp5')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (30, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-10-02T20:20:39.000' AS DateTime), CAST(N'2019-11-02T20:20:39.000' AS DateTime), 33, N'sub_Fv4SLwR5zkYMBB', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FPEJPEStpmImzZCDg7BP13V')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (31, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-10-02T20:26:39.000' AS DateTime), CAST(N'2019-11-02T20:26:39.000' AS DateTime), 34, N'sub_Fv4YX9H9OS4ukd', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FPEPDEStpmImzZCtefoA5Oq')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (33, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T16:46:17.000' AS DateTime), CAST(N'2019-11-03T16:46:17.000' AS DateTime), 36, N'sub_FvODQxxjqZSFAr', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPXRVEStpmImzZCxfh4CovH')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (34, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T17:23:59.000' AS DateTime), CAST(N'2019-11-03T17:23:59.000' AS DateTime), 37, N'sub_FvOpdDWQYTqo2U', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPY1zEStpmImzZC7rPtmJtG')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (35, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T17:57:30.000' AS DateTime), CAST(N'2019-11-03T17:57:30.000' AS DateTime), 38, N'sub_FvPNL5uVduOBST', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPYYREStpmImzZCsngN3d1P')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (36, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T18:29:28.000' AS DateTime), CAST(N'2019-11-03T18:29:28.000' AS DateTime), 39, N'sub_FvPtJCrpNyhmBs', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPZ3MEStpmImzZC4svOVM63')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (37, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T18:33:23.000' AS DateTime), CAST(N'2019-11-03T18:33:23.000' AS DateTime), 40, N'sub_FvPxGm98dVS3yi', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPZ79EStpmImzZC3P4PMDBc')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (38, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T19:11:22.000' AS DateTime), CAST(N'2019-11-03T19:11:22.000' AS DateTime), 41, N'sub_FvQZplNS0ndx5Z', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPZhvEStpmImzZCC2CryrRE')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (39, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T20:37:48.000' AS DateTime), CAST(N'2019-11-03T20:37:48.000' AS DateTime), 42, N'sub_FvRxuKkHCBBImp', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPb3ZEStpmImzZCiBCrRwVs')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (40, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-10-03T20:46:25.000' AS DateTime), CAST(N'2019-11-03T20:46:25.000' AS DateTime), 43, N'sub_FvS6jOjk1PjMPw', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FPbBtEStpmImzZCPXPialWZ')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (41, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-03T20:57:17.000' AS DateTime), CAST(N'2019-11-03T20:57:17.000' AS DateTime), 44, N'sub_FvSGj97X2VpYEf', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPbMPEStpmImzZCsuDtltXQ')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (42, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-10-03T21:04:25.000' AS DateTime), CAST(N'2019-11-03T21:04:25.000' AS DateTime), 45, N'sub_FvSOUabbyBjBrz', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FPbTJEStpmImzZCMioQXyud')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (43, N'LikeX300', N'LikeX300', N'month', CAST(5800.00 AS Decimal(18, 2)), CAST(N'2019-10-04T10:20:01.000' AS DateTime), CAST(N'2019-11-04T10:20:01.000' AS DateTime), 46, N'sub_FvfD7Zan0Tl4AM', 25, N'plan_FgOE1dPDRcUU6E', 2, N'in_1FPntGEStpmImzZCaOOND7tx')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (44, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-10-04T12:12:49.000' AS DateTime), CAST(N'2019-11-04T12:12:49.000' AS DateTime), 47, N'sub_Fvh27L5qrez2Yn', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FPpePEStpmImzZCGKFBv7uG')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (45, N'No Boost', N'No Boost', N'month', CAST(3900.00 AS Decimal(18, 2)), CAST(N'2019-10-04T13:03:24.000' AS DateTime), CAST(N'2019-11-04T13:03:24.000' AS DateTime), 49, N'sub_Fvhrceln3gvcE6', 25, N'plan_FgODkLxV4Iv7LI', 1, N'in_1FPqRMEStpmImzZCsPhxQT68')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (46, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-04T19:12:58.000' AS DateTime), CAST(N'2019-11-04T19:12:58.000' AS DateTime), 50, N'sub_FvnohMsBuKjC8Z', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPwD1EStpmImzZCmV8WF8Tm')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (48, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-04T23:07:07.000' AS DateTime), CAST(N'2019-11-04T23:07:07.000' AS DateTime), 52, N'sub_FvraAfccjX4dRu', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FPzrbEStpmImzZCRuhHBmWk')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (49, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-05T08:00:28.000' AS DateTime), CAST(N'2019-11-05T08:00:28.000' AS DateTime), 53, N'sub_Fw0C2ATYCxaF9c', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FQ8BkEStpmImzZC0wF2qmVW')
INSERT [dbo].[SG2_SocialProfile_Subscription] ([SubscriptionId], [Name], [Description], [SubscriptionType], [Price], [StartDate], [EndDate], [SocialProfileId], [StripeSubscriptionId], [StatusId], [StripePlanId], [PaymentPlanId], [StripeInvoiceId]) VALUES (50, N'LikeX500', N'LikeX500', N'month', CAST(6400.00 AS Decimal(18, 2)), CAST(N'2019-10-05T08:25:32.000' AS DateTime), CAST(N'2019-11-05T08:25:32.000' AS DateTime), 54, N'sub_Fw0bjRGkOcSd14', 25, N'plan_FgOE1RYjzLDTuM', 3, N'in_1FQ8a0EStpmImzZCntrch9f1')
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_Subscription] OFF
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ON 

INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (1, 1, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-09-18T07:12:03.397' AS DateTime), N'hassanjamil.bwp@gmail.com', CAST(N'2019-09-18T09:17:03.473' AS DateTime), N'exphjm', 50, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (2, 2, N'lucas', N'Snake,#lucas', N'New York,London', N'Moon', 1, 2, 2, N'Fitness', N'@lux.1hunnid, @peanutlive215,the.shark.puppet', 1, CAST(N'2019-09-18T09:40:32.360' AS DateTime), N'lucas.eyre@gmail.com', CAST(N'2019-10-02T20:05:46.227' AS DateTime), N'lucas.eyre@gmail.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (4, 4, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-09-19T18:24:28.593' AS DateTime), N'anthony@einfinity.email', CAST(N'2019-09-19T18:24:28.593' AS DateTime), N'anthony@einfinity.email', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (6, 6, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'hassanjamil', 1, CAST(N'2019-09-23T07:34:21.757' AS DateTime), N'abdullahbcy@gmail.com', CAST(N'2019-09-23T07:36:17.460' AS DateTime), N'abdullahbcy@gmail.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (7, 7, N'Atlantarealestate,#buckheadrealtor,buckhead,atlanta,buckheadrealestate ,Atlantahomes,Atlantaluxuryhomes,buckheadhomes', N'Snake', N'Atlanta ,tuxedopark,buckhead,brookhaven ', N'Moon', 1, 3, 1, N'Fitness', N'Bonneau Ansley ,Nicholas brown,Erin Yarbroudy ,Shanna Bradley ,Glennis Beacham ,Chase Mizell ,Becky Morris ,Travis Reed,Jere metcalf,price curtis,katie Brannon mcGuirk,Jim Getzinger ', 1, CAST(N'2019-09-23T21:58:53.030' AS DateTime), N'Sam@atlantafinehomes.com', CAST(N'2019-09-29T00:46:46.440' AS DateTime), N'Sam@atlantafinehomes.com', 43, NULL, NULL, 1)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (8, 8, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-09-24T06:42:19.307' AS DateTime), N'omar.c@me.com', CAST(N'2019-09-24T06:42:19.307' AS DateTime), N'omar.c@me.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (9, 9, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'@lucas_eyre,lucas_eyre', 1, CAST(N'2019-09-24T09:01:14.707' AS DateTime), N'hello@thebaysocial.com', CAST(N'2019-09-24T09:02:12.380' AS DateTime), N'hello@thebaysocial.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (12, 12, N'Dogs,Cats,pakistan', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', NULL, 1, CAST(N'2019-09-28T07:15:53.123' AS DateTime), N'w.sardar000@gmial.com', CAST(N'2019-09-28T07:17:30.923' AS DateTime), N'w.sardar000@gmial.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (15, 15, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'Abc', 1, CAST(N'2019-09-30T20:43:08.797' AS DateTime), N'Kashif1235mkh@yahoo.com', CAST(N'2019-09-30T20:44:12.770' AS DateTime), N'Kashif1235mkh@yahoo.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (17, 17, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 2, N'Fitness', N'@lucas_eyre,@dolly', 1, CAST(N'2019-10-01T07:45:34.333' AS DateTime), N'sga2acc@gmail.com', CAST(N'2019-10-01T07:46:20.873' AS DateTime), N'sga2acc@gmail.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (20, 20, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'Test,ASDF', 1, CAST(N'2019-10-01T17:53:07.317' AS DateTime), N'Kashifmkh1235@yahoo.com', CAST(N'2019-10-01T18:05:13.710' AS DateTime), N'Kashifmkh1235@yahoo.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (28, 28, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-02T18:59:26.040' AS DateTime), N'sraza@crovtech.com', CAST(N'2019-10-02T18:59:26.040' AS DateTime), N'sraza@crovtech.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (29, 29, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-02T19:00:28.270' AS DateTime), N'Kashifmkh@yahoo.com', CAST(N'2019-10-02T19:00:28.270' AS DateTime), N'Kashifmkh@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (32, 32, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-02T20:01:30.307' AS DateTime), N'hjamil@crovtech.com', CAST(N'2019-10-02T20:01:30.307' AS DateTime), N'hjamil@crovtech.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (33, 33, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-02T20:19:35.257' AS DateTime), N'lucas.eyre123@gmail.com', CAST(N'2019-10-02T20:19:35.257' AS DateTime), N'lucas.eyre123@gmail.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (34, 34, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-02T20:26:04.637' AS DateTime), N'lucas.eyre321@gmail.com', CAST(N'2019-10-02T20:26:04.637' AS DateTime), N'lucas.eyre321@gmail.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (36, 36, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T16:45:34.840' AS DateTime), N'VanessaBall791pE48@yahoo.com', CAST(N'2019-10-03T16:45:34.840' AS DateTime), N'VanessaBall791pE48@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (37, 37, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T17:20:22.047' AS DateTime), N'AngelaScott366gbxO@yahoo.com', CAST(N'2019-10-03T17:20:22.047' AS DateTime), N'AngelaScott366gbxO@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (38, 38, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T17:53:14.177' AS DateTime), N'ElizabethChapman5331b2B@yahoo.com', CAST(N'2019-10-03T17:53:14.177' AS DateTime), N'ElizabethChapman5331b2B@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (39, 39, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T18:28:31.190' AS DateTime), N'JuliaBlake189e3Yw@yahoo.com', CAST(N'2019-10-03T18:28:31.190' AS DateTime), N'JuliaBlake189e3Yw@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (40, 40, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T18:32:15.830' AS DateTime), N'sraza1@crovtech.com', CAST(N'2019-10-03T18:32:15.830' AS DateTime), N'sraza1@crovtech.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (41, 41, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T19:10:41.313' AS DateTime), N'SoniaLangdon414Lufk@yahoo.com', CAST(N'2019-10-03T19:10:41.313' AS DateTime), N'SoniaLangdon414Lufk@yahoo.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (42, 42, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T20:37:08.203' AS DateTime), N'bernicec9r4@gmx.com', CAST(N'2019-10-03T20:37:08.203' AS DateTime), N'bernicec9r4@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (43, 43, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T20:45:29.293' AS DateTime), N'dalilau0peeks@gmx.com', CAST(N'2019-10-03T20:45:29.293' AS DateTime), N'dalilau0peeks@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (44, 44, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T20:56:45.363' AS DateTime), N'shakitab37s@gmx.com', CAST(N'2019-10-03T20:56:45.363' AS DateTime), N'shakitab37s@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (45, 45, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-03T21:03:40.030' AS DateTime), N'alfredacebotv2s@gmx.com', CAST(N'2019-10-03T21:03:40.030' AS DateTime), N'alfredacebotv2s@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (46, 46, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-04T10:18:19.683' AS DateTime), N'delmybiparuta@gmx.com', CAST(N'2019-10-04T10:18:19.683' AS DateTime), N'delmybiparuta@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (47, 47, N'Dogs,Cats,Leopards', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'@levis', 1, CAST(N'2019-10-04T12:11:29.680' AS DateTime), N'lyndaacuterra@gmx.com', CAST(N'2019-10-04T12:22:57.923' AS DateTime), N'lyndaacuterra@gmx.com', 43, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (48, 48, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-04T12:59:48.260' AS DateTime), N'domenicrwc@gmail.com', CAST(N'2019-10-04T12:59:48.260' AS DateTime), N'domenicrwc@gmail.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (49, 49, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-04T13:01:49.923' AS DateTime), N'francejqs@gmx.com', CAST(N'2019-10-04T13:01:49.923' AS DateTime), N'francejqs@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (50, 50, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-04T19:11:22.573' AS DateTime), N'deangelo692cka@gmx.com', CAST(N'2019-10-04T19:11:22.573' AS DateTime), N'deangelo692cka@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (52, 52, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-04T23:06:33.577' AS DateTime), N'rue4jm5parez@gmx.com', CAST(N'2019-10-04T23:06:33.577' AS DateTime), N'rue4jm5parez@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (53, 53, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-05T07:59:42.447' AS DateTime), N'cammiesilker1j@gmx.com', CAST(N'2019-10-05T07:59:42.447' AS DateTime), N'cammiesilker1j@gmx.com', NULL, NULL, NULL, 2)
INSERT [dbo].[SG2_SocialProfile_TargetingInformation] ([TargetingInformationId], [SocialProfileId], [Preference1], [Preference2], [Preference3], [Preference4], [Preference5], [Preference6], [Preference7], [Preference8], [Preference9], [Preference10], [CreatedOn], [CreatedBy], [UpdatedOn], [UpdatedBy], [QueueStatus], [JVNoOfLikes], [JVLikeyStatus], [SocialAccAs]) VALUES (54, 54, N'Dogs,Cats', N'Snake', N'New York,London', N'Moon', 1, 3, 1, N'Fitness', N'', 1, CAST(N'2019-10-05T08:23:23.600' AS DateTime), N'rebeccannemarqzg@gmx.com', CAST(N'2019-10-05T08:23:23.600' AS DateTime), N'rebeccannemarqzg@gmx.com', NULL, NULL, NULL, 2)
SET IDENTITY_INSERT [dbo].[SG2_SocialProfile_TargetingInformation] OFF
SET IDENTITY_INSERT [dbo].[SG2_SystemCity] ON 

INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (1, 1, 1, N'Los Angeles', N'LA', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (2, 1, 1, N'New York', N'NY', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (3, 2, 1, N'London', N'LDN', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (4, 2, 1, N'Bradford', N'BA', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (5, 3, 1, N'Sydney', N'NSW', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (6, 3, 1, N'Melbourne', N'MEL', 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (19, 28, 1, N'Singapore', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (20, 1, 1, N'Atlanta', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (21, 30, 1, N'Vancouver', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (22, 3, NULL, N'Brisbane', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (23, 1, NULL, N'Washinghton DC', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (24, 1, NULL, N'Chicago', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (25, 30, NULL, N'toronto', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (30, 1, NULL, N'Miami ', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (38, 1, NULL, N'Phoenix', NULL, 1)
INSERT [dbo].[SG2_SystemCity] ([CityId], [CountryId], [StateId], [Name], [Code], [StatusId]) VALUES (39, 1, NULL, N'Denver', NULL, 1)
SET IDENTITY_INSERT [dbo].[SG2_SystemCity] OFF
SET IDENTITY_INSERT [dbo].[SG2_SystemConfig] ON 

INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Stripe', N'sk_test_XW31sZXCy4MwTGedBiAMGNCy', N'pk_test_7Oa7ejWDiOrrR32QGaFv1Op8', CAST(N'2019-05-10T09:55:36.253' AS DateTime), N'admin', CAST(N'2019-08-24T15:44:55.780' AS DateTime), N'test')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'Klaviyo', N'pk_ef1aec522f99ce4a6c6531bcfb8b97a998', N'pk_ef1aec522f99ce4a6c6531bcfb8b97a998', CAST(N'2019-05-10T09:56:29.620' AS DateTime), N'admin', CAST(N'2019-07-28T20:04:53.803' AS DateTime), N'test')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'StripeInstagramProductId', N'prod_FgNfut8Tkvm1vZ', N'prod_FgNfut8Tkvm1vZ', CAST(N'2019-06-20T02:01:20.360' AS DateTime), N'System', CAST(N'2019-08-24T15:44:10.097' AS DateTime), N'test')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'Klavio_UnsubscribeList', N'LfMdF5', N'LfMdF5', CAST(N'2019-07-28T08:10:47.560' AS DateTime), N'Admin', CAST(N'2019-07-28T08:10:47.560' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (5, N'Klavio_PayingSubscribeList', N'MeghM4', N'MeghM4', CAST(N'2019-07-28T08:11:34.753' AS DateTime), N'Admin', CAST(N'2019-07-28T08:11:34.753' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (6, N'Klavio_NonPayingSubscribeList', N'Kw7EB4', N'Kw7EB4', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (7, N'Klavio_TeamMembersList', N'Jzbuur', N'Jzbuur', CAST(N'2019-07-28T08:34:13.830' AS DateTime), N'Admin', CAST(N'2019-07-28T08:34:13.830' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (13, N'Intercom', N'ibbud29z', N'ibbud29z', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemConfig] ([ConfigId], [ConfigKey], [ConfigValue], [ConfigValue2], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (14, N'GoogleMapApiKey', N'AIzaSyBWgJK_Zq8MF1vlGSmIslw1ACo-TAG7VH0', N'AIzaSyBWgJK_Zq8MF1vlGSmIslw1ACo-TAG7VH0', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin', CAST(N'2019-07-28T08:12:33.357' AS DateTime), N'Admin')
SET IDENTITY_INSERT [dbo].[SG2_SystemConfig] OFF
SET IDENTITY_INSERT [dbo].[SG2_SystemCountry] ON 

INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (1, N'United States', N'USA', N'+1', 1)
INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (2, N'United Kingdom', N'UK', N'+44', 1)
INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (3, N'Australia', N'AUS', N'+61', 1)
INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (28, N'Singapore', N'SNP', N'+34', 1)
INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (30, N'Canada', N'CND', N'+94', 1)
INSERT [dbo].[SG2_SystemCountry] ([CountryId], [Name], [Code], [PhoneCode], [StatusId]) VALUES (31, N'USA', NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SG2_SystemCountry] OFF
SET IDENTITY_INSERT [dbo].[SG2_SystemRole] ON 

INSERT [dbo].[SG2_SystemRole] ([RoleId], [Name], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (1, N'Super Admin', 19, CAST(N'2019-05-12T08:09:12.413' AS DateTime), N'Admin', CAST(N'2019-05-12T08:09:12.413' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemRole] ([RoleId], [Name], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (2, N'Manager', 19, CAST(N'2019-05-12T08:09:12.413' AS DateTime), N'Admin', CAST(N'2019-05-12T08:09:12.413' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemRole] ([RoleId], [Name], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (3, N'Sales', 19, CAST(N'2019-05-12T08:09:12.430' AS DateTime), N'Admin', CAST(N'2019-05-12T08:09:12.430' AS DateTime), N'Admin')
INSERT [dbo].[SG2_SystemRole] ([RoleId], [Name], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy]) VALUES (4, N'Operations', 19, CAST(N'2019-05-12T08:09:12.430' AS DateTime), N'Admin', CAST(N'2019-05-12T08:09:12.430' AS DateTime), N'Admin')
SET IDENTITY_INSERT [dbo].[SG2_SystemRole] OFF
SET IDENTITY_INSERT [dbo].[SG2_SystemUser] ON 

INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (1, N'MR', N'Omar', N'Chaudhry', N'omar.c@me.com', 1, N'123', 22, CAST(N'2019-05-12T08:10:31.393' AS DateTime), N'Admin', CAST(N'2019-07-06T20:22:59.737' AS DateTime), N'OmarOmar', 1)
INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (2, N'MR', N'John', N'Knight', N'jknight@socialgrowth.com', 2, N'password', 22, CAST(N'2019-05-15T11:17:52.420' AS DateTime), N'OmarChaudhry', CAST(N'2019-05-15T11:19:41.387' AS DateTime), N'OmarChaudhry', 0)
INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (3, N'MS', N'Alistiar', N'A', N'Alistiar@gmail.com', 4, N'123', 22, CAST(N'2019-06-16T14:01:58.417' AS DateTime), N'OmarChaudhry', CAST(N'2019-10-01T16:25:18.810' AS DateTime), N'OmarChaudhry', 0)
INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (4, N'MR', N'kashif', N'A', N'abc@yahoo.com', 2, N'K@sh1fmkh', 22, CAST(N'2019-06-22T15:34:56.177' AS DateTime), N'OmarChaudhry', CAST(N'2019-10-01T16:17:46.733' AS DateTime), N'OmarChaudhry', 0)
INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (5, N'MR', N'Jarvee', N'J', N'jarvee@thebaysocial.com', 4, N'k4fpFgf*D!wa', 22, CAST(N'2019-08-31T10:54:16.130' AS DateTime), N'OmarChaudhry', CAST(N'2019-08-31T11:19:14.103' AS DateTime), N'OmarChaudhry', 0)
INSERT [dbo].[SG2_SystemUser] ([SystemUserId], [Title], [FirstName], [LastName], [Email], [SystemRoleId], [Password], [StatusId], [CreatedOn], [CreatedBy], [ModifiedOn], [ModifiedBy], [HostUser]) VALUES (6, N'MR', N'Hassan', N'Jamil', N'hassanjamil.bwp@gmail.com', 3, N'$tudent321', 22, CAST(N'2019-08-31T11:06:26.677' AS DateTime), N'OmarChaudhry', CAST(N'2019-08-31T11:23:35.097' AS DateTime), N'OmarChaudhry', 0)
SET IDENTITY_INSERT [dbo].[SG2_SystemUser] OFF
SET IDENTITY_INSERT [dbo].[SG2_VPSSupplier] ON 

INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (5, N'Host Us', NULL, N'www.hostus.us', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (7, N'vps.Net', NULL, N'vps.net', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (8, N'BinaryLane.com.au', NULL, N'BinaryLane.com.au', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (9, N'Host.us', NULL, N'Host.us', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (10, N'hosthatch.com', NULL, N'hosthatch.com', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (11, N'my.iniz.com', NULL, N'my.iniz.com', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (12, N'securedragon.net', NULL, N'securedragon.net', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (13, N'SPLSD PS', NULL, N'www.hostus.us', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
INSERT [dbo].[SG2_VPSSupplier] ([VPSSId], [IssuingISPName], [IssuingISPPhone], [IssuingISPWebsite], [IssuingISPAccount], [IssuingISPPassword], [IssuingISPMemo], [StatusId], [IPManageBy], [SupportPhone], [SupportEmail]) VALUES (14, N'SPLSD4 NS', NULL, N'www.hostus.us', NULL, N'69np2nxxm7jb', NULL, 19, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SG2_VPSSupplier] OFF
ALTER TABLE [dbo].[SG2_EnumerationValue] ADD  CONSTRAINT [DF__SG2_Enume__IsVis__71D1E811]  DEFAULT ((1)) FOR [IsVisible]
GO
ALTER TABLE [dbo].[SG2_QueueAuditEnumerationValue] ADD  CONSTRAINT [DF__SG2_QueueAuditEnume__IsVis__71D1E811]  DEFAULT ((1)) FOR [IsVisible]
GO
ALTER TABLE [dbo].[SG2_SocialProfile] ADD  CONSTRAINT [DF_IsArchived]  DEFAULT ((0)) FOR [IsArchived]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Notification] ADD  DEFAULT ('Auto') FOR [Mode]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_FollowersGain]  DEFAULT ((0)) FOR [FollowersGain]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Followers]  DEFAULT ((0)) FOR [Followers]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Followings]  DEFAULT ((0)) FOR [Followings]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_SG2_SocialProfile_Statistics_Followings1]  DEFAULT ((0)) FOR [FollowingsRatio]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Joiner]  DEFAULT ((0)) FOR [Joiner]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Unjoiner]  DEFAULT ((0)) FOR [Unjoiner]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Follow]  DEFAULT ((0)) FOR [Follow]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Unfollow]  DEFAULT ((0)) FOR [Unfollow]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_ContactMassage]  DEFAULT ((0)) FOR [ContactMassage]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_ContactFriends]  DEFAULT ((0)) FOR [ContactFriends]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_RE(Pin/Tweet/Blog]  DEFAULT ((0)) FOR [REPinTweetBlog]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Bump]  DEFAULT ((0)) FOR [Bump]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Like]  DEFAULT ((0)) FOR [Like]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Comment]  DEFAULT ((0)) FOR [Comment]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Engagement]  DEFAULT ((0)) FOR [Engagement]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_Repost]  DEFAULT ((0)) FOR [Repost]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_LikeComments]  DEFAULT ((0)) FOR [LikeComments]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_StoryViewer]  DEFAULT ((0)) FOR [StoryViewer]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] ADD  CONSTRAINT [DF_CustomerStatistics_BlockedFollowers]  DEFAULT ((0)) FOR [BlockedFollowers]
GO
ALTER TABLE [dbo].[SG2_Customer_ContactDetail]  WITH CHECK ADD  CONSTRAINT [FK_SG2_ContactDetails_SG2_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[SG2_Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[SG2_Customer_ContactDetail] CHECK CONSTRAINT [FK_SG2_ContactDetails_SG2_Customer]
GO
ALTER TABLE [dbo].[SG2_EnumerationValue]  WITH CHECK ADD  CONSTRAINT [FK_SG2_EnumerationValue_SG2_Enumeration] FOREIGN KEY([EnumerationId])
REFERENCES [dbo].[SG2_Enumeration] ([EnumerationId])
GO
ALTER TABLE [dbo].[SG2_EnumerationValue] CHECK CONSTRAINT [FK_SG2_EnumerationValue_SG2_Enumeration]
GO
ALTER TABLE [dbo].[SG2_Proxy]  WITH CHECK ADD  CONSTRAINT [FK_SG2_Proxy_SG2_VPSSupplier] FOREIGN KEY([VPSSId])
REFERENCES [dbo].[SG2_VPSSupplier] ([VPSSId])
GO
ALTER TABLE [dbo].[SG2_Proxy] CHECK CONSTRAINT [FK_SG2_Proxy_SG2_VPSSupplier]
GO
ALTER TABLE [dbo].[SG2_QueueAuditEnumerationValue]  WITH CHECK ADD  CONSTRAINT [FK_SG2_QueueAuditEnumerationValue_SG2_QueueAuditEnumeration] FOREIGN KEY([EnumerationId])
REFERENCES [dbo].[SG2_QueueAuditEnumeration] ([EnumerationId])
GO
ALTER TABLE [dbo].[SG2_QueueAuditEnumerationValue] CHECK CONSTRAINT [FK_SG2_QueueAuditEnumerationValue_SG2_QueueAuditEnumeration]
GO
ALTER TABLE [dbo].[SG2_SocialProfile]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_SG2_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[SG2_Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile] CHECK CONSTRAINT [FK_SG2_SocialProfile_SG2_Customer]
GO
ALTER TABLE [dbo].[SG2_SocialProfile]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_SG2_JVBox] FOREIGN KEY([JVBoxId])
REFERENCES [dbo].[SG2_JVBox] ([JVBoxId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile] CHECK CONSTRAINT [FK_SG2_SocialProfile_SG2_JVBox]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_ProxyMapping]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_ProxyMapping_SG2_Proxy] FOREIGN KEY([ProxyId])
REFERENCES [dbo].[SG2_Proxy] ([ProxyId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_ProxyMapping] CHECK CONSTRAINT [FK_SG2_SocialProfile_ProxyMapping_SG2_Proxy]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_ProxyMapping]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_ProxyMapping_SG2_SocialProfile] FOREIGN KEY([SocialProfileId])
REFERENCES [dbo].[SG2_SocialProfile] ([SocialProfileId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_ProxyMapping] CHECK CONSTRAINT [FK_SG2_SocialProfile_ProxyMapping_SG2_SocialProfile]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_Statistics_SG2_SocialProfile] FOREIGN KEY([SocialProfileId])
REFERENCES [dbo].[SG2_SocialProfile] ([SocialProfileId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Statistics] CHECK CONSTRAINT [FK_SG2_SocialProfile_Statistics_SG2_SocialProfile]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_StatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_StatusHistory_SG2_SocialProfile] FOREIGN KEY([SocialProfileId])
REFERENCES [dbo].[SG2_SocialProfile] ([SocialProfileId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_StatusHistory] CHECK CONSTRAINT [FK_SG2_SocialProfile_StatusHistory_SG2_SocialProfile]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Subscription]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_Subscription_SG2_SocialProfile] FOREIGN KEY([SocialProfileId])
REFERENCES [dbo].[SG2_SocialProfile] ([SocialProfileId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_Subscription] CHECK CONSTRAINT [FK_SG2_SocialProfile_Subscription_SG2_SocialProfile]
GO
ALTER TABLE [dbo].[SG2_SocialProfile_TargetingInformation]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SocialProfile_TargetingInformation_SG2_SocialProfile] FOREIGN KEY([SocialProfileId])
REFERENCES [dbo].[SG2_SocialProfile] ([SocialProfileId])
GO
ALTER TABLE [dbo].[SG2_SocialProfile_TargetingInformation] CHECK CONSTRAINT [FK_SG2_SocialProfile_TargetingInformation_SG2_SocialProfile]
GO
ALTER TABLE [dbo].[SG2_SystemUser]  WITH CHECK ADD  CONSTRAINT [FK_SG2_SystemUsers_SG2_SystemRoles] FOREIGN KEY([SystemRoleId])
REFERENCES [dbo].[SG2_SystemRole] ([RoleId])
GO
ALTER TABLE [dbo].[SG2_SystemUser] CHECK CONSTRAINT [FK_SG2_SystemUsers_SG2_SystemRoles]
GO
/****** Object:  StoredProcedure [dbo].[SG2_Delete_Customer]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_Delete_Customer_All]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SG2_Delete_Customer_All]
@riCustomerId int,
@riSocialProfileId Int

 
As  
Begin

IF @riSocialProfileId =0 
BEGIN 
 -- Searches for Customers based on given parameters  
DELETE PM
FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM Inner join [dbo].[SG2_SocialProfile]  SP
					ON PM.SocialProfileId=SP.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C ON C.CustomerId=SP.CustomerId
						Where C.CustomerId=@riCustomerId
						
DELETE TI FROM [dbo].[SG2_SocialProfile_TargetingInformation] TI Inner join [dbo].[SG2_SocialProfile]  SP
					ON TI.SocialProfileId=SP.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C ON C.CustomerId=SP.CustomerId
						Where C.CustomerId=@riCustomerId
DELETE SST
 FROM [dbo].[SG2_SocialProfile_Statistics] SST Inner join [dbo].[SG2_SocialProfile]  SP
					ON SST.SocialProfileId=SP.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C ON C.CustomerId=SP.CustomerId
						Where C.CustomerId=@riCustomerId

DELETE SS
FROM [dbo].[SG2_SocialProfile_Subscription] SS Inner join [dbo].[SG2_SocialProfile]  SP
					ON SS.SocialProfileId=SP.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C ON C.CustomerId=SP.CustomerId
						Where C.CustomerId=@riCustomerId
DELETE SPSH
FROM [dbo].[SG2_SocialProfile_StatusHistory] SPSH Inner join [dbo].[SG2_SocialProfile]  SP
					ON SPSH.SocialProfileId=SP.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C ON C.CustomerId=SP.CustomerId
						Where C.CustomerId=@riCustomerId


DELETE FROM [dbo].[SG2_SocialProfile] 
 Where [CustomerId]=@riCustomerId

DELETE FROM  [dbo].[SG2_Customer_ContactDetail]  Where [CustomerId]=@riCustomerId
DELETE FROM [dbo].[SG2_Customer] Where [CustomerId]=@riCustomerId 
END 

ELSE

BEGIN 

DELETE PM
FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM where PM.SocialProfileId=@riSocialProfileId
						
DELETE TI FROM [dbo].[SG2_SocialProfile_TargetingInformation] TI
						Where TI.SocialProfileId=@riSocialProfileId
DELETE SST
 FROM [dbo].[SG2_SocialProfile_Statistics] SST 
						Where SST.SocialProfileId=@riSocialProfileId

DELETE SS
FROM [dbo].[SG2_SocialProfile_Subscription] SS
						Where SS.SocialProfileId=@riSocialProfileId
DELETE SPSH
FROM [dbo].[SG2_SocialProfile_StatusHistory] SPSH 
						Where SPSH.SocialProfileId=@riSocialProfileId


DELETE FROM [dbo].[SG2_SocialProfile] 
 Where SocialProfileId=@riSocialProfileId

END

SELECT 1
     
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_Get_AllCustomers]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_Get_AllCustomers]


 
As  
Begin

 -- Searches for Customers based on given parameters  

SELECT Distinct CustomerId, ISnull(FirstName,'') + ' ' + Isnull(SurName,'') as CustomerName  FROM SG2_Customer
 
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_AssignJVBoxToCustomer]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		exists(SELECT 1 from [dbo].[SG2_SocialProfile_Subscription] where [SocialProfileId]= @riProfileId and StatusId=25))
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_AssignedNearestProxyIP]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_GetPendingSocialProfiles]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_GetSocialProfileById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Customer_GetSocialProfileById]
  @riSocialProfileId int

As
Begin
	  DECLARE @iServerRunningStatus INT 
      DECLARE @JVBoxes TABLE 
        ( 
           jvboxid       INT, 
           maxcount      INT, 
           assignedcount INT, 
           jvboxstatus   INT ,
		   ServerRunningStatus int
        ) 

		INSERT INTO @JVBoxes 
                        (jvboxid, 
                         maxcount, 
                         assignedcount, 
                         jvboxstatus,
						 ServerRunningStatus
						 ) 
            SELECT JVBox.jvboxid, 
                   maxlimit, 
                   Count(C.jvboxid), 
                   JVBox.statusid ,
				   JVBox.ServerRunningStatusId
            FROM   sg2_jvbox JVBox 
                   LEFT JOIN [dbo].[sg2_socialprofile] C 
                          ON JVBox.jvboxid = C.jvboxid
			where  JVBox.StatusId=19 
            GROUP  BY JVBox.jvboxid, 
                      maxlimit, 
                      JVBox.statusid ,
					   JVBox.ServerRunningStatusId

			 IF EXISTS(SELECT 1 
                      FROM   @JVBoxes 
                      WHERE  assignedcount < maxcount 
                             AND jvboxstatus = 19) 
              BEGIN 
                  SELECT TOP 1 @iServerRunningStatus =  ServerRunningStatus
                  FROM   @JVBoxes 
                  WHERE  assignedcount < maxcount 
                  ORDER  BY jvboxid ASC 

              END 

	
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
	   ,SP.[SocialPassword]
       ,SP.[SocialPrefferedCountry] PrefferedCountryId
	   ,SP.[SocialPrefferedCity] PrefferedCityId
	   ,Sp.SocialProfileTypeId
	   ,EV4.Name as SocialProfileType
	   ,EV.Name as [ProfileStatus]
	   ,SP.StatusId as ProfileStatusId
	   ,SP.JVboxId
	   ,EV2.Name as JVBoxStatusName
	   ,pp.PlanId
	   ,pp.PlanName
	   ,pp.StripePlanId
	   ,Subc.SubscriptionId
	   ,Subc.StartDate as SubscriptionStartDate
	   ,Subc.EndDate As SubscriptionEndDate
	   ,Subc.StatusId as SubscriptionStatusId
	   ,EV3.Name as SubscriptionStatus
	   ,proxy.[ProxyIPNumber]
	   ,proxy.[ProxyIPName]
	   ,proxy.[ProxyPort]
	   ,Subc.[StripeSubscriptionId]
	   ,SP.JVBoxStatusId as JVStatusId
	   ,SP.[JVAttempts] as JVAttempts
	   ,SP.JVAttemptsBlockedTill as JVAttemptsBlockedTill
	   ,SP.JVAttemptStatus as JVAttemptStatus 
	   ,JVBox.[ExchangeName]  as JVBoxExchangeName
	   ,CTI.SocialAccAS
	   ,Subc.StripeInvoiceId
	   ,proxy.ProxyId
	   
	   ,
	   CASE 
	   WHEN JVBox.ServerRunningStatusId IS null
	   THEN CASE @iServerRunningStatus
			WHEN 1 THEN 1
			ELSE 0
		END
	   ELSE 
	   (CASE JVBOX.[ServerRunningStatusId]
			WHEN 1 THEN 1
			ELSE 0
		END)
		END ISJVSERVERRUNNING,
		SP.IsArchived
	FROM  [dbo].[SG2_SocialProfile_TargetingInformation] CTI
		INNER JOIN [dbo].[SG2_SocialProfile] SP 
			ON CTI.[SocialProfileId]=SP.[SocialProfileId]
		INNER JOIN [dbo].[SG2_Customer] C 
			ON C.[CustomerId]=SP.[CustomerId]
		Left join SG2_EnumerationValue EV 
			ON EV.EnumerationValueId = SP.StatusId
		Left join SG2_EnumerationValue EV2
			ON EV2.EnumerationValueId = SP.jvboxstatusid
		Left join SG2_EnumerationValue EV4
			ON EV.EnumerationValueId = SP.socialprofiletypeid
		Left join [dbo].[SG2_SocialProfile_Subscription] Subc
			on Subc.socialprofileid = SP.socialprofileid and subc.statusid = 25
		Left Join [dbo].[SG2_SocialProfile_PaymentPlan] PP
			on Subc.paymentplanid = pp.planid
		Left join SG2_EnumerationValue EV3
			ON EV3.EnumerationValueId = subc.statusid
		Left join [dbo].[SG2_SocialProfile_ProxyMapping] PM
			on PM.socialprofileid = sp.socialprofileid
		Left join [dbo].[SG2_Proxy] Proxy
			on proxy.proxyid = pm.proxyid
		Left join [dbo].[SG2_JVBox] JVBox ON JVBox.[JVBoxId]=SP.[JVBoxId]
	where CTI.SocialProfileId = @riSocialProfileId

End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_GetSocialProfilesByCustomerId]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
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
		SP.JVBoxId,
		SP.JVBoxStatusId,
		SP.StatusId,
		SP.StripeCustomerId,
		SP.SocialUsername,
		SP.SocialPassword,
		SP.SocialProfileName as ProfileName,
		SP.SocialPrefferedCity AS PrefferedCityId,
		SP.SocialPrefferedCountry AS PrefferedCountryId,
		EV.[Description] StatusName,
		EV1.[Description] JVBoxStatusName,
		EV2.[DESCRIPTION] SocialProfileTypeName,
		JV.[BoxName] AS JVBoxName,
		CASE 
		WHEN SS.SubscriptionId IS Null
		THEN 'Expired'
		ELSE 
		'Active'
		End SubscriptionStatus,
		SS.[Name] as SubscriptionName
	FROM [dbo].[SG2_SocialProfile] SP
	LEFT JOIN [dbo].[SG2_SocialProfile_Subscription] SS ON SP.[SocialProfileId]=SS.[SocialProfileId]
															AND SS.StatusId=25
	INNER JOIN [dbo].[SG2_EnumerationValue] EV
		ON EV.[EnumerationValueId] = SP.STATUSID
	LEFT JOIN [dbo].[SG2_EnumerationValue] EV1
		ON EV1.[EnumerationValueId] = SP.JVBOXSTATUSID
	LEFT JOIN [dbo].[SG2_EnumerationValue] EV2
		ON EV2.[EnumerationValueId] = SP.SOCIALPROFILETYPEID
	LEFT JOIN [dbo].[SG2_JVBox] JV
		ON JV.[JVBoxId] = SP.JVBOXID
	WHERE SP.CUSTOMERID = @CustomerId;
END
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_ProfileUpdate]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

	Update  [dbo].[SG2_Customer] 
	set 
		FirstName=@rvcFirstName,
		SurName=@rvcSurName, 	
		UserName=@rvcUserName 
	where CustomerId=@iCustomerId;

If not exists(Select 1 From dbo.SG2_Customer_ContactDetail where CustomerId = @iCustomerId) 
	Begin
		Insert into dbo.SG2_Customer_ContactDetail
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
		Update dbo.SG2_Customer_ContactDetail
			set 
				PhoneNumber = @rvcPhoneNumber,
				PhoneCode = @rvcPhoneCode 
			where 
				CustomerId=@iCustomerId
	End;

exec [dbo].[SG2_usp_Customers_Get] @iCustomerId
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_SavePreference]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_ScheduleCall]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_SignUp]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_Customer_SignUp]
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
	IF Not Exists(SELECT 1 FROM [dbo].[SG2_Customer] where [EmailAddress]=@rvcEmailAddress )
	BEGIN
	INSERT INTO [dbo].[SG2_Customer]
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
		Insert into dbo.SG2_Customer_ContactDetail 
		(
			[CustomerId]   
			,[GUID]
		)
		Values (
			@iCustomerId,
			NEWID()
		)
	END

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
		'Social Profile 1',
		getdate(),
		@rvcEmailAddress,
		getdate(),
		@rvcEmailAddress

	SELECT @iSocialProfileId= @@IDENTITY

	Insert into [dbo].[SG2_SocialProfile_TargetingInformation]
	(
		[SocialProfileId],
		[Preference1],
		[Preference2],
		[Preference3],
		[Preference4],
		[Preference5],
		[Preference6],
		[Preference7],
		[Preference8],
		[Preference9],
		[CreatedOn],
		[CreatedBy],
		[UpdatedOn],
		[UpdatedBy],
		Preference10,
		[SocialAccAs]
	)
	values(
		@iSocialProfileId,
		'Dogs,Cats',
		'Snake',
		'New York,London',
		'Moon',
		1,
		3,
		1,
		'Fitness',
		'',
		getdate(),
		@rvcEmailAddress,
		getdate(),
		@rvcEmailAddress,
		1,
		2
	)

     END

exec [dbo].[SG2_usp_Customers_Get] @iCustomerId

End


GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customer_SignUpCustomerWithPreference]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Customer_SignUpCustomerWithPreference]
	@rvcFirstName	  Nvarchar(50),
    @rvcLastName	  Nvarchar(50),
    @rvcEmailAddress  Nvarchar(64),
    @rvcPassword	  Nvarchar(64),
	@rvcGUID		  Nvarchar(50),
	@rvcLastLoginIP   Nvarchar(20),
    @rvcPreference1	  nvarchar(255) ,
	@rvcPreference2	  nvarchar(255) ,
	@rvcPreference3	  nvarchar(255) ,
	@rvcPreference4	  nvarchar(255) ,
	@iPreference5	  int,
	@iPreference6	  int,
	@rvcCity          int= null,
	@rvcStatusId	  Int=1
As  
 
 	-- Searches for Customers based on given parameters  
	Declare @iCustomerId int,@iSocialProfileId int
	If not exists(Select 1 From dbo.[SG2_Customer] where CustomerId= @iCustomerId ) 
	Begin

		INSERT INTO [dbo].[SG2_Customer]
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
			Insert into dbo.SG2_Customer_ContactDetail 
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
		If Not exists (Select 1 From [dbo].[SG2_SocialProfile] where [CustomerId]= @iCustomerId)
		BEGIN
			INSERT INTO [dbo].[SG2_SocialProfile]
           ([CustomerId]
           ,[SocialProfileTypeId]
           ,[JVBoxId]
           ,[JVBoxStatusId]
           ,[StatusId]
           ,[StripeCustomerId]
           ,[SocialUsername]
           ,[SocialPassword]
           ,[SocialProfileName]
           ,[SocialPrefferedCity]
           ,[SocialPrefferedCountry]
           ,[CreatedOn]
           ,[CreatedBy]
           ,[UpdatedOn]
           ,[UpdatedBy])
     VALUES
           (@iCustomerId
           ,30
           ,null
           ,null
           ,5
           ,null
           ,null
           ,null
           ,'Social Profile 1'
           ,@rvcCity
           ,null
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
				From [dbo].[SG2_SocialProfile] where [CustomerId]= @iCustomerId order by CreatedOn desc
		END
	If not exists(Select 1 From [dbo].[SG2_SocialProfile_TargetingInformation] where [SocialProfileId]= @iSocialProfileId ) 
	Begin
		INSERT INTO [dbo].[SG2_SocialProfile_TargetingInformation]
           ([SocialProfileId]
           ,[Preference1]
           ,[Preference2]
           ,[Preference3]
           ,[Preference4]
           ,[Preference5]
           ,[Preference6]
           --,[Preference7]
           --,[Preference8]
           --,[Preference9]
           --,[Preference10]
           --,[InstaUser]
           --,[InstaPassword]
		   ,CreatedOn,
			UpdatedOn
			-- City
		   )
			 VALUES
			 (@iSocialProfileId	,
			  @rvcPreference1	,
			  @rvcPreference2	,
			  @rvcPreference3,	
			  @rvcPreference4	,
			  @iPreference5	,
			  @iPreference6	,
			  --@iPreference7	,
			  --@rvcPreference8,	
			  --@rvcPreference9	,
			  --@rvcPreference10,
			  --@rvcInstaUser	,
			  --@rvcInstaPassword,
			  getdate(),
			  GETDATE()
			  

	   )
	END

	SELECT TOP 1
		GUID,
		C.CustomerId,
		C.[FirstName],
		[EmailAddress],
		[SurName],
		[Password],
		C.StripeCustomerId,
		SP.[SocialProfileId]
	From [dbo].[SG2_Customer]  C
		inner join [dbo].[SG2_SocialProfile] SP ON C.[CustomerId]=SP.[CustomerId]
			AND SP.[SocialProfileId]=@iSocialProfileId
	WHERE C.CustomerId= @iCustomerId

End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Customers_Get]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
      ,SP.[JVBoxId]
      ,CST.[StripeCustomerId]
      ,CST.[UserName]
      ,SP.[JVBoxStatusId]
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
	  ,(Select top 1 SocialProfileId from SG2_SocialProfile where CustomerId = @Id) As DefaultSocialProfileId 
	  FROM  [dbo].[SG2_Customer] CST
			Left Join [dbo].[SG2_Customer_ContactDetail] CD on cd.customerid = cst.customerid
			Inner join [dbo].[SG2_SocialProfile] SP ON SP.[CustomerId]=cd.customerid
			Left Join [dbo].[SG2_SocialProfile_Subscription] sub on sub.[SocialProfileId]= SP.[SocialProfileId] AND sub.StatusId = 25 -- Active Subsription
		Where CST.CustomerId=@Id
   
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Delete_QueueAuditAndDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Delete_QueueAuditAndDetail]
  @riTransactionId nvarchar(max)

As  
Begin

 	
	Delete from  [dbo].[SG2_QueueAuditDetail] where transactionId=@riTransactionId

	DELETE FROM  [dbo].[SG2_QueueAudit] where transactionId=@riTransactionId


    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_AllUser]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_AllUser]
 
As  
Begin

SELECT Distinct [SystemUserId] as UserId,FirstName as UserName,SR.[Name] as RoleName  FROM [dbo].[SG2_SystemUser] SU
								LEFT JOIN SG2_SystemRole SR ON SU.SystemRoleId=SR.RoleId
  
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_AvailableMPBoxes]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_Get_AvailableMPBoxes]

As  
Begin

SELECT [JVBoxId], 
	  [BoxName]
FROM [dbo].[SG2_JVBox]

End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_AvailableProxies]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_AvailableProxies]
@riCountryId					INT,
@riCityId	        INT = null
 
As  
Begin

DECLARE @tblAvailableProxyIPs TABLE
(
    ProxyId INT,
	ProxyIPNumber NVARCHAR(max),
	FreeSlots Int
)
INSERT INTO @tblAvailableProxyIPs
SELECT P.ProxyId,P.ProxyIPNumber+' '+ISNULL(SC.Name,'') + ' '+ISNULL(SConty.Name,'') ,
((SELECT COUNT(ProxyId) FROM [dbo].[SG2_SocialProfile_ProxyMapping] WITH(NOLOCK) WHERE ProxyId = P.ProxyId ) - 3) FreeSlots
FROM [dbo].[SG2_Proxy] P
LEFT JOIN [dbo].[SG2_SystemCity] SC ON P.BaseCity=SC.CityId
LEFT JOIN [dbo].[SG2_SystemCountry] SConty ON SConty.CountryId=P.BaseCountry
where [BaseCountry]=@riCountryId 
								AND ([BaseCity]=@riCityId or ISNULL(@riCityId,0) =0 )
								and P.[StatusId]=19


DELETE FROM @tblAvailableProxyIPs WHERE FreeSlots = 0

SELECT [ProxyId] as ProxyId,
		ProxyIPNumber as ProxyIPNumber
		FROM  @tblAvailableProxyIPs

  
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_CustomerOrderHistory]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_CustomerOrderHistory]
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
 JVBoxStatus			nvarchar(50),
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
				EV.[Name] as JVBoxStatus ,
				EV2.[Name] as SocialProfileStatus,
				EV3.[Name] as SubscriptionStatus,
				 ROW_NUMBER() OVER (PARTITION BY S.[SocialProfileId] ORDER BY S.StartDate desc) AS RankId
	
		 FROM [dbo].[SG2_SocialProfile_Subscription] S
		 Inner Join [dbo].[SG2_SocialProfile] SP ON SP.[SocialProfileId]=S.[SocialProfileId]
												AND SP.SocialProfileId= @riSocialProfileId	
		 inner Join [dbo].[SG2_Customer] C on C.[CustomerId]=SP.[CustomerId]	
		 Left Join [dbo].[SG2_EnumerationValue]	EV on EV.[EnumerationValueId]	=	SP.[JVBoxStatusId]	
		 Left join 	[dbo].[SG2_EnumerationValue]	EV2 on EV2.[EnumerationValueId]	=	SP.	[StatusId]
		 Left join 	[dbo].[SG2_EnumerationValue]	EV3 on EV3.[EnumerationValueId]	=	S.	[StatusId]
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
		JVBoxStatus		,
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
		JVBoxStatus,
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
			JVBoxStatus,
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_CustomerTargetingInformation]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_CustomerTargetingInformation]
  @riSocialProfileId int

As
Begin
 
 Select [Preference1]
      ,[Preference2]
      ,[Preference3]
      ,[Preference4]
      ,[Preference5]
      ,[Preference6]
      ,[Preference7]
      ,[Preference8]
      ,[Preference9]
	  ,[Preference10]
      ,CAST(Cntry.[CountryId] as int) as SocialPrefferedCountry,
      SP.SocialUsername as SocialUsername
      ,SP.[SocialPassword]
	  ,C.CustomerId,
	  SP.SocialProfileName as SocialProfileName,
	  EV.EnumerationValueId as [JVBoxStatus],
	  SP.[SocialPrefferedCity] as City,
	  CTI.SocialProfileId,
	  EV1.Name as SPStatus,
	  C.[EmailAddress] as Email,
	  Isnull(C.[FirstName],'') + Isnull(C.[SurName],'') as UserName,
	   EV.Name as [JVBoxStatusName],
	   (SELECT COUNT(CustomerId) from [dbo].[SG2_SocialProfile] where CustomerId=C.CustomerId) as NoOfProfile,
	   P.ProxyIPNumber,
	   JV.[BoxName],
	   SocialAccAs,
	   SP.VerificationCode as VerificationCode,
	   JV.[JVBoxId] ,
	   C.Comment,
	   P.ProxyId,
	   P.ProxyPort,
	   	 ISNULL(SP.IsArchived,0) as IsArchived
		   FROM  [dbo].[SG2_SocialProfile_TargetingInformation] CTI
   INNER JOIN [dbo].[SG2_SocialProfile] SP ON CTI.[SocialProfileId]=SP.[SocialProfileId]
	AND SP.SocialProfileId=@riSocialProfileId
   INNER JOIN [dbo].[SG2_Customer] C ON C.[CustomerId]=SP.[CustomerId]
   LEFT JOIN [dbo].[SG2_SocialProfile_ProxyMapping] PM on PM.SocialProfileId=SP.SocialProfileId
   LEFT JOIN [dbo].[SG2_Proxy] P ON P.ProxyId=PM.ProxyId
   LEFT JOIN [dbo].[SG2_JVBox] JV ON JV.[JVBoxId]=SP.[JVBoxId]
   left JOIN [dbo].[SG2_SystemCity] Cty on Cty.[CityId]= SP.[SocialPrefferedCity]
   Left Join [dbo].[SG2_SystemCountry] Cntry ON Cntry.[CountryId]=Cty.CountryId
   Left join SG2_EnumerationValue EV ON EV.EnumerationValueId=SP.JVBoxStatusId --and EV.EnumerationId=1
   Left join SG2_EnumerationValue EV1 ON EV1.EnumerationValueId=SP.[StatusId]
							where CTI.SocialProfileId=@riSocialProfileId
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_EnumerationValue]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_EnumerationValue]

As
Begin
 

 Select E.[Name] as Enumeration , EV.EnumerationValueId , EV.[Name]  
 
 
  FROM  SG2_Enumeration E inner join SG2_EnumerationValue EV
						ON E.EnumerationId=EV.EnumerationId
  WHERE EV.IsVisible = 1
  order by Enumeration, SequenceNo 
End


GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_Product]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_Product]
 
As  
Begin

SELECT PlanName as [Name],StripePlanId as StripeSubscriptionId
FROM [dbo].[SG2_SocialProfile_PaymentPlan] 

End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_ProxyCitiesAndCountries]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SG2_usp_Get_ProxyCitiesAndCountries]
@riCityId int=null

 
As  
Begin

 -- Searches for Customers based on given parameters  


 if @riCityId is null
 BEGIN
SELECT Distinct 
BaseCity as CityId, 
BaseCountry as CountryId ,
SGC.[Name]  as CountryName, 
SG.[Name] as CityName,
SGC.[Name] + ' , ' + SG.[Name]  as FullCityCountryName
 FROM SG2_Proxy P 
 Inner join SG2_SystemCountry SGC ON P.BaseCountry=SGC.CountryId
 inner join SG2_SystemCity SG on P.BaseCity=SG.CityId
 --where 1=1 and (@riCityId is null or (@riCityId is not null and @riCityId=SG.CityId))
 END
 ELSE 
 BEGIN
 SELECT Distinct 
Cast(SG.CityId as varchar(50)) as CityId, 
Cast(SGC.CountryId as varchar(50)) as CountryId ,
SGC.[Name]  as CountryName, 
SG.[Name] as CityName,
SGC.[Name] + ' , ' + SG.[Name]  as FullCityCountryName
 FROM SG2_SystemCountry SGC inner join SG2_SystemCity SG on SGC.CountryId=SG.CountryId
 where SG.CityId=@riCityId

 END
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_SocialProfile_PaymentPlan]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_Get_SocialProfile_PaymentPlan]
As
Begin

	SELECT 
		[PlanId],
		[NoOfLikes] as Likes,
		[StripePlanPrice] as PlanPrice,
		[PlanName],
		[PlanShortDescription] as PlanDescription,
		[PlanTypeId] as PlanTypeId,
		EV.[Name] as PlanType,
		[StripePlanId],
		ISNULL([NoOfLikesDuration],0) as NoOfLikesDuration ,
		[StatusId],
		EV2.[Name] as StatusName,
		[SortOrder],
		[SocialPlanTypeId],
		SP.[IsDefault],
		DisplayPrice
	FROM [dbo].[SG2_SocialProfile_PaymentPlan] SP
		Left join [dbo].[SG2_EnumerationValue] EV on SP.PlanTypeId=EV.EnumerationValueId
		Left Join [dbo].[SG2_EnumerationValue] EV2 on SP.StatusId=EV2.EnumerationValueId
	WHERE
		SP.STATUSID = 19
	Order by SortOrder asc

End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_SpecificCustomerDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	 EV.Name as [Status],
	 JV.BoxName JVBoxNo,
	 P.ProxyIPNumber IPNO, 
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
	 EV1.[Name] as JVStatus,
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
	 ISNULL(SP.IsArchived,0) as IsArchived,
	 P.ProxyPort
	 from SG2_Customer C 
		inner join  SG2_Customer_ContactDetail CCD
			ON C.CustomerId=CCD.CustomerId
		Inner join [dbo].[SG2_SocialProfile] SP ON SP.[SocialProfileId] =@riProfileId
		Left join SG2_JVBox JV ON JV.JVBoxId=SP.JVBoxId
		Left join [dbo].[SG2_SocialProfile_ProxyMapping] PM ON PM.[SocialProfileId]=SP.[SocialProfileId]
		Left join SG2_Proxy P ON P.ProxyId=PM.ProxyId
		Left join [dbo].[SG2_SocialProfile_TargetingInformation] CTI ON CTI.[SocialProfileId]=SP.[SocialProfileId]
		Left join SG2_SystemUser SU ON SU.SystemUserId=C.ResponsibleTeamMemberId
		Left join SG2_EnumerationValue EV ON EV.EnumerationValueId=SP.StatusId-- AND EV.EnumerationId=1
		Left Join SG2_EnumerationValue EV1 ON EV1.EnumerationValueId=SP.JVBoxStatusId
 
	where C.CustomerId=@riCustomerId
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_Title]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_Title]
 
As  
Begin

SELECT Distinct [PkTitleId],[TitleName]   FROM  [dbo].[SG2_Customer_Title]
  
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Get_VPSSupplier]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Get_VPSSupplier]
 
As  
Begin

SELECT Distinct [VPSSId] , [IssuingISPName]  FROM  [dbo].[SG2_VPSSupplier] SU
								
 
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_GetBadProxyIPs]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_GetBadProxyIPs]
(
  @riPageSize varchar(8),
  @riPageNumber Int

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


 Declare  @tbResult table(
 RowNumber     int ,
 ProxyId     int,
 ProxyIPNumber      nvarchar(250),
 Profiles    nvarchar(max)
 
 ) 

 
 ;with cte As(
  SELECT	DISTINCT	P.ProxyId,
				P.ProxyIPNumber, 
 ( 
					stuff((
						Select ','+SP.SocialUsername FROM [dbo].[SG2_SocialProfile] SP
						inner join [dbo].[SG2_SocialProfile_BadProxy] BP ON BP.SocialProfileId=SP.SocialProfileId
																		AND SP.SocialProfileId=BP.SocialProfileId
																		AND BP.ProxyId=BP1.ProxyId
						
						group by SP.SocialUsername
						for xml path ('')
					),1,1,'')
					
				) Profiles

FROM [dbo].[SG2_SocialProfile_BadProxy] BP1
inner join [dbo].[SG2_Proxy] P ON BP1.ProxyId=P.ProxyId
  )

Insert into @tbResult
Select   ROW_NUMBER() OVER (ORDER BY [ProxyId] desc) AS RankId,
		ProxyId,  
		ProxyIPNumber,
		Profiles
 From cte 

				
Select  ProxyId,  
		ProxyIPNumber,
		Profiles,
     (Select Count(1) From @tbResult) TotalRecord
 From @tbResult 
 Where RowNumber Between @iFirstRow And @iLastRow
	
 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_GetProfilebyJVStatusId]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_GetProfilebyJVStatusId]
@riJVStatusId int =null
As
Begin
 
  Select	Customer.FirstName + ' ' + SurName as FullName,
			Customer.CustomerId as CustomerId,
			SP.SocialProfileId as SPId,
			Customer.EmailAddress as Email,
			EV.[Name] as JVStatusName,
			EV.EnumerationValueId as JVStatusId,
			SP.[SocialUsername] as [InstaUsrName],
			SP.SocialProfileName as SocialProfileName,
			TI.Preference1 as HashTagEngaged,
			TI.Preference2 as HashTagNotEngaged,
			TI.Preference3 as LocationToFocus,
			TI.Preference4 as LocationToSkip,
			TI.Preference5 as EffectiveGrowthStrategy,
			TI.Preference6 as GenderToEngage,
			TI.Preference7 as IncludeBusinessAccount,
			TI.Preference8 as UserDemographics,
			TI.Preference9 as DirectCompetitors,
			SP.SocialPrefferedCity as SPCity,
			SP.SocialPrefferedCountry as SPCountry,
			Left(Customer.Comment,150)+'...' as Comment,
			ISNULL(SP.IsArchived,0) as  IsArchived
  From dbo.SG2_Customer Customer With (Nolock) 
  inner join [dbo].[SG2_SocialProfile] SP ON SP.[CustomerId]=Customer.[CustomerId]
									--AND SP.isarchived=1
  Left join SG2_JVBox JV ON JV.JVBoxId=SP.JVBoxId
  Left join dbo.SG2_EnumerationValue EV ON EV.EnumerationValueId=SP.JVBoxStatusId And EV.EnumerationId=3
  Left join [dbo].[SG2_SocialProfile_TargetingInformation] TI on TI.[SocialProfileId]=SP.[SocialProfileId]
  where EV.Name is not null
		AND (@riJVStatusId is null or SP.JVBoxStatusId=@riJVStatusId)
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_GetProxyIpDataAgainstSupplier]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_GetProxyIpDataAgainstSupplier]
(
	@riVPSSId					INT
)
 
AS  
Begin
 				 
  SELECT DISTINCT
		       P.ProxyId,
				 ProxyIPNumber,
				 (3-(Select count(1) From [dbo].[SG2_SocialProfile_ProxyMapping] PV Where PV.ProxyId = P.ProxyId))  FreeSlots,
				 ( 
					stuff((
						Select ','+JV.BoxName FROM SG2_JVBox JV 
						inner join [dbo].[SG2_SocialProfile] SP ON SP.JVBoxId=JV.JVBoxId
						inner JOIN [dbo].[SG2_SocialProfile_ProxyMapping] PM On P.ProxyId= PM.ProxyId AND pm.[SocialProfileId] = SP.[SocialProfileId]
						group by jv.BoxName
						for xml path ('')
					),1,1,'')
					
				) JVBoxes,				 
				 SCity.[Name] + ', '+ SC.[Name]  Region,
				 EV.[Name] as ProxyIPStatus,
				 VPSS.VPSSId
				  as VPSSId,
				 VPSS.IssuingISPName as VPSSName
  FROM dbo.SG2_Proxy P WITH (NOLOCK) 
  INNER JOIN dbo.SG2_SystemCountry SC ON SC.CountryId=P.BaseCountry
  INNER JOIN dbo.SG2_SystemCity SCity ON SCity.CityId=P.BaseCity
  Left JOIN SG2_EnumerationValue EV ON P.StatusId=EV.EnumerationValueId
  Left JOIN SG2_VPSSupplier VPSS on VPSS.VPSSId=P.VPSSId
  Where VPSS.VPSSId= @riVPSSId
  Order by VPSS.VPSSId desc
 
         
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_GetUserDetailsForbackOffice]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
 JVBoxStatus	nvarchar(100),
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
			Proxy.ProxyIPNumber as Proxy,
			JVBox.BoxName as Box,
			EV.[Name] as [Status],
			Customer.updatedon,
			EV1.[Name] as [JVBoxStatus],
			customer.[EmailAddress] as EmailAddress ,
            ROW_NUMBER() OVER (PARTITION BY SP.[SocialProfileId] ORDER BY SubS.StartDate desc) AS RankId
  From dbo.SG2_Customer Customer With (Nolock) 
  Inner join [dbo].[SG2_SocialProfile] SP ON SP.CustomerId=Customer.CustomerId
	left join dbo.SG2_JVBox JVBox 
		ON SP.JVBoxId=JVBox.JVBoxId 
	left join [dbo].[SG2_SocialProfile_ProxyMapping] Custproxy 
		On Custproxy.[SocialProfileId]=SP.[SocialProfileId]
	Left join dbo.SG2_Proxy Proxy 
		On Proxy.ProxyId=Custproxy.ProxyId  
	left join [dbo].[SG2_SocialProfile_Subscription] SubS 
		ON SubS.[SocialProfileId]=SP.[SocialProfileId] 
				--AND SubS.StatusId=25
	left join [dbo].[SG2_SocialProfile_TargetingInformation] TI 
		On TI.[SocialProfileId]=SP.[SocialProfileId]
	Left join dbo.SG2_EnumerationValue EV 
		ON EV.EnumerationValueId=SP.StatusId --And EV.EnumerationId=4
	Left Join dbo.SG2_EnumerationValue EV1
		ON EV1.EnumerationValueId=SP.JVBoxStatusId --AND EV1.EnumerationId=3
	Where (
			((@riStatusId is null) or Customer.StatusId = @riStatusId)
		 AND 
		 ((@riJVStatus is null) or SP.JVBoxStatusId = @riJVStatus)
		 AND 
		  	(((@riProductId is null) or @riProductId ='' ) or  SubS.[StripePlanId] = @riProductId)
		AND ((@riSubscription is null) or SubS.StatusId= @riSubscription)
		And ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (SP.[SocialProfileName] like '%' +@rsSearchCrite +'%' 
				or Customer.UserName like '%' +@rsSearchCrite +'%'
				 or SubS.Name like '%' +@rsSearchCrite +'%' 
				 or Proxy.ProxyIPNumber like '%' +@rsSearchCrite +'%' 
				  or JVBox.BoxName like '%' +@rsSearchCrite +'%' 
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
ProxyIPNumber,
BoxName     , 
[Status]   ,
JVBoxStatus,
SocialProfileName,
SocialProfileId,
CustomerEmail

)
SELECT Distinct
 ROW_NUMBER() Over (  
           Order By UpdatedOn desc
            ) As RowNumber,
SocialAccountName,[Name],CustomerId,Products,Proxy,Box,[Status],JVBoxStatus,SocialProfileName,
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_GETVPSSupplierData]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_GETVPSSupplierData]
(
	@riVPSSId					INT
	
)
 
AS  
 
 
BEGIN

SELECT      VPSSId,
		    IPManageBy,
		    SupportPhone,
		    SupportEmail,
		    IssuingISPName,
		    IssuingISPPhone,
		    IssuingISPWebsite,
		    IssuingISPAccount,
		    IssuingISPPassword,
		    IssuingISPMemo,
			StatusId
  FROM [dbo].[SG2_VPSSupplier]  WHERE VPSSId= @riVPSSId
					
 
 
         
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_Delete]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_JVBox_Delete]
  @riJVBoxId Int

As  
Begin

 	
	IF NOT EXISTS(SELECT 1 FROM [dbo].[SG2_SocialProfile] where JVBoxId=@riJVBoxId)
	Begin
		UPDATE SG2_JVBox 
		SET StatusID = 18
		WHERE JVBoxId = @riJVBoxId

		return 1;

	 END
	 else
		return 0;
 
    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_JVBox_GetAll]
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
 
 ;With cte as (
	SELECT       JVB.JVBoxId,
				 BoxName,
			     (Select Count(1) From [dbo].[SG2_SocialProfile] CUS Where CUS.JVBoxId = JVB.JVBoxId and CUS.StatusId = 19 ) LiveUser,
				 (Select Count(1) From [dbo].SG2_JVBox) TotalRecord,
				 JVB.MaxLimit as MaxLimit,
				 EV.Name as JVBStatus,
				[ServerRunningStatusId] ,
				  ROW_NUMBER() OVER (ORDER BY JVB.JVBoxId desc) AS RowNumber
	  FROM [dbo].SG2_JVBox JVB
	  INNER JOIN [dbo].SG2_EnumerationValue EV ON JVB.StatusId= EV.EnumerationValueId
		 INNER JOIN [dbo].SG2_Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'General'
		-- INNER JOIN dbo.SG2_Customer C ON JVB.JVBoxId = C.JVBoxId
		 Where (
			((@riStatusId is null) or JVB.StatusId = @riStatusId)
			AND ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (JVB.BoxName like '%' +@rsSearchCrite +'%' or JVB.HostedBy like '%' +@rsSearchCrite +'%' or JVB.AdminName like '%' +@rsSearchCrite +'%' )
			)
			
			))

SELECT JVBoxId,BoxName,LiveUser,TotalRecord,MaxLimit,JVBStatus,ISNULL(ServerRunningStatusId,1) as ServerRunningStatusId
 FROM cte where  RowNumber Between @iFirstRow And @iLastRow
			
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_JVBox_GetById]
  @riJVBoxId Int

As  
Begin

SELECT [JVBoxId], 
	  [BoxName], 
	  [AdminName], 
	  [AdminPassword], 
	  [BoxManagedBy], 
	  [SupportPhone], 
	  [SupportEmail],
	  [HostedBy], 
	  [HostingPhone], 
	  [HostingWebsite], 
	  [HostingAccount], 
	  [HostingPassword], 
	  [HostingPriceInfo], 
	  [StatusId],
	  [MaxLimit],
      [JVBoxType],
	  [ExchangeName],
	  [ServerRunningStatusId]
FROM [dbo].[SG2_JVBox]
Where JVBoxId = @riJVBoxId

End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_GetCustomerHistory]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_JVBox_GetCustomerHistory]
(
  @riJVBoxId Int
)
As  
Begin



 
SELECT ISNULL(C.FirstName,'') + ' ' + ISNULL(C.SurName,'') as CusName,SP.[SocialUsername] as SocialUsername,EV.Name as Status, C.CustomerId
							 FROM SG2_Customer C 
							 inner join [dbo].[SG2_SocialProfile] SP ON SP.CustomerId=C.CustomerId
							Left join [dbo].[SG2_SocialProfile_TargetingInformation] CTI ON SP.[SocialProfileId]=CTI.[SocialProfileId]
							inner join SG2_EnumerationValue EV on C.StatusId=EV.EnumerationValueId 
							where JVBoxId=@riJVBoxId
	

 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_GetStatuses]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_JVBox_GetStatuses]

As
Begin
 
	Select	
		EV.EnumerationValueId as JVStatusId,
		EV.SequenceNo,
		EV.[Name] as JVStatusName,
		EV.DESCRIPTION AS JVStatusDesc
	From dbo.SG2_EnumerationValue EV 
		where EV.EnumerationId=3 and EV.IsVisible=1
	order by SequenceNo asc
  
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_JVBox_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_JVBox_Save]
(
	@riJVBoxId					INT,
	@riBoxName					NVARCHAR(100),
	@rvcAdminName				NVARCHAR(100),
	@rvcAdminPassword			NVARCHAR(50),
	@rvcBoxManagedBy			NVARCHAR(50),
	@rvcSupportEmail			NVARCHAR(50),
	@rvcSupportPhone			NVARCHAR(50),
	@rvcHostedBy				NVARCHAR(150),
	@rvcHostingPhone			NVARCHAR(50),
	@rvcHostingWebsite			NVARCHAR(50),
	@rvcHostingAccount			NVARCHAR(50),
	@rvcHostingPassword			NVARCHAR(50),
	@rvcHostingPriceInfo		NVARCHAR(50),
	@riStatusId					INT = 18,
	@riJVBoxMaxLimit           INT=Null,
	@riJVServerType				INT=NULL,
	@riJVBoxExchangeName       NVARCHAR(250),
	@riServerRunningStatusId	INT=1
)
 
AS  
 
 
BEGIN
	IF NOT EXISTS(SELECT 1 FROM [dbo].SG2_JVBox WHERE JVBoxId = @riJVBoxId ) 
		BEGIN
			INSERT INTO [dbo].SG2_JVBox
					   (
		   				 BoxName,
						 AdminName,
						 AdminPassword,
						 BoxManagedBy,
						 SupportEmail,
						 SupportPhone,
						 HostedBy,
						 HostingPhone,
						 HostingWebsite,
						 HostingAccount,
						 HostingPassword,
						 HostingPriceInfo,
						 StatusId,
						 MaxLimit,
						 [JVBoxType],
						 ExchangeName,
						[ServerRunningStatusId],
						[CreatedOn],
						[CreatedBy],
						[UpdatedOn],
						[Updateby]
						)
					 VALUES
					 (
						 @riBoxName,
						 @rvcAdminName,
						 @rvcAdminPassword,
						 @rvcBoxManagedBy,
						 @rvcSupportEmail,
						 @rvcSupportPhone,
						 @rvcHostedBy,
						 @rvcHostingPhone,
						 @rvcHostingWebsite,
						 @rvcHostingAccount,
						 @rvcHostingPassword,
						 @rvcHostingPriceInfo,
						 @riStatusId,
						 @riJVBoxMaxLimit,
						 @riJVServerType,
						 @riJVBoxExchangeName,
						 @riServerRunningStatusId,
						 GETDATE(),
						 'Admin',
						 GETDATE(),
						 'Admin'
						 )
			END
	ELSE
		BEGIN

		if @riJVBoxMaxLimit=null
		SELECT @riJVBoxMaxLimit= MaxLimit from [dbo].SG2_JVBox WHERE  JVBoxId= @riJVBoxId
			UPDATE [dbo].SG2_JVBox
			   SET
						BoxName = @riBoxName,
						AdminName = @rvcAdminName,
						AdminPassword = @rvcAdminPassword,
						BoxManagedBy = @rvcBoxManagedBy,
						SupportEmail = @rvcSupportEmail,
						SupportPhone = @rvcSupportPhone,
						HostedBy = @rvcHostedBy,
						HostingPhone = @rvcHostingPhone,
						HostingWebsite = @rvcHostingWebsite,
						HostingAccount = @rvcHostingAccount,
						HostingPassword = @rvcHostingPassword,
						HostingPriceInfo = @rvcHostingPriceInfo,
						StatusId = @riStatusId,
						MaxLimit=@riJVBoxMaxLimit,
						[JVBoxType]=@riJVServerType,
						ExchangeName=@riJVBoxExchangeName,
						[ServerRunningStatusId]=@riServerRunningStatusId,
						[UpdatedOn]=GETDATE(),
						[Updateby]='Admin'
			 WHERE  JVBoxId= @riJVBoxId
		End
END
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_LikeyAccount_Delete]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_LikeyAccount_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
				  (Select Count(1) From [dbo].SG2_LikeyAccount) TotalRecord,
				   ROW_NUMBER() OVER (ORDER BY  LikeyAccountId desc) AS RankId
	  FROM [dbo].SG2_LikeyAccount LA
	     INNER JOIN [dbo].SG2_EnumerationValue EV ON LA.StatusId= EV.EnumerationValueId
		 INNER JOIN [dbo].SG2_Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'General'
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_LikeyAccount_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_LikeyAccount_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Login_Customers]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

	SELECT TOP 1 @customerid = customerid FROM  [dbo].[SG2_Customer] where EmailAddress=@rvcEmailAddress and Password=@rvcPassword;-- and StatusId=@rvcStatusId;
 
	exec [dbo].[SG2_usp_Customers_Get] @customerid
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_PlanInformation_Delete]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_PlanInformation_Delete]
  @riPlanInformationId Int
As  
Begin
DECLARE @StripPlanId int
SELECT @StripPlanId=[StripePlanId]  FROM [dbo].[SG2_SocialProfile_PaymentPlan] where PlanId=@riPlanInformationId
if not exists(SELECT 1 from [dbo].[SG2_SocialProfile_Subscription] where [StripeSubscriptionId]=@StripPlanId)
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_PlanInformation_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_PlanInformation_GetAll]
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
	SELECT      [PlanId],
				[NoOfLikes] as Likes,
				[DisplayPrice] DisplayPrice,
				[PlanName],
				[PlanShortDescription] as PlanDescription,
				EV.[Name] as PlanType,
				[PlanTypeId] as PlanTypeId,
				[StripePlanId],
				[StripePlanPrice] as PlanPrice,
				[NoOfLikesDuration] as  NoOfLikesDuration,
				EV2.[Name] as [Status],
				[SortOrder] as SortOrder,
				EV3.[Name]      as SocialPlanType,
				  (Select Count(1) From [dbo].[SG2_SocialProfile_PaymentPlan]) TotalRecord,
				   ROW_NUMBER() OVER (ORDER BY  PlanId desc) AS RankId
	  FROM [dbo].[SG2_SocialProfile_PaymentPlan] LA
	  Left join [dbo].[SG2_EnumerationValue] EV on LA.PlanTypeId=EV.EnumerationValueId
	  Left Join [dbo].[SG2_EnumerationValue] EV2 on LA.StatusId=EV2.EnumerationValueId
	  Left join  [dbo].[SG2_EnumerationValue] EV3 on LA.SocialPlanTypeId=EV2.EnumerationValueId
		 Where (
			((@riStatusId is null)or LA.StatusId = @riStatusId)
		And ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (LA.PlanName like '%' +@rsSearchCrite +'%' or LA.PlanTypeId like '%' +@rsSearchCrite +'%' or LA.[NoOfLikes] like '%' +@rsSearchCrite +'%' )
			)
	)
	)

	SELECT  [PlanId] as PlanId,
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_PlanInformation_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_PlanInformation_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_Delete]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SG2_usp_Proxy_Delete]
  @riProxyId Int

As  
Begin
	If Not exists(
				Select 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM 
					inner join [dbo].[SG2_SocialProfile] SP 
						ON SP.[SocialProfileId] = PM.[SocialProfileId]
					Inner join [dbo].[SG2_Customer] C 
						ON  SP.CustomerId = C.CustomerId AND PM.ProxyId = @riProxyId AND C.StatusId = 5
			)
	  Begin
		UPDATE SG2_Proxy 
			SET StatusID = 4
		WHERE ProxyId = @riProxyId
	 End
    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Proxy_GetAll]
(
  @rsSearchCrite Nvarchar(MAX),
  @riPageNumber Int,
  @riPageSize varchar(8),
  @riStatusId int=null,
  @iSupplierId int=null
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


 Declare  @tbResult table(
 RowNumber     int ,
 ProxyId     int,
 ProxyIPNumber      nvarchar(250),
 FreeSlots    int,
 JVBoxes    nvarchar(250),
 Region      nvarchar(250),
 ProxyIPStatus nvarchar(50),
 VPSSId    int,
 VPSSName   nvarchar(250)
 ) 
 
 ;with cte As(
 SELECT DISTINCT
		       P.ProxyId,
				 ProxyIPNumber,
				 (3-(Select count(1) From [dbo].[SG2_SocialProfile_ProxyMapping] PV Where PV.ProxyId = P.ProxyId))  FreeSlots,
				 ( 
					stuff((
						Select ','+JV.BoxName FROM SG2_JVBox JV 
						inner join [dbo].[SG2_SocialProfile] SP ON SP.JVBoxId=JV.JVBoxId
						--inner join dbo.SG2_Customer C ON SP.CustomerId = C.CustomerId 
						inner JOIN [dbo].[SG2_SocialProfile_ProxyMapping] PM On P.ProxyId= PM.ProxyId AND pm.[SocialProfileId] = SP.[SocialProfileId]
						group by jv.BoxName
						for xml path ('')
					),1,1,'')
					
				) JVBoxes,				 
				 SCity.[Name] + ', '+ SC.[Name]  Region,
				 EV.[Name] as ProxyIPStatus,				 
				 VPSS.VPSSId as VPSSId,
				 VPSS.IssuingISPName as VPSSName,
				  ROW_NUMBER() OVER (ORDER BY P.ProxyId desc) AS RankId
  FROM dbo.SG2_Proxy P WITH (NOLOCK) 
  INNER JOIN dbo.SG2_SystemCountry SC ON SC.CountryId=P.BaseCountry
  INNER JOIN dbo.SG2_SystemCity SCity ON SCity.CityId=P.BaseCity
  INNER JOIN SG2_EnumerationValue EV ON P.StatusId=EV.EnumerationValueId
  INNER JOIN SG2_VPSSupplier VPSS on VPSS.VPSSId=P.VPSSId
  Where (
  (((@riStatusId is null) or @riStatusId=0 ) or  P.StatusId=@riStatusId))
  AND
    (((@iSupplierId is null) or @iSupplierId=0 ) or  VPSS.[VPSSId]=@iSupplierId)
  AND 
  (((@rsSearchCrite is null) or @rsSearchCrite ='' ) or P.ProxyIPNumber like '%' +@rsSearchCrite +'%')
  )

Insert into @tbResult
Select  ROW_NUMBER() OVER (ORDER BY ProxyId desc) AS RankId,
		ProxyId,  
		ProxyIPNumber,
		FreeSlots,
        JVBoxes,
	    Region,
		ProxyIPStatus,
		VPSSId,
		VPSSName
 From cte 

				
Select ProxyId,  
		ProxyIPNumber,
		FreeSlots,
        JVBoxes,
	    Region,
     (Select Count(1) From @tbResult) TotalRecord,
	 ProxyIPStatus,
	 VPSSId,
	 VPSSName
 From @tbResult 
 Where RowNumber Between @iFirstRow And @iLastRow
	
 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Proxy_GetById]
  @riProxyId Int

As  
Begin

DECLARE @iCustomerIds VARCHAR(MAX)
Declare @tblCustomersAndCity table (SocialProfileId int,CustomerName nvarchar(250),SUserName nvarchar(250), City varchar(50),RankId int)
Declare @AssignedCustomerID1 nvarchar(250),@AssignedCustomerID2 nvarchar(250),@AssignedCustomerID3 nvarchar(250),
		@AssignedCustomerID1City varchar(50), @AssignedCustomerID2City varchar(50), @AssignedCustomerID3City varchar(50)   

Insert into @tblCustomersAndCity(SocialProfileId,City,RankId,CustomerName,SUserName)
SELECT PM.SocialProfileId,SC.Name ,
 ROW_NUMBER() OVER (ORDER BY PM.SocialProfileId asc) AS RankId,C.[FirstName] +' '+ISNULL(C.SurName,''), SP.[SocialUsername]
FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM 
INNER JOIN [dbo].[SG2_SocialProfile] SP ON PM.[SocialProfileId]=SP.[SocialProfileId]
INNER JOIN [dbo].[SG2_Customer] C ON SP.CustomerId=C.CustomerId
LEFT JOIN [dbo].[SG2_SystemCity]  SC ON SC.CityId=SP.SocialPrefferedCity
where PM.ProxyId=@riProxyId 
Order by SP.CustomerId asc

SELECT @AssignedCustomerID1=CustomerName +' '+ SUserName,@AssignedCustomerID1City=City FROM @tblCustomersAndCity where RankId=1
SELECT @AssignedCustomerID2=CustomerName +' '+ SUserName,@AssignedCustomerID2City=City FROM @tblCustomersAndCity where RankId=2
SELECT @AssignedCustomerID3=CustomerName +' '+ SUserName,@AssignedCustomerID3City=City FROM @tblCustomersAndCity where RankId=3


SELECT    distinct  P.ProxyId,
		    ProxyIPNumber,
		    ProxyIPName,
		    BaseCity,
		    BaseCountry,
		    GeoPoints,
		    VPSSId as VPSSId,
			ISNULL(@AssignedCustomerID1,'') AssignedCustomerID1,
		    ISNULL(@AssignedCustomerID2,'') AssignedCustomerID2,
			ISNULL(@AssignedCustomerID3,'') AssignedCustomerID3,
			@AssignedCustomerID1City AssignedCustomerID1City,
			@AssignedCustomerID2City AssignedCustomerID2City,
			@AssignedCustomerID3City AssignedCustomerID3City,
			P.StatusId,
			P.ProxyPort as ProxyPort
  FROM [dbo].[SG2_Proxy]  P	
  WHERE P.ProxyId = @riProxyId


 
    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_GetbyProfileId]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Proxy_GetbyProfileId]
@riSpId int

 
As  
Begin

 -- Searches for Customers based on given parameters  
SELECT P.* FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM inner join 
             [dbo].[SG2_Proxy] P ON  P.[ProxyId]=PM.[ProxyId]
			 AND PM.[SocialProfileId]=@riSpId
 
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_IsCustomerIPExist]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Proxy_IsCustomerIPExist]
  @riProxyId Int

As  
Begin

  DECLARE @CustomerIPExists bit = 0

 IF EXISTS (SELECT 1 
  FROM [dbo].[SG2_SocialProfile_ProxyMapping]
  WHERE ProxyId = @riProxyId)
  BEGIN
    SET @CustomerIPExists = 1
  END
 
  select  @CustomerIPExists
    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Proxy_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_Proxy_Save]
(
	@riProxyId					INT,
	@riProxyIPNumber			NVARCHAR(15),
	@rvcProxyIPName				NVARCHAR(50),
	@rvcBaseCity				NVARCHAR(50),
	@rvcBaseCountry				NVARCHAR(50),
	@rvcGeoPoints				NVARCHAR(150),
	@riIssuingISPId			INT,
	@riSocailProfileID1		INT = null,
	@riSocalProfileID2		INT = null,
	@riSocailProfileID3		INT = null,
	@rvcProxyPort				NVARCHAR(50),
	@riStatusId					INT = 18
)
 
AS  
 
 
BEGIN
IF NOT EXISTS(SELECT 1 FROM [dbo].[SG2_Proxy] WHERE ProxyId = @riProxyId ) 
BEGIN
INSERT INTO [dbo].[SG2_Proxy]
           (
		    ProxyIPNumber,
		    ProxyIPName,
		    BaseCity,
		    BaseCountry,
		    GeoPoints,
			StatusId,
			VPSSId,
			ProxyPort
			)
     VALUES
	 (
		   @riProxyIPNumber,
		   @rvcProxyIPName,
		   @rvcBaseCity,
		   @rvcBaseCountry,
		   @rvcGeoPoints,
		   @riStatusId,
		   @riIssuingISPId,
		   @rvcProxyPort
		 )

		 Select @riProxyId = SCOPE_IDENTITY() 
  
END
ELSE
BEGIN
UPDATE [dbo].[SG2_Proxy]
   SET
		    ProxyIPNumber =@riProxyIPNumber,
		    ProxyIPName = @rvcProxyIPName,
		    BaseCity = @rvcBaseCity,
		    BaseCountry = @rvcBaseCountry,
		    GeoPoints = @rvcGeoPoints,
			StatusId = @riStatusId,
			VPSSId=@riIssuingISPId,
			ProxyPort=@rvcProxyPort
 WHERE  ProxyId= @riProxyId

END

	IF(@riSocailProfileID1 IS NOT NULL)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping] WITH(NOLOCK) WHERE ProxyId = @riProxyID AND [SocialProfileId] = @riSocailProfileID1 )
			BEGIN
				INSERT INTO [dbo].[SG2_SocialProfile_ProxyMapping]([ProxyId],[SocialProfileId])
				VALUES(@riProxyId,@riSocailProfileID1)
			END
	END

	IF(@riSocalProfileID2 IS NOT NULL)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping]  WITH(NOLOCK) WHERE ProxyId = @riProxyID and [SocialProfileId] = @riSocalProfileID2 )
			BEGIN
				INSERT INTO [dbo].[SG2_SocialProfile_ProxyMapping]([ProxyId],[SocialProfileId])
				VALUES(@riProxyId,@riSocalProfileID2)
			END
	END
	IF(@riSocailProfileID3 IS NOT NULL)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping]  WITH(NOLOCK) WHERE ProxyId = @riProxyID and [SocialProfileId] = @riSocailProfileID3)
			BEGIN
				INSERT INTO [dbo].[SG2_SocialProfile_ProxyMapping]([ProxyId],[SocialProfileId])
				VALUES(@riProxyId,@riSocailProfileID3)
			END
	END


DELETE FROM [dbo].[SG2_SocialProfile_ProxyMapping] Where ProxyId = @riProxyId 
AND [SocialProfileId] NOT IN (@riSocailProfileID1,@riSocalProfileID2,@riSocailProfileID3)


SELECT      ProxyId,
		    ProxyIPNumber,
		    ProxyIPName,
		    BaseCity,
		    BaseCountry,
		    GeoPoints,
			@riSocailProfileID1 as SocailProfileID1,
			@riSocalProfileID2 as SocalProfileID2,
			@riSocailProfileID3 as SocailProfileID3,
			@rvcProxyPort As ProxyPort
  FROM [dbo].[SG2_Proxy]  WHERE ProxyId = @riProxyId
					
         
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_ProxyIP_GetGeoPoints]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SG2_usp_ProxyIP_GetGeoPoints]


 
As  
Begin

 SELECT Distinct 
[ProxyId],
SGC.[Name] + '   ' + SG.[Name]  as FullCityCountryName
 FROM SG2_Proxy P 
 Inner join SG2_SystemCountry SGC ON P.BaseCountry=SGC.CountryId
 inner join SG2_SystemCity SG on P.BaseCity=SG.CityId
 where [GeoPoints] is null  or GeoPoints = ''
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_ProxyIP_SaveGeoPoints]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SG2_usp_ProxyIP_SaveGeoPoints]
(
	@rvcProxyXML   XML
)
AS  
BEGIN

DECLARE @tblresult Table(
GeoPoint Nvarchar(50),
ProxyId int
)
Insert into @tblresult(ProxyId,GeoPoint)
 Select nref.value('ProxyId[1]', 'INT'),
     nref.value('GeoPoint[1]', 'Nvarchar(50)')
   From @rvcProxyXML.nodes('/Proxes/Proxy') AS r(nref)

   update P
   set P.GeoPoints=tb.GeoPoint
   FROM 
   [dbo].[SG2_Proxy] P inner join @tblresult tb
   ON P.ProxyId=tb.ProxyId
   where P.GeoPoints is null or P.GeoPoints = ''
    
	SELECT @@ERROR
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_ProxyMapping_Insert]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_ProxyMapping_Insert]
  @riProfileId int,
  @riProxyId Int

As  
Begin
--If exists(Select 1 FROM [dbo].[SG2_SocialProfile_ProxyMapping] PM 
--inner join [dbo].[SG2_SocialProfile] SP ON SP.[SocialProfileId]=PM.[SocialProfileId]
--											AND  PM.ProxyId=@riProxyId)
 Begin

 Delete from [dbo].[SG2_SocialProfile_ProxyMapping] where --[ProxyId]=@riProxyId
														--and 
														[SocialProfileId]=@riProfileId
if(@riProxyId is not Null)
BEGIN
  insert into [dbo].[SG2_SocialProfile_ProxyMapping]
  SELECT @riProxyId,@riProfileId
END
 End
    
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_QueueAudit_Detail_InsertLog]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_QueueAudit_Detail_InsertLog]
@rTransactionId  nvarchar(32) ,
@rStepName nvarchar(50) ,
@rStepDetail nvarchar(max) ,
@rStepStatus int ,
@rStepError nvarchar(max) ,
@rCreatedDate datetime ,
@rCreatedBy varchar(50) ,
@rBase64Image varbinary(MAX) =null
As  
Begin

	
INSERT INTO [dbo].[SG2_QueueAuditDetail]
           ([TransactionId]
           ,[StepName]
           ,[StepDetail]
           ,[StepStatus]
           ,[StepError]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[Base64Image])

SELECT   
@rTransactionId  ,
@rStepName  ,
@rStepDetail  ,
@rStepStatus  ,
@rStepError  ,
@rCreatedDate  ,
@rCreatedBy ,
@rBase64Image  
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_QueueAudit_GetDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_QueueAudit_GetDetail]
@rJVServerId int =null

As  
Begin
	
	SELECT 
		SGQA.[CreatedDate],
		SGQA.[QueueAction],
		EV2.Name [QueueType],
		EV.Name [QueueStatus],
		SGQA.[TransactionId],
		SP.[SocialUsername],
		(SELECT COUNT(*) from SG2_QueueAudit QA where [QueueStatus]=48 and QA.[JVServerId]= ISNULL(@rJVServerId,SGQA.[JVServerId])) as TotalError,
		(SELECT COUNT(*) from SG2_QueueAudit QA where [QueueStatus]=43 and QA.[JVServerId]= ISNULL(@rJVServerId,SGQA.[JVServerId])) as TotalPending,
		(SELECT COUNT(*) from SG2_QueueAudit QA where [QueueStatus]=44 and QA.[JVServerId]= ISNULL(@rJVServerId,SGQA.[JVServerId])) as TotalInProgress
	FROM [dbo].[SG2_QueueAudit] SGQA
		 LEFT JOIN [dbo].[SG2_EnumerationValue] EV ON SGQA.QueueStatus=EV.[EnumerationValueId]
		 LEFT JOIN [dbo].[SG2_EnumerationValue] EV2 ON SGQA.QueueType=EV2.[EnumerationValueId]
		 LEFT JOIN [dbo].[SG2_SocialProfile] SP ON SP.SOCIALPROFILEID = SGQA.PROFILEID
	WHERE (SGQA.[JVServerId]= @rJVServerId )
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_QueueAudit_GetimageData]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_QueueAudit_GetimageData]
@rTransactionId varchar(250)

As  
Begin
	
SELECT top 1 [Base64Image] FROM  [dbo].[SG2_QueueAuditDetail] where [QueueAuditDetailId]=@rTransactionId
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_QueueAudit_InsertLog]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_QueueAudit_InsertLog]
@rTransactionId nvarchar(32) ,
@rQueueType smallint ,
@rQueueStatus smallint ,
@rQueueData nvarchar(max) ,
@rErrorDescription nvarchar(max) ,
@rProfileId int ,
@rJVBoxData nvarchar(max) ,
@rCreatedDate datetime ,
@rCreatedBy nvarchar(50) ,
@rModifiedDate datetime ,
@rModifiedBy nvarchar(50) ,
@rNoOfAttempts int,
@rQueueAction nvarchar(50),
@rJVServerId int
As  
Begin

	
INSERT INTO [dbo].[SG2_QueueAudit]
           ([TransactionId]
           ,[QueueType]
           ,[QueueStatus]
           ,[QueueData]
           ,[ErrorDescription]
           ,[ProfileId]
           ,[JVBoxData]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy]
           ,[NoOfAttempts]
		   ,[QueueAction]
		   ,[JVServerId])

SELECT   
@rTransactionId  ,
@rQueueType  ,
@rQueueStatus  ,
@rQueueData  ,
@rErrorDescription  ,
@rProfileId  ,
@rJVBoxData  ,
@rCreatedDate  ,
@rCreatedBy  ,
@rModifiedDate  ,
@rModifiedBy  ,
@rNoOfAttempts  ,
@rQueueAction,
@rJVServerId
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_QueueAuditDetail_GetDetail]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_QueueAuditDetail_GetDetail]
@rTransactionId nvarchar(32)

As  
Begin
	
SELECT 
		[QueueAuditDetailId]
		,[TransactionId]
		,[StepName]
		,[StepDetail]
		,[StepStatus]
		,[StepError]
		,[CreatedDate]
		,[CreatedBy]
		,
		Case when [Base64Image] IS null
		Then 0
		else 1 End IsImageExists,
		'' [Base64Image]
FROM [dbo].[SG2_QueueAuditDetail] SGQADetail with (Nolock)
where [TransactionId]=@rTransactionId
 
         
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Report_GetJVBoxandProxyIPsData]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Report_GetJVBoxandProxyIPsData]
(
  @dtFromDate Date = null,
  @dtToDate   Date = null
)
As
Begin	
SET FMTONLY OFF;

		 DECLARE  -- JV Box
		   @iAllSlotsOnJVBox BIGINT,
		   @iUsedSlotsOnJVBox BIGINT,
		   

		   -- Proxy IPs
		   @iAllAvailableIPs BIGINT,
		   @iTotalUsedIPs BIGINT
		
		  

			-- JV Box
			SELECT @iAllSlotsOnJVBox = SUM(MaxLimit) 
			From [dbo].[SG2_JVBox] WITH(NOLOCK)
		

			SELECT @iUsedSlotsOnJVBox = COUNT(SocialProfileId) 
			FROM [dbo].[SG2_SocialProfile] 
			WITH(NOLOCK) Where JVBoxId IS NOT NULL
			AND CAST(CreatedOn AS DATE) BETWEEN @dtFromDate AND @dtToDate
		  
			-- Proxy IPs
		
			
			SELECT @iTotalUsedIPs = COUNT(ProxyId) 
			FROM [dbo].[SG2_Proxy] WITH(NOLOCK)

			SELECT @iAllAvailableIPs = COUNT(*) FROM 
			[dbo].[SG2_SocialProfile_ProxyMapping] WITH(NOLOCK)


		     SELECT 
			  CAST(ISNULL(@iAllSlotsOnJVBox,0) AS BIGINT ) AS AllSlotsOnJVBox,
			  CAST(ISNULL(@iUsedSlotsOnJVBox,0) AS BIGINT ) AS UsedSlotsOnJVBox,
			  CAST((ISNULL(@iAllSlotsOnJVBox,0) - ISNULL(@iUsedSlotsOnJVBox,0))AS BIGINT ) AS FreeSlotsOnJVServer,
			  CAST(ISNULL(@iAllAvailableIPs,0)AS BIGINT ) AS AllProxyIPs,
			  CAST(ISNULL(@iTotalUsedIPs,0)AS BIGINT ) AS  UsedProxyIPs,
			  (ISNULL(@iAllAvailableIPs,0) -  ISNULL(@iTotalUsedIPs,0)) AS RemainingProxyIPs

	

			
	-- GROUP BY [SocialProfileId], [Username],[Date]
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Report_GetMostUsedProductData]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
			FROM [dbo].[SG2_SocialProfile_Subscription] t
			WHERE T.StartDate BETWEEN @dtFromDate AND @dtToDate

			SELECT S.Name as PlanName, COUNT(S.PaymentPlanID) PlanSold, @iTotalPlan as TotalPlanSold
			FROM [dbo].[SG2_SocialProfile_Subscription] S 
			WHERE S.StartDate BETWEEN @dtFromDate AND @dtToDate
			GROUP BY S.Name


End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Report_GetReportData]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
			From [dbo].[SG2_SocialProfile_Subscription] S 
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_Set_ProfileJVAttempts]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_Set_ProfileJVAttempts]
  @riJVAttempts int,
  @rdtJVAttemptsBlockedTill datetime,
  @riJVAttemptStatus int,
  @riProfileId int
As  
Begin

	Update  [dbo].[SG2_SocialProfile]
	set 
	[JVAttempts]           =@riJVAttempts,
	[JVAttemptsBlockedTill]=@rdtJVAttemptsBlockedTill, 	
	[JVAttemptStatus]	    =@riJVAttemptStatus ,
	UpdatedOn=GETDATE()
	where [SocialProfileId]=@riProfileId;

	SELECT 
         [JVAttempts]          , 
		 [JVAttemptsBlockedTill],
		 [JVAttemptStatus]	   
		 from [dbo].[SG2_SocialProfile]
		 where [SocialProfileId]=@riProfileId;
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_BadProxy]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_GetNotificationsByStatus]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	from [dbo].[SG2_SocialProfile_Notification] nt
		inner join [dbo].[SG2_SocialProfile] sp
			on nt.socialprofileid = sp.socialprofileid
		Inner Join [dbo].[SG2_Customer] c
			on c.customerid = sp.customerid
	where nt.StatusId  in (51,52)
		order by createdOn desc
END
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_SaveStatistics]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_Statistics_GetFollowers_Old]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_Statistics_GetStatistics]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SocialProfile_Subscription_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_Subscription_Save]
(
	@riSocialProfileId					INT,
	@riStripeSubscriptionId				NVARCHAR(255),
	@rvcDescription						NVARCHAR(250),
	@rvcName							NVARCHAR(255),
	@riPrice							Decimal(18,2),
	@riStripePlanId						NVARCHAR(255),
	@rvcSubscriptionType				NVARCHAR(255),
	@rdtStartDate						datetime,
	@rdtEndDate							datetime,
	@riStatusId							Int,
	@riPaymentPlanId					Int,
	@rvcStripeInvoiceId					NVARCHAR(255)
)
 
AS  
 
 
BEGIN
	  declare @subId int;

      Update [dbo].[SG2_SocialProfile_Subscription]
	  Set StatusId=27
	  where [SocialProfileId]=@riSocialProfileId
	  AND StatusId not in (26,27)

		BEGIN
			INSERT INTO [dbo].[SG2_SocialProfile_Subscription]
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
							(select top 1 [PlanId] from [dbo].[SG2_SocialProfile_PaymentPlan] where [StripePlanId] = @riStripePlanId),
						@rvcStripeInvoiceId
						 )
			END

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
  FROM [dbo].[SG2_SocialProfile_Subscription]
	where [SubscriptionId] = @subId
		
END
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_Supplier_Delete]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE Procedure [dbo].[SG2_usp_Supplier_Delete]
@riSSPId int

As  
Begin

IF not exists(SELECT 1 from [dbo].[SG2_Proxy] where [VPSSId]=@riSSPId )
BEGIN 
DELETE FROM [dbo].[SG2_VPSSupplier] where [VPSSId]=@riSSPId
END
ELSE
BEGIN
Update [dbo].[SG2_VPSSupplier] set [StatusId]=18 where [VPSSId]=@riSSPId
END


Return 1
     
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemConfig_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
     (Select Count(1) From [dbo].[SG2_SystemConfig]) TotalRecord
from [dbo].[SG2_SystemConfig] SC
Where  ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (SC.ConfigKey like '%' +@rsSearchCrite +'%' or SC.ConfigValue like '%' +@rsSearchCrite +'%' or SC.ConfigValue2 like '%' +@rsSearchCrite +'%' )
			)


 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemConfig_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemConfig_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemRole_AllRole]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SG2_usp_SystemRole_AllRole]

As  
Begin


  Select [RoleId],[Name] From [dbo].[SG2_SystemRole] Where StatusId = 19 and RoleId  <> 1 -- Super Admin

 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemUser_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
     (Select Count(1) From [dbo].[SG2_SystemConfig]) TotalRecord
from [dbo].SG2_SystemUser SU
		  INNER JOIN SG2_SystemRole SR ON SU.SystemRoleID = SR.RoleId
		  INNER JOIN [dbo].SG2_EnumerationValue EV ON SU.StatusId= EV.EnumerationValueId
		  INNER JOIN [dbo].SG2_Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'SystemUser'
Where SU.HostUser = 0
	AND	( 
			(@riStatusId is null) or SU.StatusId = @riStatusId)
			AND	((@rsSearchCrite is null or @rsSearchCrite = '') 
				or (SU.FirstName like '%' +@rsSearchCrite +'%' 
				or SU.LastName like '%' +@rsSearchCrite +'%' 
				  ))


 Return @@Error 
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemUser_GetById]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemUser_Login]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
FROM  [dbo].SG2_SystemUser SU
   INNER JOIN [dbo].[SG2_SystemRole] SR ON SU.SystemRoleId = SR.RoleId
WHERE Email = @rvcEmailAddress AND PASSWORD= @rvcPassword 
 
 
    
End

GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_SystemUser_Save]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[SG2_usp_VPSSupplier_BulkInsert]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SG2_usp_VPSSupplier_BulkInsert]
(
	@rvcProxyXML   XML

)
 
AS  
BEGIN

	DECLARE @tblresult Table(
		City Nvarchar(50),
		ProxyUserName Nvarchar(50),
		Country Nvarchar(50),
		Host Nvarchar(50),
		HostWebsite Nvarchar(50),
		ProxyPassword Nvarchar(50),
		[IP] Nvarchar(250),
		ProxyPort  Nvarchar(50),
		CountryId int,
		CityId int,
		IsCountryExists Bit default(0),
		IsCityExists Bit default(0),
		IsSupplierExists Bit default(0),
		[VPSSId]  int
	)


	Insert into @tblresult(City,ProxyUserName,Country,Host,HostWebsite,ProxyPassword,[IP],ProxyPort)
	 Select nref.value('City[1]', 'Nvarchar(50)'),
		 nref.value('ProxyUserName[1]', 'Nvarchar(50)'),
		  nref.value('Country[1]', 'Nvarchar(50)'),
		   nref.value('Host[1]', 'Nvarchar(50)'),
			nref.value('HostWebsite[1]', 'Nvarchar(50)'),
			 nref.value('ProxyPassword[1]', 'Nvarchar(50)'),
			  nref.value('IP[1]', 'Nvarchar(250)'),
			   nref.value('ProxyPort[1]', 'Nvarchar(50)')
	   From @rvcProxyXML.nodes('/Proxes/Proxy') AS r(nref)

	 Update tbl
		set tbl.IsCountryExists=1,
			tbl.CountryId=SC.CountryId
		from @tblresult tbl inner join [dbo].[SG2_SystemCountry] SC
		ON Upper(tbl.Country)=Upper(SC.[Name])

	Update tbl
		set tbl.IsCityExists=1,
			tbl.CityId=S.CityId
		from 
			@tblresult tbl inner join [dbo].[SG2_SystemCity] S
		ON Upper(tbl.City)=Upper(S.[Name])

	--SELECT '@tblresult-1',* FROM @tblresult
	If exists(SELECT 1 FROM @tblresult where CountryId is null)
	Begin
		INSERT INTO [dbo].[SG2_SystemCountry]([Name],[StatusId])
		SELECT Country,1
			From @tblresult where  CountryId is null
	END

	Update tbl
		set tbl.IsCountryExists=1,
			tbl.CountryId=SC.CountryId
		from @tblresult tbl inner join [dbo].[SG2_SystemCountry] SC
			ON Upper(tbl.Country)=Upper(SC.[Name])

--SELECT '@tblresult-2',* FROM @tblresult
	If exists(SELECT 1 FROM @tblresult where CityId is null)
	Begin
		--SELECT 'Test'
		INSERT INTO [dbo].[SG2_SystemCity]([Name],[CountryId],[StatusId])
		SELECT City,CountryId,1
			From @tblresult where  CityId is null
	END

	Update tbl
		set tbl.IsCityExists=1,
			tbl.CityId=S.CityId
		from @tblresult tbl inner join [dbo].[SG2_SystemCity] S
	ON Upper(tbl.City)=Upper(S.[Name])


	Update tbl
		set 
			tbl.VPSSId=S.VPSSId
		from @tblresult tbl left join [dbo].[SG2_VPSSupplier] S
	On tbl.Host=S.IssuingISPName

	Update S
		Set S.IssuingISPWebsite=tbl.HostWebsite,
			IssuingISPPassword=ProxyPassword
		from @tblresult tbl inner join [dbo].[SG2_VPSSupplier] S
	On tbl.VPSSId=S.VPSSId

	INSERT INTO [dbo].[SG2_VPSSupplier](
		[IssuingISPName],
		[IssuingISPWebsite],
		[IssuingISPPassword],
		[StatusId]
	)
	select Distinct Host,HostWebsite,ProxyPassword,19 from @tblresult tbl where tbl.VPSSId is null

	Update tbl
	set tbl.IsSupplierExists=1,
		tbl.VPSSId=S.VPSSId
	from @tblresult tbl inner join [dbo].[SG2_VPSSupplier] S
	On tbl.Host=S.IssuingISPName
	
	Insert into [dbo].[SG2_Proxy]
	(
		[ProxyIPNumber],
		[ProxyIPName],
		[BaseCity],
		[BaseCountry],
		[StatusId],
		[VPSSId],
		[ProxyPort]
	)
		SELECT [IP],
				ProxyUserName +','+ProxyPassword,
				CityId,
				CountryId,
				19,
				VPSSId,
				ProxyPort
		FROM @tblresult 
			Where ip not in (select distinct proxyipnumber from [dbo].[SG2_Proxy])

	SELECT Distinct 
		[ProxyId],
		SGC.[Name] + '   ' + SG.[Name]  as FullCityCountryName
		FROM SG2_Proxy P 
			Inner join SG2_SystemCountry SGC ON P.BaseCountry=SGC.CountryId
			inner join SG2_SystemCity SG on P.BaseCity=SG.CityId
		 where [GeoPoints]   is null  
	End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_VPSSupplier_GetAll]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_VPSSupplier_GetAll]
(
 @rsSearchCrite Nvarchar(MAX),
  @riPageNumber Int,
  @riPageSize varchar(8),
  @riStatusId int=null
)
 
AS  
 
 
BEGIN

-- Searches for Products based on given parameters  
 Declare @iFirstRow Int    
 Declare @iLastRow Int 

 Declare @xmlSearchCriteria Xml
 
 
 --Set @xmlSearchCriteria = dbo.udf_UTL_UpdateWildcardInput(@rsSearchCriteria)

 Set @iFirstRow  = (@riPageNumber-1) * @riPageSize + 1    
 Set @iLastRow = @riPageSize + @iFirstRow - 1 


 Declare  @tbResult table(
 RowNumber     int ,
 VPSId     int,
 ISPName      nvarchar(250),
 NoOfAssignedIP int,
 StatusName    nvarchar(50)
 ) 
 
 ;with cte As(
 SELECT DISTINCT
		         VPS.VPSSId,
				 VPS.[IssuingISPName],
				 ((Select count(1) From [dbo].[SG2_Proxy] P Where P.VPSSId = VPS.[VPSSId]))  NoofAssignedIPs,		 
				 EV.[Name] as VPSSIdStatus,
				  ROW_NUMBER() OVER (ORDER BY VPS.[VPSSId] desc) AS RankId
  FROM [dbo].[SG2_VPSSupplier] VPS WITH (NOLOCK) 
  Left JOIN SG2_EnumerationValue EV ON VPS.StatusId=EV.EnumerationValueId
  Where (
  (((@riStatusId is null) or @riStatusId=0 ) or  VPS.StatusId=@riStatusId))
  AND 
  (((@rsSearchCrite is null) or @rsSearchCrite ='' ) or VPS.[IssuingISPName] like '%' +@rsSearchCrite +'%')
  )

Insert into @tbResult(RowNumber,VPSId,ISPName,NoOfAssignedIP,StatusName)
Select  ROW_NUMBER() OVER (ORDER BY [VPSSId] desc) AS RankId,
		VPSSId,  
		[IssuingISPName],
        NoofAssignedIPs,
	    VPSSIdStatus
 From cte 

				
Select  VPSId,  
		ISPName,
		NoOfAssignedIP,
		StatusName,
     (Select Count(1) From @tbResult) TotalRecord
 From @tbResult 
 Where RowNumber Between @iFirstRow And @iLastRow
	
 Return @@Error 
 
         
End
GO
/****** Object:  StoredProcedure [dbo].[SG2_usp_VPSSupplier_SaveUpdate]    Script Date: 10/5/2019 1:50:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SG2_usp_VPSSupplier_SaveUpdate]
(
	@riVPSSId					INT,
	@rvcIPManageBy				NVARCHAR(50),
	@rvcSupportPhone			NVARCHAR(50),
	@rvcSupportEmail			NVARCHAR(50),
	@rvcIssuingISPName			NVARCHAR(50),
	@rvcIssuingISPPhone			NVARCHAR(50),
	@rvcIssuingISPWebsite		NVARCHAR(50),
	@rvcIssuingISPAccount		NVARCHAR(50),
	@rvcIssuingISPPassword		NVARCHAR(50),
	@rvcIssuingISPMemo			NVARCHAR(50),
	@riStatusId					INT = 18
)
 
AS  
 
 
BEGIN
IF NOT EXISTS(SELECT 1 FROM [dbo].SG2_VPSSupplier WHERE VPSSId = @riVPSSId ) 
BEGIN
INSERT INTO [dbo].[SG2_VPSSupplier]
           (
		    IPManageBy,
		    SupportPhone,
		    SupportEmail,
		    IssuingISPName,
		    IssuingISPPhone,
		    IssuingISPWebsite,
		    IssuingISPAccount,
		    IssuingISPPassword,
		    IssuingISPMemo,
			StatusId
			)
     VALUES
	 (	   @rvcIPManageBy,
		   @rvcSupportPhone,
		   @rvcSupportEmail,
		   @rvcIssuingISPName,
		   @rvcIssuingISPPhone,
		   @rvcIssuingISPWebsite,
		   @rvcIssuingISPAccount,
		   @rvcIssuingISPPassword,
		   @rvcIssuingISPMemo,
		   @riStatusId   
		 )

		 Select @riVPSSId = SCOPE_IDENTITY() 
  
END
ELSE
BEGIN
UPDATE [dbo].[SG2_VPSSupplier]
   SET
		    IPManageBy = @rvcIPManageBy,
		    SupportPhone = @rvcSupportPhone,
		    SupportEmail = @rvcSupportEmail,
		    IssuingISPName = @rvcIssuingISPName,
		    IssuingISPPhone = @rvcIssuingISPPhone,
		    IssuingISPWebsite = @rvcIssuingISPWebsite,
		    IssuingISPAccount = @rvcIssuingISPAccount,
		    IssuingISPPassword = @rvcIssuingISPPassword,
		    IssuingISPMemo = IssuingISPMemo,
			StatusId = @riStatusId
 WHERE  VPSSId= @riVPSSId

END

SELECT      VPSSId,
		    IPManageBy,
		    SupportPhone,
		    SupportEmail,
		    IssuingISPName,
		    IssuingISPPhone,
		    IssuingISPWebsite,
		    IssuingISPAccount,
		    IssuingISPPassword,
		    IssuingISPMemo,
			StatusId
  FROM [dbo].[SG2_VPSSupplier]  WHERE VPSSId= @riVPSSId
					
 
  --SELECT DISTINCT
		--       P.ProxyId,
		--		 ProxyIPNumber,
		--		 (3-(Select count(1) From SG2_Customer_ProxyMapping PV Where PV.ProxyId = P.ProxyId))  FreeSlots,
		--		 ( 
		--			stuff((
		--				Select ','+JV.BoxName FROM SG2_JVBox JV 
		--				inner join dbo.SG2_Customer C ON C.JVBoxId = jv.JVBoxId 
		--				inner JOIN dbo.SG2_Customer_ProxyMapping PM On P.ProxyId= PM.ProxyId AND pm.CustomerId = c.CustomerId
		--				group by jv.BoxName
		--				for xml path ('')
		--			),1,1,'')
					
		--		) JVBoxes,				 
		--		 SCity.[Name] + ', '+ SC.[Name]  Region,
		--		 EV.[Name] as ProxyIPStatus,
		--		 VPSS.VPSSId
		--		  as VPSSId,
		--		 VPSS.IssuingISPName as VPSSName
  --FROM dbo.SG2_Proxy P WITH (NOLOCK) 
  --INNER JOIN dbo.SG2_SystemCountry SC ON SC.CountryId=P.BaseCountry
  --INNER JOIN dbo.SG2_SystemCity SCity ON SCity.CityId=P.BaseCity
  --Left JOIN SG2_EnumerationValue EV ON P.StatusId=EV.EnumerationValueId
  --Left JOIN SG2_VPSSupplier VPSS on VPSS.VPSSId=P.VPSSId
  --Where VPSS.VPSSId= @riVPSSId
  --Order by VPSS.VPSSId desc
 
         
End
GO
ALTER DATABASE [DB_129180_socialgrowth] SET  READ_WRITE 
GO
