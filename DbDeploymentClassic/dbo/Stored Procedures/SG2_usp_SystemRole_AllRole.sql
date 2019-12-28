
CREATE Procedure [dbo].[SG2_usp_SystemRole_AllRole]

As  
Begin


  Select [RoleId],[Name] From [dbo].[SG2_SystemRole] Where StatusId = 19 and RoleId  <> 1 -- Super Admin

 Return @@Error 
End
