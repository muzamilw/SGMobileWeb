CREATE Procedure [dbo].[SG2_usp_LikeyAccount_GetById]
  @riLikeyAccountId Int

As  
Begin

SELECT LikeyAccountId, 
	  InstaUserName, 
	  InstaPassword, 
	  Country, 
	  City, 
	  Gender, 
	  HashTag,
	  [StatusId]
FROM [dbo].SG2_LikeyAccount
Where LikeyAccountId = @riLikeyAccountId

End
