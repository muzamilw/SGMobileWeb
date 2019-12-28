  
CREATE Procedure [dbo].[SG2_usp_LikeyAccount_GetAll]  
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
   
 ;with cte as (  
 SELECT       LikeyAccountId,  
     InstaUserName,  
      EV.Name StatusName,  
      (Select Count(1) From [dbo].LikeyAccount) TotalRecord,  
       ROW_NUMBER() OVER (ORDER BY  LikeyAccountId desc) AS RankId  
   FROM [dbo].LikeyAccount LA  
      INNER JOIN [dbo].EnumerationValue EV ON LA.StatusId= EV.EnumerationValueId  
   INNER JOIN [dbo].Enumeration E ON EV.EnumerationId= E.EnumerationId AND E.Name = 'General'  
   Where (  
   ((@riStatusId is null) or LA.StatusId = @riStatusId)  
  And (   
    (@rsSearchCrite is null or @rsSearchCrite = '')   
    or (LA.InstaUserName like '%' +@rsSearchCrite +'%' or LA.Country like '%' +@rsSearchCrite +'%' or EV.Name like '%' +@rsSearchCrite +'%' )  
   )  
 )  
 )  
  
 SELECT LikeyAccountId,  
     InstaUserName,  
     StatusName,  
     TotalRecord  
 FROM cte where RankId Between @iFirstRow And @iLastRow  
  
  
 Return @@Error   
End  