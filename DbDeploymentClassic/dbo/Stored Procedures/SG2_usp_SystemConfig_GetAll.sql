
CREATE Procedure [dbo].[SG2_usp_SystemConfig_GetAll]
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
        [ConfigId],
		[ConfigKey], 
		[ConfigValue] as ConfigValue1,
		[ConfigValue2] as ConfigValue2,
     (Select Count(1) From [dbo].[SystemConfig]) TotalRecord
from [dbo].[SystemConfig] SC
Where  ( 
				(@rsSearchCrite is null or @rsSearchCrite = '') 
				or (SC.ConfigKey like '%' +@rsSearchCrite +'%' or SC.ConfigValue like '%' +@rsSearchCrite +'%' or SC.ConfigValue2 like '%' +@rsSearchCrite +'%' )
			)


 Return @@Error 
End
