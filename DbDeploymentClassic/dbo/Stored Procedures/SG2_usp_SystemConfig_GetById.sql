
CREATE Procedure [dbo].[SG2_usp_SystemConfig_GetById]
  @riConfigId smallint

As  
Begin

   SELECT    [ConfigId],
			 [ConfigKey], 
			 [ConfigValue], 
			 [ConfigValue2], 
			 [CreatedOn], 
			 [CreatedBy], 
			 [ModifiedOn], 
			 [ModifiedBy]
  FROM [dbo].[SG2_SystemConfig]
  WHERE [ConfigId] = @riConfigId

    
End
