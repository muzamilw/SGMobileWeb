CREATE Procedure [dbo].[SG2_usp_Login_Customers]  
  @rvcEmailAddress Nvarchar(64),  
  @rvcPassword Nvarchar(64),    
  @rvcCreatedBy  Nvarchar(64),  
  @rvcLastLoginIP Nvarchar(20),  
  @rvcStatusId Int=1  
  
   
As    
Begin  
  
 -- Searches for Customers based on given parameters    
  
 declare @customerid int = null;  
  
 SELECT TOP 1 @customerid = customerid FROM  [dbo].[Customer] where EmailAddress=@rvcEmailAddress and Password=@rvcPassword;-- and StatusId=@rvcStatusId;  
   
 exec [dbo].[SG2_usp_Customers_Get] @customerid  
   
      
End  
  