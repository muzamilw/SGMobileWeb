CREATE PROCEDURE [dbo].[SG2_usp_SystemConfig_Save]
(
	@riConfigId			smallint,
	@rvcConfigValue		nvarchar(250),
	@rvcConfigValue2	nvarchar(250),
	@rdtModifiedOn		datetime,
	@rvcModifiedBy		nvarchar(50)
)
 
AS  
 
 
BEGIN
	
		UPDATE  [dbo].[SG2_SystemConfig]
		   SET
					[ConfigValue] = @rvcConfigValue,
					[ConfigValue2] = @rvcConfigValue2,
					[ModifiedOn] = @rdtModifiedOn,
					[ModifiedBy] = @rvcModifiedBy
		 WHERE ConfigId = @riConfigId
					    
End
