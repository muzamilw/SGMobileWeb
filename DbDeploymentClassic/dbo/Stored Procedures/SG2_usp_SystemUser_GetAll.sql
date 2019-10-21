  
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
     (Select Count(1) From [dbo].[SystemUser]) TotalRecord  
from [dbo].SystemUser SU  
    INNER JOIN SystemRole SR ON SU.SystemRoleID = SR.RoleId  
    INNER JOIN [dbo].EnumerationValue EV ON SU.StatusId= EV.EnumerationValueId  
    INNER JOIN [dbo].Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'SystemUser'  
Where SU.HostUser = 0  
 AND (   
   (@riStatusId is null) or SU.StatusId = @riStatusId)  
   AND ((@rsSearchCrite is null or @rsSearchCrite = '')   
    or (SU.FirstName like '%' +@rsSearchCrite +'%'   
    or SU.LastName like '%' +@rsSearchCrite +'%'   
      ))  
  
  
 Return @@Error   
End  