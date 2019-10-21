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
