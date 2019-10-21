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
FROM  [dbo].SystemUser SU  
   INNER JOIN [dbo].[SystemRole] SR ON SU.SystemRoleId = SR.RoleId  
WHERE Email = @rvcEmailAddress AND PASSWORD= @rvcPassword   
   
   
      
End  
  