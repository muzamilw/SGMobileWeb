
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
