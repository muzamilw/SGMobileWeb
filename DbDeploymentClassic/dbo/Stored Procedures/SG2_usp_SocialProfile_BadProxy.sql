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
					
         
