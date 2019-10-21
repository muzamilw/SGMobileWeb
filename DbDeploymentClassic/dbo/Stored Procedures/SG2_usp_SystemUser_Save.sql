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
