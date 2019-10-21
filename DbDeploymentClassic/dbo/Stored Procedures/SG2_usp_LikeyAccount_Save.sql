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
