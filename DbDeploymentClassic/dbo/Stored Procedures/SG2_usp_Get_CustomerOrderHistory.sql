
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

