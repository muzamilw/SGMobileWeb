  
CREATE Procedure [dbo].[SG2_usp_Get_AllUser]  
   
As    
Begin  
  
SELECT Distinct [SystemUserId] as UserId,FirstName as UserName,SR.[Name] as RoleName  FROM [dbo].[SystemUser] SU  
        LEFT JOIN SystemRole SR ON SU.SystemRoleId=SR.RoleId  
    
End  
  