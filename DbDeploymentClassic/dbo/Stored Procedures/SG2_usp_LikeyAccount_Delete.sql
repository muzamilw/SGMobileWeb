
CREATE Procedure [dbo].[SG2_usp_LikeyAccount_Delete]
  @riLikeyAccountId Int
As  
Begin

   UPDATE SG2_LikeyAccount 
   SET StatusID = 18
   WHERE LikeyAccountId = @riLikeyAccountId
 
 Return 1;
    
End
